using BugSense;
using BugSense.Core.Model;
using Jirabox.Common;
using Jirabox.Common.Enumerations;
using Jirabox.Core.Contracts;
using Jirabox.Core.ExceptionExtension;
using Jirabox.Model;
using Jirabox.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Jirabox.Services
{
    public class JiraService : IJiraService
    {       
        private IHttpManager httpManager;
        private IDialogService dialogService;
        private ICacheDataService cacheDataService;
        public CancellationTokenSource tokenSource = null;

        public JiraService(IHttpManager httpManager, IDialogService dialogService, ICacheDataService cacheDataService)
        {
            this.httpManager = httpManager;
            this.dialogService = dialogService;
            this.cacheDataService = cacheDataService;
            tokenSource = new CancellationTokenSource();
        }

        public async Task<bool> LoginAsync(string serverUrl, string username, string password, CancellationTokenSource cancellationTokenSource)
        {            
            var requestUrl = string.Format("{0}{1}/", App.BaseUrl, JiraRequestType.Search.ToString().ToLower());
            var response = await httpManager.GetAsync(requestUrl, true, username, password);

            if (response == null) return false;
            if (response.IsSuccessStatusCode) return true;

            throw new HttpRequestStatusCodeException(response.StatusCode);
        }

        public async Task<ObservableCollection<Project>> GetProjects(string serverUrl, string username, string password)
        {
            const string cacheFileName = "Projects.cache";

            //Check cache data
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var projectListFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<ObservableCollection<Project>>(projectListFile);
            }

            var requestUrl = string.Format("{0}{1}/", App.BaseUrl, JiraRequestType.Project.ToString().ToLower());
            var projects = new ObservableCollection<Project>();
            try
            {
                var response = await httpManager.GetAsync(requestUrl, true, username, password);
                response.EnsureSuccessStatusCode();
                var responseStr = await response.Content.ReadAsStringAsync();
                projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(responseStr);

                //Download all project images if they do not exist
                foreach (var project in projects)
                {
                    await DownloadImage(project.AvatarUrls.Size48, project.Key);
                }

                //Save new data to the cache
                cacheDataService.Save(cacheFileName, responseStr);
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetProjects"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return projects;
        }

        public async Task<Project> GetProjectByKey(string serverUrl, string username, string password, string key)
        {
            var cacheFileName = string.Format("ProjectByKey.{0}.cache", key);
            //Check cache file is exist            
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var projectFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<Project>(projectFile);
            }
            
            var requestUrl = string.Format("{0}{1}/{2}", App.BaseUrl, JiraRequestType.Project.ToString().ToLower(), key);
            Project project = null;
            HttpResponseMessage response = null;
            try
            {
                response = await httpManager.GetAsync(requestUrl, true, username, password);
                response.EnsureSuccessStatusCode();
                var responseStr = await response.Content.ReadAsStringAsync();
                project = JsonConvert.DeserializeObject<Project>(responseStr);

                //Download project and project lead images if they do not exist
                await DownloadImage(project.AvatarUrls.Size48, key);
                await DownloadImage(project.Lead.AvatarUrls.Size48, project.Lead.UserName);

                //Save new data to the cache
                cacheDataService.Save(cacheFileName, responseStr);
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetProjectByKey"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return project;
        }

        public async Task<Issue> GetIssueByKey(string serverUrl, string username, string password, string key)
        {
            HttpResponseMessage response = null;
            Issue issue = null;            
            var requestUrl = string.Format("{0}{1}/{2}?expand=changelog", App.BaseUrl, JiraRequestType.Issue.ToString().ToLower(CultureInfo.InvariantCulture), key);
            try
            {
                response = await httpManager.GetAsync(requestUrl, true, username, password);
                response.EnsureSuccessStatusCode();


                var responseStr = await response.Content.ReadAsStringAsync();
                issue = JsonConvert.DeserializeObject<Issue>(responseStr);

                var assignee = issue.Fields.Assignee;
                var reporter = issue.Fields.Reporter;
                var comments = issue.Fields.Comment.Comments;

                //Download assignee and reporter images if necessary
                if (assignee != null)
                    await DownloadImage(assignee.AvatarUrls.Size48, assignee.UserName);

                if (reporter != null)
                    await DownloadImage(reporter.AvatarUrls.Size48, reporter.UserName);

                //Download comment author images if necessary
                foreach (var comment in comments)
                {
                    await DownloadImage(comment.Author.AvatarUrls.Size48, comment.Author.UserName);
                }
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetIssueByKey"
                });
                extras.Add(new CrashExtraData
                {
                    Key = "Request Url",
                    Value = requestUrl
                });

                BugSenseHandler.Instance.LogException(exception, extras);
                dialogService.ShowDialog(AppResources.ErrorMessage, "Error");
            }
            return issue;
        }

        public async Task<ObservableCollection<Issue>> Search(string searchText, bool assignedToMe = false, bool reportedByMe = false, CancellationTokenSource tokenSource = null)
        {
            var fields = new List<string> { "summary", "status", "assignee", "reporter", "description", "issuetype", "priority", "comment" };
            var expands = new List<string> { "changelog"};
            var url = string.Format("{0}{1}", App.BaseUrl, JiraRequestType.Search.ToString().ToLower());
            var jql = string.Empty;

            if (!string.IsNullOrEmpty(searchText))
            {
                jql += string.Format("text ~ {0}", searchText);
            }
            if (assignedToMe)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    jql += string.Format("{0} AND assignee = currentUser()", jql);
                }
                else
                {
                    jql += "assignee = currentUser()";
                }
            }
            if (reportedByMe)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    jql += string.Format("{0} AND reporter = currentUser()", jql);
                }
                else
                {
                    jql += "reporter = currentUser()";
                }
            }

            var request = new SearchRequest();
            request.Fields = fields;
            request.Expands = expands;
            request.JQL = jql;
            request.MaxResults = 50;
            request.StartAt = 0;

            var extras = BugSenseHandler.Instance.CrashExtraData;
            string data = JsonConvert.SerializeObject(request);
            try
            {
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.Search"
                });
                extras.Add(new CrashExtraData
                {
                    Key = "Request Url",
                    Value = url
                });
                extras.Add(new CrashExtraData
                {
                    Key = "Post Data",
                    Value = data
                });
                
                var result = await httpManager.PostAsync(url, data, true, App.UserName, App.Password, tokenSource);
                if (result == null) return null;
                result.EnsureSuccessStatusCode();
                var responseString = await result.Content.ReadAsStringAsync();
                extras.Add(new CrashExtraData
                {
                    Key = "Response String",
                    Value = responseString
                });
                var response = JsonConvert.DeserializeObject<SearchResponse>(responseString);
                return new ObservableCollection<Issue>(response.Issues);
            }          
            catch (Exception exception)
            {                
                BugSenseHandler.Instance.LogException(exception, extras);
            }            
            return null;
        }

        public async Task<ObservableCollection<Issue>> GetIssuesByProjectKey(string serverUrl, string username, string password, string key)
        {
            var cacheFileName = string.Format("IssuesByProjectKey.{0}.cache", key);
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var issuesFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<ObservableCollection<Issue>>(issuesFile);
            }
            
            var requestUrl = string.Format("{0}{1}/{2}", App.BaseUrl, JiraRequestType.Issue.ToString().ToLower(CultureInfo.InvariantCulture), key);
            string jql = "project = " + key;
            var issues = await GetIssues(jql);            

            //Save issues to the cache
            cacheDataService.Save(cacheFileName, JsonConvert.SerializeObject(issues));
            return issues;
        }

        public async Task<ObservableCollection<Issue>> GetIssues(string jql, List<string> fields = null, int startAt = 0, int maxResult = 50)
        {
            fields = fields ?? new List<string> { "summary", "status", "assignee", "reporter", "description", "issuetype", "priority", "comment" };
            var requestUrl = string.Format("{0}{1}", App.BaseUrl, JiraRequestType.Search.ToString().ToLower());

            var request = new SearchRequest();
            request.Fields = fields;
            request.JQL = jql;
            request.MaxResults = maxResult;
            request.StartAt = startAt;

            string data = JsonConvert.SerializeObject(request);
            SearchResponse searchResponse = null;
            try
            {
                var response = await httpManager.PostAsync(requestUrl, data, true, App.UserName, App.Password);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                searchResponse = JsonConvert.DeserializeObject<SearchResponse>(responseString);
                return new ObservableCollection<Issue>(searchResponse.Issues);
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Post Data",
                    Value = data
                });
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetIssues"
                });

                BugSenseHandler.Instance.LogException(exception, extras); 
            }
            return null;
        }

        public async Task<ObservableCollection<IssueType>> GetIssueTypesOfProject(string projectKey)
        {
            var cacheFileName = string.Format("IssueTypesOfProject.{0}.cache", projectKey);

            //Check cache data file
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var issueTypesFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<ObservableCollection<IssueType>>(issueTypesFile);
            }

            var requestUrl = string.Format("{0}{1}/createmeta?projectKeys={2}", App.BaseUrl, JiraRequestType.Issue.ToString().ToLower(CultureInfo.InvariantCulture), projectKey);
            ObservableCollection<IssueType> issueTypes = null;
            try
            {
                var result = await httpManager.GetAsync(requestUrl, true, App.UserName, App.Password);
                result.EnsureSuccessStatusCode();
                var responseString = await result.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<IssueTypeResponse>(responseString);
                if (responseObj.Projects.Count > 0)
                {
                    var issueTypeList = responseObj.Projects[0].IssueTypes.Where(item => item.IsSubtask == false);
                    foreach (var issueType in issueTypeList)
                    {
                        await DownloadImage(issueType.IconUrl, issueType.Name);
                    }
                    issueTypes = new ObservableCollection<IssueType>(issueTypeList);

                    //Save new file to the cache
                    cacheDataService.Save(cacheFileName, JsonConvert.SerializeObject(issueTypes));
                }
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetIssueTypesOfProject"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return issueTypes;
        }

        public async Task<CreateIssueResponse> CreateIssue(CreateIssueRequest request)
        {
            var url = string.Format("{0}{1}/", App.BaseUrl, JiraRequestType.Issue.ToString().ToLower(CultureInfo.InvariantCulture));
            var data = JsonConvert.SerializeObject(request);
            CreateIssueResponse createIssueResponse = null;
           
            try
            {
                var result = await httpManager.PostAsync(url, data, true, App.UserName, App.Password);
                result.EnsureSuccessStatusCode();
                var responseString = await result.Content.ReadAsStringAsync();
                createIssueResponse = JsonConvert.DeserializeObject<CreateIssueResponse>(responseString);
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.CreateIssue"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return createIssueResponse;
        }

        public async Task<ObservableCollection<Priority>> GetPriorities()
        {
            const string cacheFileName = "Priorities.cache";
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var prioritiesFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<ObservableCollection<Priority>>(prioritiesFile);
            }

            var url = string.Format("{0}{1}", App.BaseUrl, JiraRequestType.Priority.ToString().ToLower());
            ObservableCollection<Priority> priorityList = null;
            try
            {
                var result = await httpManager.GetAsync(url, true, App.UserName, App.Password);
                result.EnsureSuccessStatusCode();
                var responseString = await result.Content.ReadAsStringAsync();
                priorityList = new ObservableCollection<Priority>(JsonConvert.DeserializeObject<List<Priority>>(responseString));

                foreach (var priority in priorityList)
                {
                    await DownloadImage(priority.IconUrl, priority.Name);
                }

                //Save new data to the cache
                cacheDataService.Save(cacheFileName, JsonConvert.SerializeObject(priorityList));

            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetPriorities"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return priorityList;
        }

        public async Task<User> GetUserProfileAsync(string username)
        {
            var cacheFileName = string.Format("UserProfile.{0}.cache", username);

            //Check cache file 
            var isCacheExist = await cacheDataService.DoesFileExist(cacheFileName);
            if (isCacheExist)
            {
                var userProfileFile = await cacheDataService.Get(cacheFileName);
                return JsonConvert.DeserializeObject<User>(userProfileFile);
            }

            string url = string.Format("{0}user?username={1}", App.BaseUrl, username);
            var response = await httpManager.GetAsync(url, true, username, App.Password);
            response.EnsureSuccessStatusCode();
            User user = null;
            try
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(responseStr);
                var userDisplayPictureUrl = user.AvatarUrls.Size48.Substring(0, user.AvatarUrls.Size48.Length - 2) + "183";

                await DownloadImage(userDisplayPictureUrl, user.UserName);
                App.User = user;

                //Save new data to the cache
                cacheDataService.Save(cacheFileName, responseStr);                

            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetUserProfileAsync"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return user;
        }

        public async Task<bool> AddComment(string issueKey, string comment)
        {
            HttpResponseMessage response = null;
            var requestUrl = string.Format("{0}{1}/{2}/comment", App.BaseUrl, JiraRequestType.Issue.ToString().ToLower(CultureInfo.InvariantCulture), issueKey);

            var commentRequest = new AddCommentRequest();
            commentRequest.Body = comment;

            var data = JsonConvert.SerializeObject(commentRequest);
            try
            {
                response = await httpManager.PostAsync(requestUrl, data, true, App.UserName, App.Password);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.AddComment"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return true;
            return false;
        }

        public BitmapImage GetDisplayPicture(string username)
        {
            byte[] data;
            var bitmapImage = new BitmapImage();
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var path = Path.Combine("Images", username + ".png");
                    if (!isf.FileExists(path)) return null;

                    using (var fs = isf.OpenFile(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        data = new byte[fs.Length];
                        if (data.Length == 0) return null;
                        fs.Read(data, 0, data.Length);
                        using (var ms = new MemoryStream(data))
                        {
                            bitmapImage.SetSource(ms);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.GetDisplayPicture"
                });

                BugSenseHandler.Instance.LogException(exception, extras);
            }
            return bitmapImage;
        }

        private async Task DownloadImage(string url, string filename)
        {
            try
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!isf.DirectoryExists("Images"))
                    {
                        isf.CreateDirectory("Images");
                    }

                    var path = Path.Combine("Images", filename.Replace(":", ".").Replace(" ", "") + ".png");
                    if (isf.FileExists(path)) return;

                    var result = await httpManager.GetAsync(url, true, App.UserName, App.Password);
                    if (result.IsSuccessStatusCode)
                    {
                        var contentResult = await result.Content.ReadAsByteArrayAsync();

                        var image = new BitmapImage();
                        var stream = new MemoryStream(contentResult);
                        image.SetSource(stream);
                        var wb = new WriteableBitmap(image);

                        using (var fs = isf.CreateFile(path))
                        {
                            PNGWriter.WritePNG(wb, fs);
                        }
                    }
                }
            }
            catch(IsolatedStorageException storageException)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.DownloadImage.IsolatedStorageException"
                });

                BugSenseHandler.Instance.LogException(storageException, extras);                

            }
            catch (Exception exception)
            {
                var extras = BugSenseHandler.Instance.CrashExtraData;
                extras.Add(new CrashExtraData
                {
                    Key = "Method",
                    Value = "JiraService.DownloadImage"
                });              

                BugSenseHandler.Instance.LogException(exception, extras);                
            }
        }       
    }
}

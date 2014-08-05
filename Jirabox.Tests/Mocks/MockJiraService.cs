using Jirabox.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.Tests.Mocks
{
    public class MockJiraService : IJiraService
    {
        public string BaseUrl { get; set; }       

        public async Task<bool> LoginAsync(string serverUrl, string username, string password, CancellationTokenSource cancellationTokenSource)
        {
            var server = "https://dev.atlassian.com/jira";
            var userName = "jirabox";
            var pass = "123456";
            if (serverUrl == server && username == userName && password == pass)
                return true;
            return false;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.Project>> GetProjects(string serverUrl, string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Model.User> GetUserProfileAsync(string username)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Media.Imaging.BitmapImage GetDisplayPicture(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<Model.Project> GetProjectByKey(string serverUrl, string username, string password, string key)
        {
            throw new NotImplementedException();
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.Issue>> GetIssuesByProjectKey(string serverUrl, string username, string password, string key)
        {
            throw new NotImplementedException();
        }

        public async Task<Model.Issue> GetIssueByKey(string serverUrl, string username, string password, string key)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddComment(string issueKey, string comment)
        {
            throw new NotImplementedException();
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.IssueType>> GetIssueTypesOfProject(string projectKey)
        {
            throw new NotImplementedException();
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.Priority>> GetPriorities()
        {
            throw new NotImplementedException();
        }

        public async Task<Model.CreateIssueResponse> CreateIssue(Model.CreateIssueRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.Issue>> Search(string searchText, bool assignedToMe = false, bool reportedByMe = false, CancellationTokenSource tokenSource = null)
        {
            throw new NotImplementedException();
        }


        public Task<System.Collections.ObjectModel.ObservableCollection<Model.Transition>> GetTransitions(string issueKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PerformTransition(string issueKey, string transitionId, string comment)
        {
            throw new NotImplementedException();
        }


        public Task<bool> PerformTransition(string issueKey, string transitionId)
        {
            throw new NotImplementedException();
        }


        public Task<System.Collections.ObjectModel.ObservableCollection<Model.Project>> GetProjects(string serverUrl, string username, string password, bool withoutCache = false)
        {
            throw new NotImplementedException();
        }
    }
}

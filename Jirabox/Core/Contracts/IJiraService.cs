using Jirabox.Model;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.Core.Contracts
{
    public interface IJiraService
    {        
        Task<bool> LoginAsync(string serverUrl, string username, string password, CancellationTokenSource cancellationTokenSource);

        Task<ObservableCollection<Project>> GetProjects(string serverUrl, string username, string password, bool withoutCache = false);

        Task<User> GetUserProfileAsync(string username);

        byte[] GetDisplayPicture(string username);

        Task<Project> GetProjectByKey(string serverUrl, string username, string password, string key, bool withoutCache = false);

        Task<ObservableCollection<Issue>> GetIssuesByProjectKey(string serverUrl, string username, string password, string key, bool withoutCache = false);

        Task<Issue> GetIssueByKey(string serverUrl, string username, string password, string key);

        Task<bool> AddComment(string issueKey, string comment);

        Task<ObservableCollection<IssueType>> GetIssueTypesOfProject(string projectKey);

        Task<ObservableCollection<Priority>> GetPriorities();

        Task<CreateIssueResponse> CreateIssue(CreateIssueRequest request);

        Task<ObservableCollection<Issue>> Search(string searchText, bool assignedToMe = false, bool reportedByMe = false, bool isFavourite = false, CancellationTokenSource tokenSource = null);
       
        Task<ObservableCollection<Transition>> GetTransitions(string issueKey);

        Task<bool> PerformTransition(string issueKey, string transitionId);

        Task<ObservableCollection<Favourite>> GetFavourites();

        Task<bool> LogWork(string issueKey, string startedDate, string worked, string comment, CancellationTokenSource tokenSource = null);

        Task<bool> DownloadAttachment(string fileUrl, string fileName);
    }
}

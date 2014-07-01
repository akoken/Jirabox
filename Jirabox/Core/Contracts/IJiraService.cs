using Jirabox.Model;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Jirabox.Core.Contracts
{
    public interface IJiraService
    {        
        Task<bool> LoginAsync(string serverUrl, string username, string password, CancellationTokenSource cancellationTokenSource);

        Task<ObservableCollection<Project>> GetProjects(string serverUrl, string username, string password);

        Task<User> GetUserProfileAsync(string username);

        BitmapImage GetDisplayPicture(string username);

        Task<Project> GetProjectByKey(string serverUrl, string username, string password, string key);

        Task<ObservableCollection<Issue>> GetIssuesByProjectKey(string serverUrl, string username, string password, string key);

        Task<Issue> GetIssueByKey(string serverUrl, string username, string password, string key);

        Task<bool> AddComment(string issueKey, string comment);

        Task<ObservableCollection<IssueType>> GetIssueTypesOfProject(string projectKey);

        Task<ObservableCollection<Priority>> GetPriorities();

        Task<CreateIssueResponse> CreateIssue(CreateIssueRequest request);

        Task<ObservableCollection<Issue>> Search(string searchText, bool assignedToMe = false, bool reportedByMe = false, CancellationTokenSource tokenSource = null);

        
    }
}

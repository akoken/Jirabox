using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class ProjectDetailViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;        
        private Mock<IJiraService> jiraServiceMock;        
        private ProjectDetailViewModel projectDetailViewModel;

        [TestInitialize]
        public void Initialize()
        {            
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();         
            projectDetailViewModel = new ProjectDetailViewModel(navigationServiceMock.Object, jiraServiceMock.Object);           
        }

        [TestMethod]
        public void Initialize_Should_GetProjectAndIssues()
        {
            //Arrange
            var project = new Project { Name = "ALM" };
            var issues = new ObservableCollection<Issue>{new Issue{ProxyKey="ALM"}};
            jiraServiceMock.Setup(x => x.GetProjectByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(project));
            jiraServiceMock.Setup(x => x.GetIssuesByProjectKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(issues));
            //Act
            projectDetailViewModel.Initialize("ALM");

            //Assert
            Assert.IsNotNull(projectDetailViewModel.Project);
            Assert.IsNotNull(projectDetailViewModel.Issues);
        }

        [TestMethod]
        public void When_ShowIssueDetailCommand_Executed_NavigatesToIssueDetailViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>())).Verifiable();
            var issue = new Issue { ProxyKey = "ALM-100" };           

            //Act
            projectDetailViewModel.ShowIssueDetailCommand.Execute(issue);

            //Assert 
            navigationServiceMock.Verify(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>()));
        }

        [TestMethod]
        public void When_CreateIssueCommand_Executed_NavigatesToCreateIssueViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<CreateIssueViewModel>(It.IsAny<Project>())).Verifiable();
            var project = new Project();

            //Act
            projectDetailViewModel.CreateIssueCommand.Execute(project);

            //Assert 
            navigationServiceMock.Verify(x => x.Navigate<CreateIssueViewModel>(It.IsAny<Project>()));
        }

        [TestMethod]
        public void When_RefreshCommand_Executed_NavigatesToCreateIssueViewModel()
        {           
            //Arrange
            var projectKey = "ALM";
            var project = new Project { Name = projectKey };
            var issues = new ObservableCollection<Issue>();
            jiraServiceMock.Setup(x => x.GetProjectByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(project));
            jiraServiceMock.Setup(x => x.GetIssuesByProjectKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult(issues));
            projectDetailViewModel.Key = projectKey;

            //Act
            projectDetailViewModel.RefreshCommand.Execute(projectKey);

            //Assert
            Assert.IsTrue(projectDetailViewModel.Project != null);
            Assert.IsTrue(projectDetailViewModel.Issues != null);
        }       
    }
}

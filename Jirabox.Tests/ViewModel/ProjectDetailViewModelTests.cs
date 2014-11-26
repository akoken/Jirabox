using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Threading.Tasks;

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
            jiraServiceMock.Setup(x => x.GetProjectByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(Task.FromResult<Project>(project));
            
            //Act
            projectDetailViewModel.Initialize("ALM");

            //Assert
            Assert.IsTrue(projectDetailViewModel.Project != null);
            Assert.IsTrue(projectDetailViewModel.Issues == null);
        }

        [TestMethod]
        public void ShowIssueDetailCommand_Execution_NavigatesToIssueDetailViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>())).Verifiable();
            var issue = new Issue { ProxyKey = "ALM-100" };           

            //Act
            projectDetailViewModel.ShowIssueDetailCommand.Execute(issue);

            //Assert
            navigationServiceMock.Verify(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>()));
        }
    }
}

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
    public class IssueDetailViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private IssueDetailViewModel issueDetailViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();
            issueDetailViewModel = new IssueDetailViewModel(jiraServiceMock.Object, navigationServiceMock.Object);
        }

        [TestMethod]
        public void Verify_DownloadAttachmentCommand_Executed()
        {
            //Arrange
            jiraServiceMock.Setup(x => x.DownloadAttachment(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true)).Verifiable();

            //Act
            issueDetailViewModel.DownloadAttachmentCommand.Execute(new Attachment());

            //Assert
            jiraServiceMock.Verify(x => x.DownloadAttachment(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void When_LogworkCommand_Executed_NavigatesToLogWorkViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<LogWorkViewModel>(It.IsAny<object>())).Verifiable();
            issueDetailViewModel.Issue = new Issue { ProxyKey = "ALM-100" };

            //Act
            issueDetailViewModel.LogWorkCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.Navigate<LogWorkViewModel>(It.IsAny<object>()));
        }

        [TestMethod]
        public void When_Transaction_IsNull_ChangeStatusCommand_Fails()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<ChangeStatusViewModel>(null)).Verifiable();

            //Act
            issueDetailViewModel.ChangeStatusCommand.Execute(null);
            
            //Assert
            navigationServiceMock.Verify(x => x.Navigate<ChangeStatusViewModel>(null), Times.Never(), "ChangeStatusCommand cannot be executed. Because there is no any transition");
        }

        [TestMethod]
        public void When_AddCommentCommand_Executed_NavigatesToAddCommentViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<AddCommentViewModel>(It.IsAny<string>())).Verifiable();
            issueDetailViewModel.Issue = new Issue { ProxyKey = "ALM-100" };
            //Act
            issueDetailViewModel.AddCommentCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.Navigate<AddCommentViewModel>(It.IsAny<string>()));
        }

        [TestMethod]
        public void Initialize_OnNavigated_ReturnsIssue()
        {
            //Arrange
            var transitions = new ObservableCollection<Transition>();
            jiraServiceMock.Setup(x => x.GetIssueByKey(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new Issue { ProxyKey = "ALM-100"}));
            jiraServiceMock.Setup(x => x.GetTransitions(It.IsAny<string>())).Returns(Task.FromResult(transitions));

            //Act
            issueDetailViewModel.Initialize("ALM-100");

            //Assert
            Assert.IsTrue(issueDetailViewModel.IsDataLoaded);
            Assert.IsNotNull(issueDetailViewModel.Issue);
        }

        [TestMethod]
        public void Verify_CleanUp()
        {
            //Arrange
            issueDetailViewModel.IsDataLoaded = false;
            issueDetailViewModel.Issue = new Issue();

            //Act
            issueDetailViewModel.CleanUp();

            //Assert
            Assert.IsTrue(issueDetailViewModel.IsDataLoaded);
            Assert.IsNull(issueDetailViewModel.Issue);
        }
    }
}

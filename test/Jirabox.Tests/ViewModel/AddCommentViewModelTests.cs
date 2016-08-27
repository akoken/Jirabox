using Jirabox.Core.Contracts;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Threading.Tasks;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class AddCommentViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private Mock<IDialogService> dialogServiceMock;
        private AddCommentViewModel addCommentViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            addCommentViewModel = new AddCommentViewModel(jiraServiceMock.Object, dialogServiceMock.Object, navigationServiceMock.Object);
        }

        [TestMethod]
        public void Initialize_OnNavigated_SetsIssueKey()
        {
            //Arrange
            const string issueKey = "ALM-100";

            //Act
            addCommentViewModel.Initialize(issueKey);

            //Assert
            Assert.IsTrue(!string.IsNullOrEmpty(addCommentViewModel.IssueKey));
            Assert.IsTrue(addCommentViewModel.IssueKey.Equals(issueKey));            
        }

        [TestMethod]
        public void CleanUp_Should_ClearIssueKey()
        {
            //Arrange
            const string issueKey = "ALM-100";
            addCommentViewModel.Initialize(issueKey);

            //Act
            addCommentViewModel.CleanUp();
            
            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(addCommentViewModel.IssueKey));
            Assert.IsTrue(string.IsNullOrEmpty(addCommentViewModel.CommentText));
        }

        [TestMethod]
        public void When_SendCommand_Executed_NavigatesBack()
        {
            //Arrange
            jiraServiceMock.Setup(x => x.AddComment(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();

            //Act
            addCommentViewModel.SendCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.GoBack());
        }

        [TestMethod]
        public void When_CancelCommand_Executed_NavigatesBack()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();

            //Act
            addCommentViewModel.CancelCommand.Execute(null);

            //Assert            
            navigationServiceMock.Verify(x => x.GoBack());
        }
    }
}

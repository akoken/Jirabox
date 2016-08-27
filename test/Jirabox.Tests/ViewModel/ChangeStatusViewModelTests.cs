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
    public class ChangeStatusViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private Mock<IDialogService> dialogServiceMock;
        private ChangeStatusViewModel changeStatusViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            changeStatusViewModel = new ChangeStatusViewModel(navigationServiceMock.Object, dialogServiceMock.Object, jiraServiceMock.Object);
        }

        [TestMethod]
        public void Initialize_OnNavigated_GetsNavigationParameters()
        {
            //Arrange
            const string issueKey = "ALM-100";
            var transitions = new ObservableCollection<Transition> {new Transition {Id = "1"}};
            var statusPackage = new StatusPackage
            {
                SelectedIssue = new Issue {ProxyKey = issueKey},
                Transitions = transitions
            };
            navigationServiceMock.Setup(x => x.GetNavigationParameter()).Returns(statusPackage);

            //Act
            changeStatusViewModel.Initialize();

            //Assert
            Assert.IsTrue(changeStatusViewModel.SelectedIssue.ProxyKey.Equals(issueKey));
            Assert.IsNotNull(changeStatusViewModel.Transitions);
            Assert.IsTrue(changeStatusViewModel.Transitions.Count > 0);
        }

        [TestMethod]
        public void When_ChangeStatusCommand_Executed_NavigatesBack()
        {
            //Arrange
            const string issueKey = "ALM-100";
            var transitions = new ObservableCollection<Transition> { new Transition { Id = "1" } };
            var statusPackage = new StatusPackage
            {
                SelectedIssue = new Issue { ProxyKey = issueKey },
                Transitions = transitions
            };
            navigationServiceMock.Setup(x => x.GetNavigationParameter()).Returns(statusPackage);
            
            changeStatusViewModel.Initialize();
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();
            jiraServiceMock.Setup(x => x.PerformTransition(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            //Act
            changeStatusViewModel.ChangeStatusCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.GoBack());
        }

        [TestMethod]
        public void When_CancelCommand_Executed_NavigatesBack()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();

            //Act
            changeStatusViewModel.CancelCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.GoBack());
        }

    }
}

using System;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class CreateIssueViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private Mock<IDialogService> dialogServiceMock;
        private CreateIssueViewModel createIssueViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();
            dialogServiceMock = new Mock<IDialogService>();
            createIssueViewModel = new CreateIssueViewModel(jiraServiceMock.Object,dialogServiceMock.Object, navigationServiceMock.Object);
        }

        [TestMethod]
        public void Initialize_OnNavigated_GetsProject()
        {
            //Arrange
            var issueTypes = new ObservableCollection<IssueType>{new IssueType{Name = "Bug"}};
            var priorities = new ObservableCollection<Priority>{new Priority{ Name = "Major"}};

            jiraServiceMock.Setup(x => x.GetIssueTypesOfProject(It.IsAny<string>())).Returns(Task.FromResult(issueTypes));
            jiraServiceMock.Setup(x => x.GetPriorities()).Returns(Task.FromResult(priorities));
            navigationServiceMock.Setup(x => x.GetNavigationParameter()).Returns(new Project { Key = "ALM" });

            //Act
            createIssueViewModel.Initialize();

            //Assert
            Assert.IsNotNull(createIssueViewModel.IssueTypes);
            Assert.IsNotNull(createIssueViewModel.PriorityList);
        }

        [TestMethod]
        public void Initialize_ProjectIsNull_ReturnsNullIssueTypes()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.GetNavigationParameter()).Returns(null);

            //Act
            createIssueViewModel.Initialize();
            
            //Assert
            Assert.IsNull(createIssueViewModel.IssueTypes);
            Assert.IsNull(createIssueViewModel.PriorityList);
        }

        [TestMethod]
        public void When_CreateIssueCommand_Executed_NavigatesBack()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();
            jiraServiceMock.Setup(x => x.CreateIssue(It.IsAny<CreateIssueRequest>())).Returns(Task.FromResult(new CreateIssueResponse {Key = "ALM"}));
            createIssueViewModel.Project = new Project {Key = "ALM"};
            createIssueViewModel.Description = "Application Lifecycle Management";
            createIssueViewModel.Summary = "Application Lifecycle Management";
            createIssueViewModel.IssueTypes = new ObservableCollection<IssueType> {new IssueType {Name = "Task"}};
            createIssueViewModel.PriorityList = new ObservableCollection<Priority>{new Priority {Id = "3"}};
            createIssueViewModel.SelectedIssueTypeIndex = 0;
            createIssueViewModel.SelectedPriorityIndex = 0;
                
            //Act
            createIssueViewModel.CreateIssueCommand.Execute(null);

            //Assert
            navigationServiceMock.Verify(x => x.GoBack());
        }

        [TestMethod]
        public void Cancel_Should_ClearProperties()
        {
            //Act
            createIssueViewModel.CleanUp();

            //Assert
            Assert.IsNull(createIssueViewModel.Project);
            Assert.IsTrue(String.IsNullOrEmpty(createIssueViewModel.Summary));
            Assert.IsTrue(String.IsNullOrEmpty(createIssueViewModel.Description));
        }

        [TestMethod]
        public void When_CancelCommand_Executed_NavigatesBack()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.GoBack()).Verifiable();

            //Act
            createIssueViewModel.CancelCommand.Execute(null);

            //Assert
            Assert.IsNull(createIssueViewModel.Project);
            Assert.IsTrue(String.IsNullOrEmpty(createIssueViewModel.Summary));
            Assert.IsTrue(String.IsNullOrEmpty(createIssueViewModel.Description));
            navigationServiceMock.Verify(x => x.GoBack());
        }

        [TestMethod]
        public void ValidateParameters_EmptySummary_ReturnsFalse()
        {
            //Arrange
            const bool expected = false;
            createIssueViewModel.Summary = string.Empty;

            //Act
            var actual = createIssueViewModel.ValidateParameters();

            //Assert
            Assert.IsTrue(actual == expected);
        }

        [TestMethod]
        public void ValidateParameters_ValidSummary_ReturnsTrue()
        {
            //Arrange
            const bool expected = true;
            createIssueViewModel.Summary = "This is a valid summary.";

            //Act
            var actual = createIssueViewModel.ValidateParameters();

            //Assert
            Assert.IsTrue(actual == expected);
        }
    }
}

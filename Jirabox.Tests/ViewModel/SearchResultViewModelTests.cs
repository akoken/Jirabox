using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class SearchResultViewModelTests
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private SearchResultViewModel searchResultViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();
            searchResultViewModel = new SearchResultViewModel(jiraServiceMock.Object, navigationServiceMock.Object);
        }

        [TestMethod]
        public void Initialize_OnNavigation_GetsIssues()
        {
            //Arrange
            var issues = new ObservableCollection<Issue> { new Issue { ProxyKey = "ALM-100" } };
            jiraServiceMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationTokenSource>())).Returns(Task.FromResult(issues));
            navigationServiceMock.Setup(x => x.GetNavigationParameter()).Returns(new SearchParameter { SearchText = "Jirabox", IsFavourite = false });

            //Act
            searchResultViewModel.Initialize();

            //Assert
            Assert.IsNotNull(searchResultViewModel.Issues);
        }

        [TestMethod]
        public void When_ShowIssueDetailCommand_Executed_NavigatesToIssueDetailViewModel()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>())).Verifiable();
            var issue = new Issue { ProxyKey = "ALM-100" };

            //Act
            searchResultViewModel.ShowIssueDetailCommand.Execute(issue);

            //Assert
            navigationServiceMock.Verify(x => x.Navigate<IssueDetailViewModel>(It.IsAny<string>()));
        }

        [TestMethod]
        public void Verify_NavigateToLoginView()
        {
            //Arrange
            navigationServiceMock.Setup(x => x.Navigate<LoginViewModel>(It.IsAny<object>())).Verifiable();

            //Act
            searchResultViewModel.NavigateToLoginView();

            //Assert
            navigationServiceMock.Verify(x => x.Navigate<LoginViewModel>(It.IsAny<object>()));
        }

        [TestMethod]
        public void Verify_SetNavigationSearchText()
        {
            //Arrange
            var searchText = "Jirabox";
            var searchCriteria = new SearchParameter{SearchText = searchText};
            navigationServiceMock.SetupProperty(x => x.NavigationParameter).SetReturnsDefault(searchCriteria);

            //Act
            searchResultViewModel.SetNavigationSearchText(searchText);

            //Assert
            Assert.IsNotNull(navigationServiceMock.Object.NavigationParameter);
            Assert.IsTrue(((SearchParameter)navigationServiceMock.Object.NavigationParameter).SearchText.Equals(searchText));
        }

        [TestMethod]
        public void Verify_SetNavigationToIssuesReportedByMe()
        {
            //Arrange            
            var searchCriteria = new SearchParameter { IsReportedByMe = true };
            navigationServiceMock.SetupProperty(x => x.NavigationParameter).SetReturnsDefault(searchCriteria);

            //Act
            searchResultViewModel.SetNavigationToIssuesReportedByMe();

            //Assert
            Assert.IsNotNull(navigationServiceMock.Object.NavigationParameter);
            Assert.IsTrue(((SearchParameter)navigationServiceMock.Object.NavigationParameter).IsReportedByMe);
        }

        [TestMethod]
        public void Verify_SetNavigationToAssignedIssues()
        {
            //Arrange            
            var searchCriteria = new SearchParameter { IsAssignedToMe = true };
            navigationServiceMock.SetupProperty(x => x.NavigationParameter).SetReturnsDefault(searchCriteria);

            //Act
            searchResultViewModel.SetNavigationToAssignedIssues();

            //Assert
            Assert.IsNotNull(navigationServiceMock.Object.NavigationParameter);
            Assert.IsTrue(((SearchParameter)navigationServiceMock.Object.NavigationParameter).IsAssignedToMe);
        }

        [TestMethod]
        public void Verify_CleanUp()
        {
            //Arrange
            var issues = new ObservableCollection<Issue> { new Issue { ProxyKey = "ALM-100" } };
            searchResultViewModel.Issues = new ObservableCollection<Issue>(issues);

            //Act
            searchResultViewModel.CleanUp();

            //Assert
            Assert.IsNull(searchResultViewModel.Issues);
        }
    }
}

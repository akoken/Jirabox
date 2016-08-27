using Jirabox.Core.Contracts;
using Jirabox.Model;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Threading.Tasks;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class UserProfileViewModelTests
    {        
        private Mock<IJiraService> jiraServiceMock;
        private UserProfileViewModel userProfileViewModel;

        [TestInitialize]
        public void Initialize()
        {
            jiraServiceMock = new Mock<IJiraService>();
            userProfileViewModel = new UserProfileViewModel(jiraServiceMock.Object);
        }

        [TestMethod]
        public void Initialize_OnNavigated_GetsUserProfile()
        {
            //Arrange
            var user = new User {UserName = "Jirabox"};
            jiraServiceMock.Setup(x => x.GetUserProfileAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

            //Act
            userProfileViewModel.Initialize();

            //Assert
            Assert.IsTrue(userProfileViewModel.IsDataLoaded);
            Assert.IsNotNull(userProfileViewModel.User);

        }
    }
}

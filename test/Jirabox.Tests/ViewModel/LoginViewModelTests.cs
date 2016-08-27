using Jirabox.Core.Contracts;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class LoginViewModelTests 
    {
        private Mock<INavigationService> navigationServiceMock;
        private Mock<IDialogService> dialogServiceMock;
        private Mock<IJiraService> jiraServiceMock;
        private LoginViewModel loginViewModel;
      
        [TestInitialize]
        public void InitializeTests()
        {
            dialogServiceMock = new Mock<IDialogService>();            
            jiraServiceMock = new Mock<IJiraService>();
            navigationServiceMock = new Mock<INavigationService>();                       
            loginViewModel = new LoginViewModel(navigationServiceMock.Object, dialogServiceMock.Object, jiraServiceMock.Object);
        }

        [TestMethod]        
        public void ValidateUrl_InvalidServerUrl_ReturnsFalse()
        {         
            //Arrange
            string url = "dev.atlassian.com";

            //Act
            var result = loginViewModel.ValidateUrl(url);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateUrl_ValidServerUrl_ReturnsTrue()
        {
            //Arrange
            loginViewModel.ServerUrl = "https://dev.atlassian.com/jira";
            //Act
            var result = loginViewModel.ValidateUrl(loginViewModel.ServerUrl);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void IsInputsValid_EmptyUserName_ReturnsFalse()
        {
            //Arrange          
            loginViewModel.UserName = string.Empty;
            loginViewModel.Password = "password";

            //Act
            var result = loginViewModel.IsInputsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInputsValid_EmptyPassword_ReturnsFalse()
        {
            //Arrange         
            loginViewModel.UserName = "username";
            loginViewModel.Password = string.Empty;

            //Act
            var result = loginViewModel.IsInputsValid();

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInputsValid_ValidUserNameAndPassword_ReturnsTrue()
        {
            //Arrange        
            loginViewModel.UserName = "username";
            loginViewModel.Password = "password";

            //Act
            var result = loginViewModel.IsInputsValid();

            //Assert
            Assert.IsTrue(result);
        }
    }
}

using GalaSoft.MvvmLight.Ioc;
using Jirabox.Core.Contracts;
using Jirabox.Tests.Mocks;
using Jirabox.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Jirabox.Tests
{
    [TestClass] 
    public class LoginViewModelTests
    {
        [TestInitialize]
        public void Initialize()
        {
            SimpleIoc.Default.Register<INavigationService, MockNavigationService>();
            SimpleIoc.Default.Register<IJiraService, MockJiraService>();
            SimpleIoc.Default.Register<IDialogService, MockDialogService>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }

        [TestMethod]
        public void LoginViewModel_Instantiation()
        {
            var loginViewModel = ServiceLocator.Current.GetInstance<LoginViewModel>();
            Assert.IsNotNull(loginViewModel);
        }

        [TestMethod]
        public void ServerUrl_Should_Be_Valid()
        {
            var loginViewModel = ServiceLocator.Current.GetInstance<LoginViewModel>();
            var serverUrl = "http://dev.atlassian.com/jira";
            var expected = true;
            var actual = loginViewModel.ValidateUrl(serverUrl);
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void ServerUrl_Should_Be_Invalid()
        {
            var loginViewModel = ServiceLocator.Current.GetInstance<LoginViewModel>();
            var serverUrl = "dev.atlassian.com/jira";
            var expected = false;
            var actual = loginViewModel.ValidateUrl(serverUrl);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Login()
        {
            var loginViewModel = ServiceLocator.Current.GetInstance<LoginViewModel>();
            loginViewModel.ServerUrl = "https://dev.atlassian.com/jira";
            loginViewModel.UserName = "jirabox";
            loginViewModel.Password = "123456";
            loginViewModel.Login();            
        }
    }
}

using System.Threading;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Jirabox.Tests.ViewModel
{
    [TestClass]
    public class SettingsViewModelTests
    {
        private Mock<IDialogService> dialogServiceMock;
        private SettingsViewModel settingsViewModel;

        [TestInitialize]
        public void Initialize()
        {            
            dialogServiceMock = new Mock<IDialogService>();
            settingsViewModel = new SettingsViewModel(dialogServiceMock.Object);
        }

        [TestMethod]
        public void When_ClearImageCacheCommand_Executed_ReturnsDialogMessage()
        {
            //Arrange
            dialogServiceMock.Setup(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            settingsViewModel.ClearImageCacheCommand.Execute(null);

            //Assert
            dialogServiceMock.Verify(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<string>()));
        }


        [TestMethod]
        public void When_SaveMaxSearchResultCommand_Executed_SavesInputValue()
        {
            //Arrange
            const int maxCount = 500;
            var maxSearchResultSetting = new IsolatedStorageProperty<int>(Settings.MaxSearchResult, 50);

            //Act
            settingsViewModel.SaveMaxSearchResultCommand.Execute(maxCount);

            //Assert
            Assert.IsTrue(maxSearchResultSetting.Value == maxCount);
        }

        [TestMethod]
        public void When_ClearAttachmentCacheCommand_Executed_ReturnsDialogMessage()
        {
            //Arrange
            dialogServiceMock.Setup(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            settingsViewModel.ClearAttachmentCacheCommand.Execute(null);

            Thread.Sleep(500);
            //Assert
            dialogServiceMock.Verify(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}

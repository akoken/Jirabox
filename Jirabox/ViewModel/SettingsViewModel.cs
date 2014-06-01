using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Common;
using Jirabox.Core.Contracts;
using Jirabox.Resources;
using System;

namespace Jirabox.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private IDialogService dialogService;
        private bool isGroupingEnabled;
        private IsolatedStorageProperty<bool> setting;
        public RelayCommand ClearCacheCommand { get; private set; }

        public bool IsGroupingEnabled
        {
            get { return isGroupingEnabled; }
            set
            {
                if (isGroupingEnabled != value)
                {
                    isGroupingEnabled = value;
                    
                    setting.Value = value;
                    RaisePropertyChanged(() => IsGroupingEnabled);
                    MessengerInstance.Send<bool>(isGroupingEnabled);
                }
            }
        }

        public SettingsViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            setting = new IsolatedStorageProperty<bool>(Settings.IsGroupingEnabled, true);
            IsGroupingEnabled = setting.Value;

            ClearCacheCommand = new RelayCommand(() =>
            {
                try
                {
                    var cacheSetting = new IsolatedStorageProperty<bool>(Settings.ClearImageCache, false);
                    cacheSetting.Value = true;
                    dialogService.ShowDialog(AppResources.ImageCacheCleanedMessage, "Done");
                }
                catch (Exception ex)
                {
                    dialogService.ShowDialog(ex.Message, "Error");
                }

            });
        }
    }
}

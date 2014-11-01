using BugSense;
using BugSense.Core.Model;
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
        private readonly IDialogService dialogService;
        private bool isGroupingEnabled;
        private double maxSearchResult;
        private IsolatedStorageProperty<bool> setting;

        public RelayCommand ClearImageCacheCommand { get; private set; }
        public RelayCommand ClearAttachmentCacheCommand { get; private set; }
        public RelayCommand<int> SaveMaxSearchResultCommand { get; private set; }

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
        public double MaxSearchResult
        {
            get { return maxSearchResult; }
            set
            {
                if (maxSearchResult != value)
                {
                    maxSearchResult = value;
                    RaisePropertyChanged(() => MaxSearchResult);
                }
            }
        }

        public SettingsViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            //Set grouping setting
            setting = new IsolatedStorageProperty<bool>(Settings.IsGroupingEnabled, true);
            IsGroupingEnabled = setting.Value;

            //Set maximum search result setting
            MaxSearchResult = new IsolatedStorageProperty<int>(Settings.MaxSearchResult, 50).Value;

            ClearImageCacheCommand = new RelayCommand(() =>
            {
                try
                {
                    var cacheSetting = new IsolatedStorageProperty<bool>(Settings.ClearImageCache, false);
                    cacheSetting.Value = true;
                    dialogService.ShowDialog(AppResources.ImageCacheClearedMessage, AppResources.Done);
                }
                catch (Exception exception)
                {
                    var extras = BugSenseHandler.Instance.CrashExtraData;
                    extras.Add(new CrashExtraData
                    {
                        Key = "Method",
                        Value = "SettingsViewModel.ClearCacheCommand"
                    });

                    BugSenseHandler.Instance.LogException(exception, extras);
                }

            });

            SaveMaxSearchResultCommand = new RelayCommand<int>((value) =>
            {
                try
                {
                    var maxSearchResultSetting = new IsolatedStorageProperty<int>(Settings.MaxSearchResult, 50);
                    maxSearchResultSetting.Value = value;
                }
                catch (Exception exception)
                {
                    var extras = BugSenseHandler.Instance.CrashExtraData;
                    extras.Add(new CrashExtraData
                    {
                        Key = "Method",
                        Value = "SettingsViewModel.SaveSettingsCommand"
                    });

                    BugSenseHandler.Instance.LogException(exception, extras);
                }
            });

            ClearAttachmentCacheCommand = new RelayCommand(async () =>
            {
                try
                {
                    await StorageHelper.ClearAttachmentCache();
                    dialogService.ShowDialog(AppResources.AttachmentCacheClearedMessage, AppResources.Done);
                }
                catch (Exception exception)
                {
                    var extras = BugSenseHandler.Instance.CrashExtraData;
                    extras.Add(new CrashExtraData
                    {
                        Key = "Method",
                        Value = "SettingsViewModel.ClearAttachmentCacheCommand"
                    });

                    BugSenseHandler.Instance.LogException(exception, extras);
                }
            });
        }
    }
}

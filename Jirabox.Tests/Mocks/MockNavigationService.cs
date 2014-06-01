using Jirabox.Core.Contracts;
using Microsoft.Practices.ServiceLocation;
using System;

namespace Jirabox.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public bool CanGoBack
        {
            get { throw new NotImplementedException(); }
        }

        public object NavigationParameter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void Navigate<TDestinationViewModel>(object parameter = null)
        {            
            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            dialogService.ShowDialog(string.Format("Navigated to {0}", typeof(TDestinationViewModel).Name), "Test Result");
        }

        public void Navigate<TDestinationViewModel>(string parameter)
        {
            var dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            dialogService.ShowDialog(string.Format("Navigated to {0}", typeof(TDestinationViewModel).Name), "Test Result");
        }

        public object GetNavigationParameter()
        {
            throw new NotImplementedException();
        }

        public void RemoveBackEntry()
        {
            throw new NotImplementedException();
        }
    }
}

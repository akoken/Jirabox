using System;
using System.Windows.Controls;

namespace Jirabox.Core.Contracts
{
    /// <summary> 
    /// The NavigationService interface. 
    /// </summary> 
    public interface INavigationService
    {    
        bool CanGoBack { get; }

        object NavigationParameter { get; set; }
      
        void GoBack();
  
        void Navigate<TDestinationViewModel>(object parameter = null);

        void Navigate<TDestinationViewModel>(string parameter);

        object GetNavigationParameter();

        //Removes back entry from stack
        void RemoveBackEntry();
    }
}

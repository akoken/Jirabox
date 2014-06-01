using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Jirabox.Tests.ViewModel
{    
    public class ViewModelLocator
    {     
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }   
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
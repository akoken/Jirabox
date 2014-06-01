using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Windows;

namespace Jirabox.ViewModel
{
    public class CommentDetailViewModel : ViewModelBase
    {
        private IDialogService dialogService;
        private Comment commentDetail;

        public Comment CommentDetail
        {
            get
            { 
                return commentDetail;
            }
            set
            {
                if (commentDetail != value)
                {
                    commentDetail = value;
                    RaisePropertyChanged(() => CommentDetail);
                }
            }
        }
        public RelayCommand<RoutedEventArgs> CloseCommand { get; private set; }

        public CommentDetailViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            CloseCommand = new RelayCommand<RoutedEventArgs>((e) => 
            {
                CommentDetail = null;
                MessengerInstance.Send<RoutedEventArgs>(e);                
            });

            MessengerInstance.Register<Comment>(this, comment => 
            {
                if (comment != null)
                {
                    CommentDetail = comment;
                }
            });
        }

    }
}

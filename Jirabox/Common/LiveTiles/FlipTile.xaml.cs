using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Jirabox.Common.LiveTiles
{    
    public partial class FlipTile : UserControl
    {
        Random random = null;



        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(FlipTile), new PropertyMetadata(null));


        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(FlipTile), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTitleChanged)));


        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FlipTile;
            control.txt1.Text = e.NewValue.ToString();
        }


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(FlipTile), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnMessageChanged)));

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FlipTile;
            control.txt2.Text = e.NewValue.ToString();
        }     

        public FlipTile()
        {
            InitializeComponent();
            random = new Random();
            Storyboard anim = (Storyboard)FindName("liveTileAnim1");            
            anim.Begin();
        }

        private void liveTileAnim1_Completed_1(object sender, EventArgs e)
        {           
            Storyboard anim = (Storyboard)FindName("liveTileAnim1_Inverse");
            anim.Begin();
        }

        private void liveTileAnim1_Inverse_Completed_1(object sender, EventArgs e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnim1");
            anim.Begin();
        }
    }
}

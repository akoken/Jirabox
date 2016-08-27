using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Jirabox.Common.LiveTiles
{
    public partial class ImageTile : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ImageTile), new PropertyMetadata(string.Empty, OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageTileControl = d as ImageTile;
            imageTileControl.txt1.Text = e.NewValue.ToString();
        }


        public BitmapImage DisplayPicture
        {
            get { return (BitmapImage)GetValue(DisplayPictureProperty); }
            set { SetValue(DisplayPictureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayPictureProperty =
            DependencyProperty.Register("DisplayPicture", typeof(BitmapImage), typeof(ImageTile), new PropertyMetadata(null));
   
        public ImageTile()
        {
            InitializeComponent();
            Storyboard anim = (Storyboard)FindName("liveTileAnim1_Part1");
            anim.Begin();
        }
        private void liveTileAnim1_Part1_Completed_1(object sender, EventArgs e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnim1_Part2");
            anim.Begin();
        }

        private void liveTileAnim1_Part2_Completed_1(object sender, EventArgs e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnim2_Part1");
            anim.Begin();
        }
        private void liveTileAnim2_Part1_Completed_1(object sender, EventArgs e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnim2_Part2");
            anim.Begin();
        }

        private void liveTileAnim2_Part2_Completed_1(object sender, EventArgs e)
        {
            Storyboard anim = (Storyboard)FindName("liveTileAnim1_Part1");
            anim.Begin();
        }
    }
}
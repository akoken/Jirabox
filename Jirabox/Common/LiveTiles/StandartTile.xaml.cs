using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Jirabox.Common.LiveTiles
{
    public partial class StandartTile : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(StandartTile), new PropertyMetadata(string.Empty, OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageTileControl = d as StandartTile;
            imageTileControl.txt1.Text = e.NewValue.ToString();
        }


        public BitmapImage BackgroundImage
        {
            get { return (BitmapImage)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(BitmapImage), typeof(StandartTile), new PropertyMetadata(null));
        public StandartTile()
        {
            InitializeComponent();          
        }
    }
}

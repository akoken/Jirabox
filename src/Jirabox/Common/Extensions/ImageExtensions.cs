using System.IO;
using System.Windows.Media.Imaging;

namespace Jirabox.Common.Extensions
{
    public static class ImageExtensions
    {
        public static BitmapImage ToBitmapImage(this byte[] data)
        {
            var bitmapImage = new BitmapImage();
            if (data == null) return bitmapImage;
           
            using (var ms = new MemoryStream(data))
            {
                bitmapImage.SetSource(ms);
                return bitmapImage;
            }
        }
    }
}

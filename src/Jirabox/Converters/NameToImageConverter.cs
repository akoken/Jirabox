using GalaSoft.MvvmLight;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Jirabox.Converters
{
    public class NameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                var sync = new object();
                if (value == null) return null;

                var filename = value.ToString().Replace(":", ".").Replace(" ", "");
                var bi = new BitmapImage();

                lock (sync)
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        var path = Path.Combine("Images", filename + ".png");
                        if (!isf.FileExists(path)) return null;

                        using (var fs = isf.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            var data = new byte[fs.Length];
                            if (data.Length == 0) return null;
                            fs.Read(data, 0, data.Length);
                            using (var ms = new MemoryStream(data))
                            {
                                bi.SetSource(ms);
                            }
                        }
                    }
                }
                return bi;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

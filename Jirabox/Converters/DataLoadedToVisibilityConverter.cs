using System;
using System.Windows;
using System.Windows.Data;

namespace Jirabox.Converters
{
    public class DataLoadedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isDataLoaded = (bool)value;

            if (isDataLoaded)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;  
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

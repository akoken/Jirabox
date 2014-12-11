using System;
using System.Windows.Data;

namespace Jirabox.Converters
{
    public class UserStatusToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isActive = (bool)value;

            if (isActive)
                return "Active";
            return "Inactive";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

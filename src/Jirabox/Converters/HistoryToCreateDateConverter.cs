using Jirabox.Model;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace Jirabox.Converters
{
    public class HistoryToCreateDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var histories = value as List<History>;

            if (histories != null && histories.Count > 0)
                return histories[0].CreateDate;
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

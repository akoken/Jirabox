using System;
using System.Windows.Data;

namespace Jirabox.Converters
{
    public class CheckIssueSummaryLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var summary = value.ToString();
            if (summary.Length > 100)
                return string.Format("{0}...", summary.Remove(98));
            return summary;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jirabox.Common.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var collection = new ObservableCollection<T>();
            foreach (T item in source)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}

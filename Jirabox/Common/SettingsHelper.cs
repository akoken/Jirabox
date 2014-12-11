using System.IO.IsolatedStorage;

namespace Jirabox.Common
{
    /// <summary>
    /// Helper class is needed because IsolatedStorageProperty is generic and 
    /// can not provide singleton model for static content
    /// </summary>
    internal static class SettingsHelper
    {      
        public static readonly object ThreadLocker = new object();
        public static readonly IsolatedStorageSettings Store = IsolatedStorageSettings.ApplicationSettings;
    }

    /// <summary>
    /// This is wrapper class for storing one setting
    /// Object of this type must be single
    /// </summary>
    /// <typeparam name="T">Any serializable type</typeparam>
    public class IsolatedStorageProperty<T>
    {
        private readonly object defaultValue;
        private readonly string name;
        private readonly object syncObject = new object();

        public IsolatedStorageProperty(string name, T defaultValue = default(T))
        {
            this.name = name;
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Determines if setting exists in the storage
        /// </summary>
        public bool Exists
        {
            get { return SettingsHelper.Store.Contains(name); }
        }

        /// <summary>
        /// Use this property to access the actual setting value
        /// </summary>
        public T Value
        {
            get
            {
                //If property does not exist - initializing it using default value
                if (!Exists)
                {
                    //Initializing only once
                    lock (syncObject)
                    {
                        if (!Exists) SetDefault();
                    }
                }

                return (T)SettingsHelper.Store[name];
            }
            set
            {
                SettingsHelper.Store[name] = value;
                Save();
            }
        }

        private static void Save()
        {
            lock (SettingsHelper.ThreadLocker)
            {
                SettingsHelper.Store.Save();
            }
        }

        public void SetDefault()
        {
            Value = (T)defaultValue;
        }
    }
}

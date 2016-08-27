using Jirabox.Core.Contracts;
using System.IO;
using System.IO.IsolatedStorage;

namespace Jirabox.Services
{
    public class CacheService : ICacheService
    {
        private const string CacheFolderName = "Cache";

        public void Save(string fileName, string content)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(CacheFolderName))
                    store.CreateDirectory(CacheFolderName);

                using (var stream = store.OpenFile(Path.Combine(CacheFolderName, fileName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(content);
                    }
                }
            }
        }

        public string Read(string fileName)
        {
            var path = Path.Combine(CacheFolderName, fileName);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(path))
                {
                    using (var stream = new IsolatedStorageFileStream(path, FileMode.Open, store))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            var content = reader.ReadString();
                            return content;                  
                        }
                    }
                }
                throw new IOException("Could not find requested cached file.");
            }            
        }

        public bool DoesFileExist(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return store.FileExists(Path.Combine(CacheFolderName, fileName));             
            }     
        }

        public void ClearCache()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(CacheFolderName))
                    return;
                               
                foreach (var file in store.GetFileNames(Path.Combine(CacheFolderName, "*.*")))
                {
                    store.DeleteFile(Path.Combine(CacheFolderName, file));
                }
                
                store.DeleteDirectory(CacheFolderName);
            }
        }
    }
}

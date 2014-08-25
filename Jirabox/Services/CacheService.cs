using Jirabox.Core.Contracts;
using System.IO;
using System.IO.IsolatedStorage;

namespace Jirabox.Services
{
    public class CacheService : ICacheService
    {
        private const string cacheFolderName = "Cache";

        public void Save(string fileName, string content)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(cacheFolderName))
                    store.CreateDirectory(cacheFolderName);

                using (var stream = store.OpenFile(Path.Combine(cacheFolderName, fileName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
            var path = Path.Combine(cacheFolderName, fileName);
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
                return store.FileExists(Path.Combine(cacheFolderName, fileName));             
            }     
        }

        public void ClearCache()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(cacheFolderName))
                    return;

                // Get the subfolders that reside under path
                var folders = store.GetDirectoryNames(Path.Combine(cacheFolderName, "*.*"));
               
                // Delete all files at the root level in that folder.
                foreach (var file in store.GetFileNames(Path.Combine(cacheFolderName, "*.*")))
                {
                    store.DeleteFile(Path.Combine(cacheFolderName, file));
                }

                // Finally delete the path
                store.DeleteDirectory(cacheFolderName);
            }
        }
    }
}

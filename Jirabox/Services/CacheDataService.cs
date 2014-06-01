using Jirabox.Core.Contracts;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Jirabox.Services
{
    public class CacheDataService : ICacheDataService
    {
        private const string cacheFolderName = "CacheFolder";
        public async void Save(string fileName, string content)
        {
            StorageFolder cacheFolder;
            var folder = ApplicationData.Current.LocalFolder;            
            byte[] data = Encoding.UTF8.GetBytes(content);            

            if (!Directory.Exists(string.Format(@"{0}\{1}", folder.Path, cacheFolderName)))
                cacheFolder = await folder.CreateFolderAsync(cacheFolderName);
            
            cacheFolder = await folder.GetFolderAsync(cacheFolderName);
            var file = await cacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream s = await file.OpenStreamForWriteAsync())
            {
                await s.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task<string> Get(string fileName)
        {
            byte[] data;

            var folder = ApplicationData.Current.LocalFolder;
            var cacheFolder = await folder.GetFolderAsync(cacheFolderName);

            var file = await cacheFolder.GetFileAsync(fileName);

            using (Stream s = await file.OpenStreamForReadAsync())
            {
                data = new byte[s.Length];
                await s.ReadAsync(data, 0, (int)s.Length);
            }

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public async Task<bool> DoesFileExist(string fileName)
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (Directory.Exists(string.Format(@"{0}\{1}", folder.Path, cacheFolderName)))
            {
                var cacheFolder = await folder.GetFolderAsync(cacheFolderName);
                var files = await cacheFolder.GetFilesAsync();
                foreach (var file in files)
                {
                    if (file.Name == fileName)
                        return true;
                }                
            }
            return false;            
        } 

        public async void ClearCacheData()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (Directory.Exists(string.Format(@"{0}\{1}", folder.Path, cacheFolderName)))
            {
                var cacheFolder = await folder.GetFolderAsync(cacheFolderName);
                var files = await cacheFolder.GetFilesAsync();

                foreach (var file in files)
                {
                    await file.DeleteAsync();
                }
            }
        }
    }
}

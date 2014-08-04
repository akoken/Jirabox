using BugSense;
using BugSense.Core.Model;
using Jirabox.Model;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage;

namespace Jirabox.Common
{
    public class StorageHelper
    {
        private const string path = "credential.ak";
        private static object sync = new object();
        public static void SaveUserCredential(string serverUrl, string username, string password)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {                
                if (isf.FileExists(path))                
                    isf.DeleteFile(path);     
           
                using (var sw = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, isf)))
                {
                    sw.WriteLine(serverUrl);
                    sw.WriteLine(username);
                    sw.WriteLine(password);
                }
            }
        }

        public static UserCredential GetUserCredential()
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {                
                if (isf.FileExists(path))
                {
                    var fs = isf.OpenFile(path, FileMode.Open, FileAccess.Read);
                    using (var sr = new StreamReader(fs))
                    {
                        var data = sr.ReadToEnd();
                        var credentialArray = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        var userCredential = new UserCredential
                        {
                            ServerUrl = credentialArray[0],
                            UserName = credentialArray[1],
                            Password = credentialArray[2]
                        };
                        return userCredential;                       
                    }
                }
                return null;
            }
        }

        public async static Task<bool> DoesFileExistAsync(string fileName, StorageFolder folder)
        {
            try
            {
                await folder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClearCache()
        {
            using (var local = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var pattern = @"Images\*";
                var files = local.GetFileNames(pattern);

                foreach (var file in files)
                {
                    try
                    {
                        lock (sync)
                        {
                            local.DeleteFile(Path.Combine("Images", file));
                        }
                    }
                    catch (IsolatedStorageException storageException)
                    {
                        var extras = BugSenseHandler.Instance.CrashExtraData;
                        extras.Add(new CrashExtraData
                        {
                            Key = "Method",
                            Value = "StorageHelper.ClearCache"
                        });

                        BugSenseHandler.Instance.LogException(storageException, extras);
                    }
                }         
            }
        }

        public static void ClearUserCredential()
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(path))
                {
                    isf.DeleteFile(path);
                }
            }
        }

        public static BitmapImage GetDisplayPicture(string fileName)
        {
            var sync = new object();
            if (fileName == null) return null;

            var filename = fileName.ToString().Replace(":", ".").Replace(" ", "");
            byte[] data;
            var displayPicture = new BitmapImage();

            lock (sync)
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var path = Path.Combine("Images", filename + ".png");
                    if (!isf.FileExists(path)) return null;

                    using (var fs = isf.OpenFile(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        data = new byte[fs.Length];
                        if (data.Length == 0) return null;
                        fs.Read(data, 0, data.Length);
                        using (var ms = new MemoryStream(data))
                        {
                            displayPicture.SetSource(ms);
                        }
                    }
                }
            }
            return displayPicture;
        }
    }
}

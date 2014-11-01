using BugSense;
using BugSense.Core.Model;
using Jirabox.Model;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage;

namespace Jirabox.Common
{
    public class StorageHelper
    {
        private const string path = "jirabox.user";
        private const string encryptedPath = "jirabox.pass";
        private static object sync = new object();

        public static void SaveUserCredential(string serverUrl, string username, string password)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(path))
                    isf.DeleteFile(path);

                using (var sw = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, isf)))
                {
                    byte[] pPassword = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), null);

                    sw.WriteLine(serverUrl);
                    sw.WriteLine(username);
                    WriteEncryptedPasswordToFile(pPassword);
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
                        var password = ProtectedData.Unprotect(ReadEncryptedPasswordFromFile(), null);

                        var userCredential = new UserCredential
                        {
                            ServerUrl = credentialArray[0],
                            UserName = credentialArray[1],
                            Password = Encoding.UTF8.GetString(password, 0, password.Length)
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

        public static void ClearImageCache()
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
                            Value = "StorageHelper.ClearImageCache"
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

        public static async Task ClearAttachmentCache()
        {
            var local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var dataFolder = await local.GetFolderAsync("Attachments");
            await dataFolder.DeleteAsync();
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

        private static void WriteEncryptedPasswordToFile(byte[] passwordData)
        {
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var writestream = new IsolatedStorageFileStream(encryptedPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, file))
                {
                    using (var writer = new StreamWriter(writestream).BaseStream)
                    {
                        writer.Write(passwordData, 0, passwordData.Length);
                    }
                }
            }
        }

        private static byte[] ReadEncryptedPasswordFromFile()
        {
            byte[] passwordArray = null;
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var readstream = new IsolatedStorageFileStream(encryptedPath, System.IO.FileMode.Open, FileAccess.Read, file))
                {
                    using (var reader = new StreamReader(readstream).BaseStream)
                    {
                        passwordArray = new byte[reader.Length];
                        reader.Read(passwordArray, 0, passwordArray.Length);
                    }
                }
            }
            return passwordArray;
        }

        public static void DeleteOldCredentialFile()
        {
            string filePath = "credential.ak";
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filePath))
                    isf.DeleteFile(filePath);
            }
        }

        public static async Task WriteDataToIsolatedStorageFile(string fileName, byte[] data)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            
            var dataFolder = await local.CreateFolderAsync("Attachments", CreationCollisionOption.OpenIfExists);
            
            var file = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(data, 0, data.Length);
            }
        }
    }
}

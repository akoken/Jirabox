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
        private const string Path = "jirabox.user";
        private const string EncryptedPath = "jirabox.pass";
        private static readonly object Sync = new object();

        public static void SaveUserCredential(string serverUrl, string username, string password)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(Path))
                    isf.DeleteFile(Path);

                using (var sw = new StreamWriter(new IsolatedStorageFileStream(Path, FileMode.Create, FileAccess.Write, isf)))
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
                if (isf.FileExists(Path))
                {
                    var fs = isf.OpenFile(Path, FileMode.Open, FileAccess.Read);
                    using (var sr = new StreamReader(fs))
                    {
                        var data = sr.ReadToEnd();
                        var credentialArray = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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
                if (!local.DirectoryExists("Images")) return;

                const string pattern = @"Images\*";
                var files = local.GetFileNames(pattern);

                foreach (var file in files)
                {
                    try
                    {
                        lock (Sync)
                        {
                            local.DeleteFile(System.IO.Path.Combine("Images", file));
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
                if (isf.FileExists(Path))
                {
                    isf.DeleteFile(Path);
                }
            }
        }

        public static async Task ClearAttachmentCache()
        {
            var local = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var folders = await local.GetFoldersAsync();
            
            foreach (StorageFolder folder in folders)
            {
                if(folder.Name == "Attachments")
                {
                    await folder.DeleteAsync();
                }
            }            
        }

        public static BitmapImage GetDisplayPicture(string fileName)
        {           
            if (fileName == null) return null;

            var filename = fileName.Replace(":", ".").Replace(" ", "");
            var displayPicture = new BitmapImage();

            lock (Sync)
            {
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var path = System.IO.Path.Combine("Images", filename + ".png");
                    if (!isf.FileExists(path)) return null;

                    using (var fs = isf.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var data = new byte[fs.Length];
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
                using (var writestream = new IsolatedStorageFileStream(EncryptedPath, FileMode.Create, FileAccess.Write, file))
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
            byte[] passwordArray;
            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var readstream = new IsolatedStorageFileStream(EncryptedPath, FileMode.Open, FileAccess.Read, file))
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
            const string filePath = "credential.ak";
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filePath))
                    isf.DeleteFile(filePath);
            }
        }

        public static async Task WriteDataToIsolatedStorageFile(string fileName, byte[] data)
        {
            StorageFolder local = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var dataFolder = await local.CreateFolderAsync("Attachments", CreationCollisionOption.OpenIfExists);

            var file = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(data, 0, data.Length);
            }
        }
    }
}

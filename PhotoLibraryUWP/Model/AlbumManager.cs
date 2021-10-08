using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;

namespace PhotoLibraryUWP.Model
{
    class AlbumManager
    {
        private static string localPath = ApplicationData.Current.LocalFolder.Path;
        public static void addPhotosToAlbum(List<Photo> photos, Album album)
        {
            album.ListofPhotos.AddRange(photos);
            album.CoverPhoto = album.ListofPhotos.FirstOrDefault();
        }

        //public static void deletePhotosFromAlbum(Album album, List<Photo> deleteThesePhotos)
        //{
        //    album.ListOfPhotos.Remove(deleteThesePhotos);
        //}

        public static async void saveToFile(Album album, string userName)
        {
            StorageFolder userFolder = await StorageFolder.GetFolderFromPathAsync(localPath + $"\\{userName}");
            await userFolder.CreateFileAsync($"{album.Name}.json", CreationCollisionOption.ReplaceExisting);
            StorageFile albumFile = await userFolder.GetFileAsync($"{album.Name}.json");
            string jsonString = JsonConvert.SerializeObject(album);
            await FileIO.WriteTextAsync(albumFile, jsonString);
        }

        public static async void readUserAlbum(string userName, ObservableCollection<Album> albumList)
        {
            albumList.Clear();
            DirectoryInfo d = new DirectoryInfo(localPath + $"\\{userName}");
            if (!d.Exists)
            {
                d.Create();
                Debug.WriteLine("Created file at " + localPath);
            }
            else
            {
                Debug.WriteLine("Not created" + localPath);
            }

            StorageFolder userFolder = await StorageFolder.GetFolderFromPathAsync(localPath + $"\\{userName}");
            IReadOnlyList<StorageFile> userAlbumList = await userFolder.GetFilesAsync();

            foreach (StorageFile album in userAlbumList)
            {
                string albumDetails = await FileIO.ReadTextAsync(album);
                Album userAlbum = JsonConvert.DeserializeObject<Album>(albumDetails);
                albumList.Add(userAlbum);
            }
        }

        public static async void deleteUserAlbum(Album albumName, string userName)
        {
            var localPath = ApplicationData.Current.LocalFolder.Path;
            StorageFolder userFolder = await StorageFolder.GetFolderFromPathAsync(localPath + $"\\{userName}");
            StorageFile albumFileToDelete = await userFolder.GetFileAsync($"{albumName.Name}.json");
            await albumFileToDelete.DeleteAsync();
        }

        public static void displayUserPhotosByAlbum(Album album, ObservableCollection<Photo> photoList)
        {
            var allPhotos = album.ListofPhotos;
            photoList.Clear();
            allPhotos.ForEach(photo => photoList.Add(photo));
        }
        
    }
}

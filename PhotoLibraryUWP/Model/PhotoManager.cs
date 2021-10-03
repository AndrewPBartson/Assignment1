using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    public class PhotoManager
    {
        public static void GetAllPhotos(ObservableCollection<Photo> photos)
        {
            var allPhotos = getPhotos();
            photos.Clear();
            /*foreach (var sound in allSounds)
            {
                sounds.Add(sound);
            }*/

            allPhotos.ForEach(photo => photos.Add(photo));
        }

        public static void getPhotosByCategory(ObservableCollection<Photo> photos, PhotoCategory category)
        {
            var allPhotos = getPhotos();
            var filteredPhotos = allPhotos.Where(p => p.Category == category).ToList();
            photos.Clear();
            filteredPhotos.ForEach(photo => photos.Add(photo));
        }
        private static List<Photo> getPhotos()
        {
            var photos = new List<Photo>();
            photos.Add(new Photo("Art", PhotoCategory.Art));
            photos.Add(new Photo("Watermelon", PhotoCategory.Art));

            photos.Add(new Photo("Balloons", PhotoCategory.Celebration));
            photos.Add(new Photo("Holiday", PhotoCategory.Celebration));

            photos.Add(new Photo("Deer", PhotoCategory.Nature));
            photos.Add(new Photo("Sunset", PhotoCategory.Nature));

            return photos;

        }
    }
}

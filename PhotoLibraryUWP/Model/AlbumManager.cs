using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PhotoLibraryUWP.Model
{
    class AlbumManager
    {
        public static void GetMyAlbums(ObservableCollection<Album> Albums)
        {
            ManageDataFile MyAlbum = new ManageDataFile();
            var allPhotos = MyAlbum.GetMyAlbums();
            Albums.Clear();
            allPhotos.ForEach(album => Albums.Add(album));
        }

    }
}

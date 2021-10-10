using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace PhotoLibraryUWP.Model
{


    class ManageDataFile
    {

       
        public List<Photo> GetMyPhotos()
        {
            //Retuen all Photos on All Albums which User Has already Saved

            UserDataFile ReadingFile = new UserDataFile();
            List<PhotoAlbumInformation> MyALbumsInfo = ReadingFile.FetchingPhotoAlbumInformation(UserManagement.CurrentAppUser);
            List<Photo> Photos = new List<Photo>();
           
            Photos.AddRange(MyALbumsInfo.Where(x => x.UserInfo.Name == UserManagement.CurrentAppUser.Name).Select(x => new Photo()
            { Name = x.PhotoName, PhotoPath = x.PhotoPath }));
            return Photos;
        }

        public List<Album> GetMyAlbums()
        { //Retuen all Albums which User Has already Saved
          
            UserDataFile ReadingFile = new UserDataFile();
            List<PhotoAlbumInformation> MyALbumsInfo = ReadingFile.FetchingPhotoAlbumInformation(UserManagement.CurrentAppUser);
            List<Album> Albums = new List<Album>();
            foreach (var listItem in MyALbumsInfo.Where(x=>x.UserInfo.Name == UserManagement.CurrentAppUser.Name))
            {
                var existingAlbum = Albums.Where(x => x.Name == listItem.Albumname && x.Description == listItem.AlbumDescription).FirstOrDefault();
                if (existingAlbum == null)
                {
                    existingAlbum = new Album();
                    existingAlbum.Name = listItem.Albumname;
                    existingAlbum.Description = listItem.AlbumDescription;
                    existingAlbum.ListofPhotos = new List<Photo>();
                    Albums.Add(existingAlbum);
                }

                var photoToBeAdded = new Photo() { Name = listItem.PhotoName, PhotoPath = listItem.PhotoPath, Category = listItem.Category };
                existingAlbum.ListofPhotos.Add(photoToBeAdded);

                if (listItem.IsCoverphoto)
                {
                    existingAlbum.CoverPhoto = photoToBeAdded;
                }

                
            }

            return Albums;
        }


        public bool DeletePhotoAlbum(User CurrentUser, Album CurrentAlbum)
        {

            UserDataFile Delete = new UserDataFile();
                       
            Delete.DeleteAlbum( CurrentUser,  CurrentAlbum);

         
            return true;

        }

        public bool RemovePhotoFromAlbum(User CurrentUser, Album CurrentAlbum , List<Photo> Currentphotos )
        {

            UserDataFile Delete = new UserDataFile();

            Delete.DeletePhotoFormAlbum(CurrentUser, CurrentAlbum , Currentphotos);


            return true;

        }

        public bool ChangeCoverPhoto(User CurrentUser, Album CurrentAlbum, Photo Currentphoto)
        {

            UserDataFile CoverPhoto = new UserDataFile();

            CoverPhoto.ChangeCoverPhoto(CurrentUser, CurrentAlbum, Currentphoto);


            return true;

        }

        

    }
}

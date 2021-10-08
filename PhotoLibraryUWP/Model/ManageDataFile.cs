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

        //public List<PhotoAlbumInformation> Loaddata()
        //{
        //    UserDataFile Datafile = new UserDataFile();
        //    List<PhotoAlbumInformation> MyALbumsInfo = Datafile.FetchingPhotoAlbumInformation(UserManagement.CurrentAppUser);

        //    return (MyALbumsInfo);


        //}

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
            
            
            List<PhotoAlbumInformation> MyALbumsInfo1;
            MyALbumsInfo1 = MyALbumsInfo;
           
            // MyALbumsInfo.Where(m => m.UserInfo.Name == UserManagement.CurrentAppUser.Name &&  m.IsCoverphoto ).GroupBy(n => new { n.Albumname, n.AlbumDescription });
           var ALbumCount=  MyALbumsInfo.Where(m => m.UserInfo.Name == UserManagement.CurrentAppUser.Name && m.IsCoverphoto).ToList();
          
            //PhotoAlbumInformation PhotoCount  ;

            foreach (var listItem in ALbumCount)
            {
                Album album = new Album();
                album.Name = listItem.Albumname;
                album.Description = listItem.AlbumDescription;
                List<Photo> Photos = new List<Photo>();

               var  PhotoCount = MyALbumsInfo1.Where(m => m.UserInfo.Name == UserManagement.CurrentAppUser.Name &&
                                                m.Albumname == listItem.Albumname &&
                                                 m.AlbumDescription == listItem.AlbumDescription).ToList();

                foreach (var photoitem in PhotoCount)
                {
                    Photo Nphoto = new Photo();
                    Nphoto.Name = photoitem.PhotoName;
                    Nphoto.PhotoPath = photoitem.PhotoPath;
                    Nphoto.Category = photoitem.Category;
                    Photos.Add(Nphoto);
                    album.ListofPhotos = Photos;
                    if (photoitem.IsCoverphoto) album.CoverPhoto = Nphoto;
                }
              

                Albums.Add(album);
            }




            return Albums;
        }

        public void GetMyAlbums(ObservableCollection<Photo> photoList, User CurrentAppUser)
        {
            UserDataFile ReadingFile = new UserDataFile();
            List<PhotoAlbumInformation> MyALbumsInfo = ReadingFile.FetchingPhotoAlbumInformation(UserManagement.CurrentAppUser);
            List<Album> albums = new List<Album>();

            //  albums.AddRange(MyALbumsInfo.Select(x => new Album() { Name = x.NewAlbum, Description = x. }));
             

            //var Selected CoverPhoto = MyALbumsInfo.FirstOrDefault(x => IsCoverphoto);
            //Coverphoto.Name = SelectedCoverPhoto.PhotoName;
            //Coverphoto.PhotoPath = SelectedCoverPhoto.PhotoName;
            //Coverphoto.Category = SelectedCoverPhoto.Category;
        }

        public bool DeletePhotoAlbum(User CurrentUser, Album CurrentAlbum)
        {

            UserDataFile Delete = new UserDataFile();
                       
            Delete.DeleteAlbum( CurrentUser,  CurrentAlbum);

         
            return true;

        }

        public bool RemovePhotoFromAlbum(User CurrentUser, Album CurrentAlbum , Photo Currentphoto )
        {

            UserDataFile Delete = new UserDataFile();

            Delete.DeletePhotoFormAlbum(CurrentUser, CurrentAlbum , Currentphoto);


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

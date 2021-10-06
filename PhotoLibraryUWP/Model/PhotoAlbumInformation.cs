using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PhotoLibraryUWP.Model
{
    class PhotoAlbumInformation
    {
        

        public User UserInfo { get; set; }
        public Album NewAlbum { get; set; }
        public List<Photo> Photos { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoName { get; set; }
        public Boolean IsCoverphoto { get; set; }
        public String Category { get; set; }
        public Photo Coverphoto { get; set;}


        public PhotoAlbumInformation() {
            UserInfo = new User();
            NewAlbum = new Album();
            
        }
        public PhotoAlbumInformation(User UserInfo, Album NewAlbum, List<Photo>  Photos , Photo Coverphoto)
        {
            //UserInfo = new User();
            //NewAlbum = new Album();

        }
        public List<PhotoAlbumInformation> FetchingPhotoAlbumInformation()
        {
            string Filepath = $".\\Files\\PhotoAlbumInformation.csv";

            List<PhotoAlbumInformation> PhotoAlbumList = File.ReadAllLines(Filepath)
               .Skip(1)
               .Select(user => FromCsv(user))
               .ToList();
            return (PhotoAlbumList);

        }

        public PhotoAlbumInformation FromCsv(string csvLine)
        {
            
           
          
            string[] values = csvLine.Split(',');
            //PhotoAlbumInformation PhotoAlbumList = new PhotoAlbumInformation();
            this.UserInfo.Name = Convert.ToString(values[0]).Trim();
            this.NewAlbum.Name = Convert.ToString(values[1]).Trim();
            this.NewAlbum.Description = Convert.ToString(values[2]).Trim();
            this.PhotoName= Convert.ToString(values[3]).Trim();
            this.PhotoPath = Convert.ToString(values[4]).Trim();
            this.Category = Convert.ToString(values[5]).Trim();
            this.IsCoverphoto = Convert.ToBoolean(values[6]);
            return this;
        }

        ////public void IntoCsv(String UserName, String Albumname, String AlbumDescription, List<Photo> photopath, bool Iscoverphoto)

        //public void IntoCsv(User Userinfo, Album NewAlbum, List<Photo> photopath, Photo coverphoto)
        //{
        //    //String NewuserAlbuminfo;
        //    //List<String> NewAlbumInfo ;

        //    //NewAlbumInfo = null;
        //    //foreach (Photo selectedphoto in photopath)
        //    //{
        //    //    NewuserAlbuminfo = Username + ","+ AlbumName+','+ AlbumDescription+ ','+ Photopath+','+ IsCoverphoto.ToString();
        //    //    NewAlbumInfo.Add(NewuserAlbuminfo);
        //    //}

        //}

       // public void SaveInPhotoAlbum(PhotoAlbumInformation )
     //  public void SavnPhotoAlbum(String UserName, String Albumname, String AlbumDescription, List<Photo> photopath, bool Iscoverphoto)
             public void SavingPhotoAlbum(User UserInfo, Album NewAlbum, List<Photo>  Photos , Photo Coverphoto)
        {

            //String NewuserAlbuminfo;
            //List<String> NewAlbumInfo;

        //NewAlbumInfo = null;
        string Filepath = $".\\Files\\PhotoAlbumInformation.csv";
            foreach (Photo selectedphoto in Photos)
            {
                string   NewuserAlbuminfo = string.Join( ',', UserInfo.Name + "," + NewAlbum.Name + ',' +
                    NewAlbum.Description + ',' + selectedphoto.PhotoPath + ','+ selectedphoto.Name +','+ selectedphoto.Category + true);
                
                IEnumerable<string> result = NewuserAlbuminfo.Cast<String>();
                File.AppendAllLines(Filepath, result);
                 
            }

                             
        }
        public List<Photo> GetMyPhotos()
        {
            List<PhotoAlbumInformation> MyALbumsInfo = FetchingPhotoAlbumInformation();
            List<Photo> Photos = new List<Photo>();
            //Category= Enum.Parse((int)typeof(PhotoCategory), x.Category,true) 
            Photos.AddRange(MyALbumsInfo.Select(x => new Photo () { Name = x.PhotoName, PhotoPath = x.PhotoPath  }));

            return (Photos);
                    }

        public void GetMyAlbums(ObservableCollection<Photo> photoList)
        {
            List<PhotoAlbumInformation> MyALbumsInfo = FetchingPhotoAlbumInformation();
            List<Album> albums=new List<Album>();
             
          //  albums.AddRange(MyALbumsInfo.Select(x => new Album() { Name = x.NewAlbum, Description = x. }));


            //var SelectedCoverPhoto = MyALbumsInfo.FirstOrDefault(x => IsCoverphoto);
            //Coverphoto.Name = SelectedCoverPhoto.PhotoName;
            //Coverphoto.PhotoPath = SelectedCoverPhoto.PhotoName;
            //Coverphoto.Category = SelectedCoverPhoto.Category;


        }





    }


}

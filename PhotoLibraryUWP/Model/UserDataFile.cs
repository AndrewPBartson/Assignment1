using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Windows.UI.Popups;
using Windows.Storage;

namespace PhotoLibraryUWP.Model
{
    class UserDataFile
    {


        private String Filepath;

        public void CreateFile()
        {
            
            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            if (!d.Exists) d.Create();

            Filepath = $"{d}\\PhotoAlbumInformation.csv";

            FileInfo F = new FileInfo(Filepath);
           
            if (!F.Exists)

            {

                F.Create();
               
            }
            
      }

        public List<PhotoAlbumInformation> FetchingPhotoAlbumInformation(User AppUser)
        {
            CreateFile();

            List<PhotoAlbumInformation> PhotoAlbumList;

            PhotoAlbumList = File.ReadAllLines(Filepath)
               .Select(user => FromCsv(user))
               .ToList();
           
            return (PhotoAlbumList);
        }

        public PhotoAlbumInformation FromCsv(string csvLine)
        {

            PhotoCategory myValueAsEnum;


            string[] values = csvLine.Split(',');
            PhotoAlbumInformation PhotoAlbumInformationList = new PhotoAlbumInformation();
            PhotoAlbumInformationList.UserInfo.Name = Convert.ToString(values[0]).Trim();
            PhotoAlbumInformationList.Albumname = Convert.ToString(values[1]).Trim();
            PhotoAlbumInformationList.AlbumDescription = Convert.ToString(values[2]).Trim();
            PhotoAlbumInformationList.PhotoName = Convert.ToString(values[3]).Trim();
            myValueAsEnum = (PhotoCategory)(Convert.ToInt32(values[4]));
            PhotoAlbumInformationList.Category = myValueAsEnum;
            PhotoAlbumInformationList.PhotoPath = $"/Assets/Photos/{myValueAsEnum}/{Convert.ToString(values[3]).Trim()}.png";

            PhotoAlbumInformationList.IsCoverphoto = Convert.ToBoolean(values[5]);

            return PhotoAlbumInformationList;
        }

        public void SavingPhotoAlbum(User UserInfo, Album NewAlbum, List<Photo> Photos, Photo Coverphoto)
        {

          

            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

            //if Album Exists then just add photo
            if (PhotoAlbumList.Any(m => m.UserInfo.Name == UserInfo.Name &&
                                               m.Albumname == NewAlbum.Name &&
                                                m.AlbumDescription == NewAlbum.Description))

            {
                AddingPhotoToCurrentAlbum(UserInfo, NewAlbum, Photos);
                return ;
            }

            Boolean iscoverphoto;

            foreach (Photo selectedphoto in Photos)
            {
                iscoverphoto = false ;
                if (selectedphoto.Name == Coverphoto.Name) iscoverphoto = true;

                string NewuserAlbuminfo = string.Join(',', UserInfo.Name + "," + NewAlbum.Name + ',' +
                    NewAlbum.Description + ',' + selectedphoto.Name + ',' + ((int)selectedphoto.Category) + ','  + iscoverphoto);

                File.AppendAllText(Filepath, NewuserAlbuminfo + Environment.NewLine);

               
            }
        

        }


        public  void AddingPhotoToCurrentAlbum(User UserInfo, Album NewAlbum, List<Photo> Photos )
        {
            
            foreach (Photo selectedphoto in Photos)
            {
                string NewuserAlbuminfo = string.Join(',', UserInfo.Name + "," + NewAlbum.Name + ',' +
                NewAlbum.Description + ',' + selectedphoto.Name + ',' + ((int)selectedphoto.Category) + ',' + false);

                File.AppendAllText(Filepath, NewuserAlbuminfo + Environment.NewLine);
            }


        }

    
        public bool DeleteAlbum(User UserInfo, Album NewAlbum)
        {

           
            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

            var Validdata = PhotoAlbumList.Where( m => !(m.UserInfo.Name == UserInfo.Name &&
                                               m.Albumname == NewAlbum.Name &&
                                                m.AlbumDescription == NewAlbum.Description)).ToList();
            File.Delete(Filepath);

           List<string> NewuserAlbuminfo=new List<string>();

            foreach (var validdata in Validdata)
            {
                NewuserAlbuminfo.Add(string.Join(',', validdata.UserInfo.Name + "," + validdata.Albumname + ',' +
                                      validdata.AlbumDescription + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));

            
                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }
           
           return true;
        }

        public bool DeletePhotoFormAlbum(User UserInfo, Album NewAlbum ,List<Photo> SelectedPhoto)
        {
            
            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);
            
            var CrrentAlbumInfo = PhotoAlbumList.Where(m => (m.UserInfo.Name == UserInfo.Name &&
                                            m.Albumname == NewAlbum.Name &&
                                             m.AlbumDescription == NewAlbum.Description)).ToList();

            var Validdata = PhotoAlbumList.Where(m => !(m.UserInfo.Name == UserInfo.Name &&
                                              m.Albumname == NewAlbum.Name &&
                                               m.AlbumDescription == NewAlbum.Description )).ToList();
            File.Delete(Filepath);

            List<string> NewuserAlbuminfo = new List<string>();

            foreach (var validdata in Validdata)
            {
                NewuserAlbuminfo.Add(string.Join(',', validdata.UserInfo.Name + "," + validdata.Albumname + ',' +
                                      validdata.AlbumDescription + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));


                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }

             var filtered = CrrentAlbumInfo
                   .Where(x => ! SelectedPhoto.Any(y => (y.Name == x.PhotoName ))).ToList();

            if (filtered.Count() < 1) return true; 
            var hascoverphoto = filtered.First(x=>x.IsCoverphoto=true);

            if ( hascoverphoto != null )  filtered.First(x => x.IsCoverphoto = true);

            foreach (var photo in filtered)
            {
              
                NewuserAlbuminfo.Add(string.Join(',', photo.UserInfo.Name + "," + photo.Albumname + ',' +
                                   photo.AlbumDescription + ',' + photo.PhotoName + ',' + ((int)photo.Category) + ',' + photo.IsCoverphoto));
                File.WriteAllLines(Filepath, NewuserAlbuminfo);

            }
          return true;
        }


        public bool ChangeCoverPhoto(User UserInfo, Album NewAlbum, Photo SelectedPhoto)
        {

            

            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

          

            var OldAlbum = PhotoAlbumList.Where(m => m.UserInfo.Name == UserInfo.Name &&
                                m.Albumname == NewAlbum.Name &&
                                 m.AlbumDescription == NewAlbum.Description ).ToList();

            var Validdata = PhotoAlbumList.Where(m => !(m.UserInfo.Name == UserInfo.Name &&
                                              m.Albumname == NewAlbum.Name &&
                                               m.AlbumDescription == NewAlbum.Description )).ToList();


            File.Delete(Filepath);

            List<string> NewuserAlbuminfo = new List<string>();

            foreach (var validdata in Validdata)
            {

                NewuserAlbuminfo.Add(string.Join(',', validdata.UserInfo.Name + "," + validdata.Albumname + ',' +
                                      validdata.AlbumDescription + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));


                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }

            Boolean iscoverphoto ;
            foreach (var oldphoto in OldAlbum)
            {
                if (oldphoto.PhotoName == SelectedPhoto.Name && oldphoto.Category == SelectedPhoto.Category)  iscoverphoto = true;  else  iscoverphoto = false;   
                

                string Oldcoverohoto = string.Join(',', oldphoto.UserInfo.Name + "," + oldphoto.Albumname + ',' +
                oldphoto.AlbumDescription + ',' + oldphoto.PhotoName + ',' + ((int)oldphoto.Category) + ',' + iscoverphoto);

                File.AppendAllText(Filepath, Oldcoverohoto + Environment.NewLine);
            }
            

            return true;
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace PhotoLibraryUWP.Model
{
    class UserDataFile
    {
        
        
        
        
        public List<PhotoAlbumInformation> FetchingPhotoAlbumInformation(User AppUser)
        {
             
            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";

            FileInfo F = new FileInfo(Filepath);
             if (!F.Exists)
            {
                F.Create();

            }
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
            //PhotoAlbumInformationList.PhotoPath = Convert.ToString(values[4]).Trim();
            myValueAsEnum = (PhotoCategory)(Convert.ToInt32(values[4]));
            PhotoAlbumInformationList.Category = myValueAsEnum;
            PhotoAlbumInformationList.PhotoPath = $"/Assets/Photos/{myValueAsEnum}/{Convert.ToString(values[3]).Trim()}.png";

            PhotoAlbumInformationList.IsCoverphoto = Convert.ToBoolean(values[5]);

            return PhotoAlbumInformationList;
        }

        public void SavingPhotoAlbum(User UserInfo, Album NewAlbum, List<Photo> Photos, Photo Coverphoto)
        {

            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";

            FileInfo F = new FileInfo(Filepath);
            if (!F.Exists)
            {
                F.Create();

            }

            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

            //if Album Exists then just add photo
            if (PhotoAlbumList.Any(m => m.UserInfo.Name == UserInfo.Name &&
                                               m.Albumname == NewAlbum.Name &&
                                                m.AlbumDescription == NewAlbum.Description))

            {
                AddingPhotoToCurrentAlbum(UserInfo, NewAlbum, Photos);
                return;
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
            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";
                       

            foreach (Photo selectedphoto in Photos)
            {
                string NewuserAlbuminfo = string.Join(',', UserInfo.Name + "," + NewAlbum.Name + ',' +
                NewAlbum.Description + ',' + selectedphoto.Name + ',' + ((int)selectedphoto.Category) + ',' + false);

                File.AppendAllText(Filepath, NewuserAlbuminfo + Environment.NewLine);
            }


        }

    
        public bool DeleteAlbum(User UserInfo, Album NewAlbum)
        {

            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";


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
                                      NewAlbum.Description + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));

            
                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }
           
           return true;
        }

        public bool DeletePhotoFormAlbum(User UserInfo, Album NewAlbum ,Photo SelectedPhoto)
        {

            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";


            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

            var Validdata = PhotoAlbumList.Where(m => !(m.UserInfo.Name == UserInfo.Name &&
                                              m.Albumname == NewAlbum.Name &&
                                               m.AlbumDescription == NewAlbum.Description && m.PhotoName== SelectedPhoto.Name)).ToList();
            File.Delete(Filepath);

            List<string> NewuserAlbuminfo = new List<string>();

            foreach (var validdata in Validdata)
            {
                NewuserAlbuminfo.Add(string.Join(',', validdata.UserInfo.Name + "," + validdata.Albumname + ',' +
                                      NewAlbum.Description + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));


                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }

            return true;
        }


        public bool ChangeCoverPhoto(User UserInfo, Album NewAlbum, Photo SelectedPhoto)
        {

            var localPath = ApplicationData.Current.LocalFolder.Path;

            DirectoryInfo d = new DirectoryInfo(localPath + $"\\File");

            String Filepath = $"{d}\\PhotoAlbumInformation.csv";


            List<PhotoAlbumInformation> PhotoAlbumList = new List<PhotoAlbumInformation>();
            PhotoAlbumList = FetchingPhotoAlbumInformation(UserInfo);

            var OldCoverPhoto = PhotoAlbumList.Where(m => m.UserInfo.Name == UserInfo.Name &&
                                  m.Albumname == NewAlbum.Name &&
                                   m.AlbumDescription == NewAlbum.Description  && m.IsCoverphoto).ToList();

            var Validdata = PhotoAlbumList.Where(m => !(m.UserInfo.Name == UserInfo.Name &&
                                              m.Albumname == NewAlbum.Name &&
                                               m.AlbumDescription == NewAlbum.Description  && m.IsCoverphoto )).ToList();
            
            File.Delete(Filepath);

            List<string> NewuserAlbuminfo = new List<string>();

            foreach (var validdata in Validdata)
            {
                NewuserAlbuminfo.Add(string.Join(',', validdata.UserInfo.Name + "," + validdata.Albumname + ',' +
                                      NewAlbum.Description + ',' + validdata.PhotoName + ',' + ((int)validdata.Category) + ',' + validdata.IsCoverphoto));


                File.WriteAllLines(Filepath, NewuserAlbuminfo);
            }

            foreach (var oldphoto in OldCoverPhoto)
            {
                string Oldcoverohoto = string.Join(',', oldphoto.UserInfo.Name + "," + oldphoto.Albumname + ',' +
                oldphoto.AlbumDescription + ',' + oldphoto.PhotoName + ',' + ((int)oldphoto.Category) + ',' + false);

                File.AppendAllText(Filepath, Oldcoverohoto + Environment.NewLine);
            }
            string NewCoverPhoto = string.Join(',', UserInfo.Name + "," + NewAlbum.Name + ',' +
                NewAlbum.Description + ',' + SelectedPhoto.Name + ',' + ((int)SelectedPhoto.Category) + ',' + true);

            File.AppendAllText(Filepath, NewCoverPhoto + Environment.NewLine);

            return true;
        }

        // public void SaveInPhotoAlbum(PhotoAlbumInformation )
        //  public
    }
}

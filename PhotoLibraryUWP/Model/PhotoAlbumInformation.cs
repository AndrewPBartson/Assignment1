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
        public String Albumname { get; set; }
        public String AlbumDescription { get; set; }

        public string PhotoPath { get; set; }
        public string PhotoName { get; set; }
        public Boolean IsCoverphoto { get; set; }
        public PhotoCategory Category { get; set; }
        public Photo Coverphoto { get; set;}


        public PhotoAlbumInformation()
        {
            UserInfo = new User();
           // NewAlbum = new Album();
            
        }
        public PhotoAlbumInformation(User UserInfo, Album NewAlbum, List<Photo>  Photos , Photo Coverphoto)
        {
            //UserInfo = new User();
            //NewAlbum = new Album();

        }
        ///////
                             
        

        
    


        }

           



}




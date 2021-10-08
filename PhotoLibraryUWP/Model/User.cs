using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    class User
    {
        public static String CurrentUser { get; set; }
        public string Name { get; set; }
        //public List<Album> ListofAlbums { get; set; }
        public string Password { get; set; }
        public User()
        {
            //Name = "Swati";
            //ListofAlbums = new List<Album>(); 
        }

        public void getAlbum()
        {

        }
        
        //public void addAlbum (Album newAlbum)
        //{
        //    ListofAlbums.Add(newAlbum);
           
        //}
        
    }
}

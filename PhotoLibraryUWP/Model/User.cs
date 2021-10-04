using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    class User
    {
        public string Name { get; set; }
<<<<<<< Updated upstream
        public List<Album> ListofAlbums { get; set; }

        public User()
        {
            Name = "Swati";
            ListofAlbums = new List<Album>(); 
        }
=======
        //public List<Album> ListofAlbums { get; set; }
        public string Password { get; set; }
      
>>>>>>> Stashed changes

        public void getAlbum()
        {

        }
<<<<<<< Updated upstream
        
        public void addAlbum (Album newAlbum)
        {
            ListofAlbums.Add(newAlbum);
           
        }
        
=======

      //  public override User()


        //public void addAlbum (Album newAlbum)
        //{
        //    ListofAlbums.Add(newAlbum);

        //}

>>>>>>> Stashed changes
    }
}

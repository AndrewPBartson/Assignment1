using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    public class Album
    {
        public string Name { get; set; }
        public string Description { get; set; }

        List<Photo> ListofPhotos { get; set; }
        Photo coverPhoto { get; set; }

        public Album(string name, string description)
        {
            Name = name;
            Description = description;
            ListofPhotos = new List<Photo>();
        }

        public void addPhoto(Photo photo)
        {
            ListofPhotos.Add(photo);
        }
    }
}

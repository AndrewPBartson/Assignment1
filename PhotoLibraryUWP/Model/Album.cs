using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;

namespace PhotoLibraryUWP.Model
{
    public class Album : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Photo> ListofPhotos { get; set; }
        public Photo CoverPhoto { get; set; }

        public Album(string name, string description)
        {
            Name = name;
            Description = description;
            ListofPhotos = new List<Photo>();
            CoverPhoto = new Photo("NoPhoto", PhotoCategory.None);
        }


    }
}

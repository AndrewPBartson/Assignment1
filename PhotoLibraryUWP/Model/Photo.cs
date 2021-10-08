using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    public enum PhotoCategory
    {
        Animals,
        Beaches,
        Birds,
        Bridges,
        Butterflies,
        Cities,
        Flowers,
        None,
        People,
        Trees
    }

    public class Photo
    {
        public string Name { get; set; }
        public PhotoCategory Category { get; set; }
        public string PhotoPath { get; set; }

        public Photo(string name, PhotoCategory category)
        {
            Name = name;
            Category = category;
            PhotoPath = $"/Assets/Photos/{category}/{name}.png"; 
        }
    }
}

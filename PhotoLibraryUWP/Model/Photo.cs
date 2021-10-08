using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoLibraryUWP.Model
{
    public enum PhotoCategory
    {
        Animals = 1,
        Beaches = 2,
        Birds = 3,
        Bridges = 4,
        Butterflies = 5,
        Cities = 6,
        Flowers =7 ,
        Trees= 8 
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
        public Photo()
        {
        }

    }
}

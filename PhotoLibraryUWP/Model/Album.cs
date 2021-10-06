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
        //private string name;
        //private string description;
        //private List<Photo> listofphotos;
        //private Photo coverphoto;

        //public string Name
        //{
        //    get { return this.name; }
        //    set
        //    {
        //        if (value != this.name)
        //        {
        //            this.name = value;
        //            NotifyPropertyChanged();
        //        }

        //    }
        //}

        //public string Description
        //{
        //    get { return this.description; }
        //    set
        //    {
        //        if(value != this.description)
        //        {
        //            this.description = value;
        //            NotifyPropertyChanged();
        //        }
        //    }
        //}

        //public List<Photo> ListofPhotos
        //{
        //    get { return this.listofphotos; }
        //    set
        //    {   if(value!= this.listofphotos)
        //        {
        //            this.listofphotos = value;
        //            NotifyPropertyChanged();
        //        }
        //    }
            
        //}

        //public Photo CoverPhoto
        //{
        //    get { return this.coverphoto; }
        //    set
        //    {
        //        if(value != this.coverphoto)
        //        {
        //            this.coverphoto = value;
        //            NotifyPropertyChanged();
        //        }

        //    }
        //}

        public Album(string name, string description)
        {
            Name = name;
            Description = description;
            ListofPhotos = new List<Photo>();
            CoverPhoto = new Photo("NoPhoto", PhotoCategory.None);
        }


    }
}

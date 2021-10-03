using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PhotoLibraryUWP.Model;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoLibraryUWP
{
   
    public sealed partial class MainPage : Page
    {

        private List<MenuItem> MenuItems;
        public MainPage()
        {
            this.InitializeComponent();
            //MenuItems = new List<MenuItem>();
            //MenuItems.Add()
        }

        private void PhotoGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        

       

        private void PhotosButton_OnClick(object sender, RoutedEventArgs e)
        {
            


        }

        private void AlbumsButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {
           
            var ClickedItem = (string)e.ClickedItem;

            if (ClickedItem == "MyPhotos")
            { 
                //call show photos in PhotoGrid
            }
            else if(ClickedItem == "MyAlbums")
            { 
            }

        }
    }
}

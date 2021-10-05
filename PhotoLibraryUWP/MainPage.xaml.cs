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
using System.Diagnostics;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoLibraryUWP
{

    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Sample observable collection of albums
        /// </summary>
        private ObservableCollection<Album> albumList;

        /// <summary>
        /// Sample observable collection of Photos
        /// </summary>
        private ObservableCollection<Photo> photoList;

        private Album currentAlbum;
        private Photo currentPhoto;
        private List<Photo> selectedPhotos;

        public MainPage()
        {
            this.InitializeComponent();
            
            albumList = new ObservableCollection<Album>();
            photoList = new ObservableCollection<Photo>();

            // Sample albums added to Album list.
            var album1 = new Album("Album1", "Mumbai");
            albumList.Add(album1);

            var album2 = new Album("Album2", "Washington");          
            albumList.Add(album2);

            var album3 = new Album("Album3", "Oregon");
            albumList.Add(album3);

            album1.ListofPhotos.Add(new Photo("bear_cubs", PhotoCategory.Animals));
            album1.ListofPhotos.Add(new Photo("chinook", PhotoCategory.Animals));
            album1.ListofPhotos.Add(new Photo("elk", PhotoCategory.Animals));
            album1.ListofPhotos.Add(new Photo("foxes", PhotoCategory.Animals));

            album2.ListofPhotos.Add(new Photo("beach_sunset_people", PhotoCategory.Beaches));
            album2.ListofPhotos.Add(new Photo("hotel_beach", PhotoCategory.Beaches));
            album2.ListofPhotos.Add(new Photo("oregon", PhotoCategory.Beaches));
            album2.ListofPhotos.Add(new Photo("rocky_shore", PhotoCategory.Beaches));

            album3.ListofPhotos.Add(new Photo("eagle", PhotoCategory.Birds));
            album3.ListofPhotos.Add(new Photo("raven_closeup", PhotoCategory.Birds));
            album3.ListofPhotos.Add(new Photo("spotted_owl", PhotoCategory.Birds));

            
            //set the selected count to zero initially
            SelectedItemCountTextBlock.Text = "0";
        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {

            var ClickedItem = (string)e.ClickedItem;

            if (ClickedItem == "MyPhotos")
            {
                AlbumGridView.Visibility = Visibility.Collapsed;
                PhotoGridView.Visibility = Visibility.Visible;
                photoList.Clear();
                PhotoManager.GetAllPhotos(photoList);
                ClearAlbumGridViewSelection();
                HeaderTextBlock.Text = "My Photos";
            }
            else if (ClickedItem == "MyAlbums")
            {
                AlbumGridView.Visibility = Visibility.Visible;
                PhotoGridView.Visibility = Visibility.Collapsed;
                HeaderTextBlock.Text = "My Albums";
            }
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            // Popup should close on "Close" button click
            if (NewAlbumPopup.IsOpen)
            {
                NewAlbumPopup.IsOpen = false;
            }
        }

        private void AlbumGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewAlbumButton.IsEnabled = !(AlbumGridView.SelectedItems.Count > 0);
            SelectedItemCountTextBlock.Text = AlbumGridView.SelectedItems.Count.ToString();
            DeleteAlbumButton.IsEnabled = !NewAlbumButton.IsEnabled;
            EditAlbumButton.IsEnabled = AlbumGridView.SelectedItems.Count == 1;
        }

        private void PhotoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemCountTextBlock.Text = PhotoGridView.SelectedItems.Count.ToString();
        }

        private void CreateAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AlbumNameTxt.Text) && !string.IsNullOrWhiteSpace(AlbumDescriptionTxt.Text))
            {
                albumList.Add(new Album(AlbumNameTxt.Text, AlbumDescriptionTxt.Text));
                AlbumNameTxt.Text = "";
                AlbumDescriptionTxt.Text = "";

                if (AlbumGridView.Visibility != Visibility.Visible)
                {
                    AlbumGridView.Visibility = Visibility.Visible;
                    PhotoGridView.Visibility = Visibility.Collapsed;
                }

                if (NewAlbumPopup.IsOpen)
                {
                    NewAlbumPopup.IsOpen = false;
                }
            }

        }

        private void NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NewAlbumPopup.IsOpen)
            {
                NewAlbumPopup.IsOpen = true;
            }
        }

        private void EditAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;

            if (AlbumGridView.SelectedItem != null)
            {
                currentAlbum = (Album)AlbumGridView.SelectedItem;
                HeaderTextBlock.Text = currentAlbum.Name;
                photoList.Clear();
                foreach (var photo in currentAlbum.ListofPhotos)
                {
                    photoList.Add(photo);
                }
            }

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            PhotoManager.GetAllPhotos(photoList);
        }

        private void AlbumGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentAlbum = (Album)e.ClickedItem;
            selectedPhotos = new List<Photo>();
        }

        private void PhotoGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentPhoto = (Photo)e.ClickedItem;
            if(selectedPhotos == null)
            {
                selectedPhotos = new List<Photo>();
            }
            selectedPhotos.Add(currentPhoto);
        }

        private void SaveAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            currentAlbum.addPhotos(selectedPhotos);
           
        }

        private void DeleteAlbumButton_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void ClearAlbumGridViewSelection()
        {
            if (AlbumGridView.SelectedItems != null && AlbumGridView.SelectedItems.Count > 0)
            {
                AlbumGridView.SelectedItem = null;
            }
        }
    }
}

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
        private string userName = "Swati";

        public MainPage()
        {
            this.InitializeComponent();

            // This is required as the Command bar opens on the First click and then the buttons can be clicked.
            // Once we add the Icons and Label to the CommandBar this will not be needed.
            MainCommandBar.IsOpen = true;

            albumList = new ObservableCollection<Album>();
            photoList = new ObservableCollection<Photo>();
            
            // Sample albums added to Album list.
            //albumList.Add(new Album("Album1", "Mumbai"));
            //albumList.Add(new Album("Album2", "Delhi"));

            //set the selected count to zero initially
            SelectedItemCountTextBlock.Text = "0";
            
           

            AlbumManager.readUserAlbum(userName, albumList);
        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {

            var ClickedItem = (string)e.ClickedItem;

            if (ClickedItem == "MyPhotos")
            {
                AlbumGridView.Visibility = Visibility.Collapsed;
                PhotoGridView.Visibility = Visibility.Visible;
            }
            else if (ClickedItem == "MyAlbums")
            {
                AlbumGridView.Visibility = Visibility.Visible;
                PhotoGridView.Visibility = Visibility.Collapsed;
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
            if (!EditAlbumPopup.IsOpen)
            {
                EditAlbumPopup.IsOpen = true;
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
            selectedPhotos.Add(currentPhoto);
        }

        private void SaveAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumManager.addPhotos(selectedPhotos, currentAlbum);
            AlbumManager.saveToFile(currentAlbum, userName);
            AlbumManager.readUserAlbum(userName, albumList);
            AlbumGridView.Visibility = Visibility.Visible;
            PhotoGridView.Visibility = Visibility.Collapsed;
            HeaderTextBlock.Text = "Your Albums";

        }

        private void DeleteAlbumButton_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void EditAlbumDetailsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseEditPopupButton_Click(object sender, RoutedEventArgs e)
        { 
            // Popup should close on "Close" button click
            if (EditAlbumPopup.IsOpen)
                {
                    EditAlbumPopup.IsOpen = false;
                }
        }
    }
}

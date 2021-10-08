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
        private string userName = "TestUser";

        public MainPage()
        {
            this.InitializeComponent();

            // This is required as the Command bar opens on the First click and then the buttons can be clicked.
            // Once we add the Icons and Label to the CommandBar this will not be needed.
            MainCommandBar.IsOpen = true;

            albumList = new ObservableCollection<Album>();
            photoList = new ObservableCollection<Photo>();
            
            AlbumManager.readUserAlbum(userName, albumList);

            //AlbumEnableDisable(false);
            //EditEnableDisable(false);

        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ClickedItem = (string)e.ClickedItem;

            if (ClickedItem == "My Photos")
            {
                AlbumGridView.Visibility = Visibility.Collapsed;
                PhotoGridView.Visibility = Visibility.Visible;
                photoList.Clear();
                ClearAlbumGridViewSelection();
                HeaderTextBlock.Text = "My Photos";
                AlbumEnableDisable(false);
                EditEnableDisable(false);
            }
            else if (ClickedItem == "My Albums")
            {
                AlbumGridView.Visibility = Visibility.Visible;
                PhotoGridView.Visibility = Visibility.Collapsed;
                HeaderTextBlock.Text = "My Albums";
                AlbumEnableDisable(true);
                EditEnableDisable(false);
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
            DeleteAlbumButton.IsEnabled = !NewAlbumButton.IsEnabled;
            EditAlbumButton.IsEnabled = AlbumGridView.SelectedItems.Count == 1;
        }

        private void PhotoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            EditEnableDisable(true);

            /* CHANGE COVER PHOTO
             
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;

            if (AlbumGridView.SelectedItem != null)
            {
                EditEnableDisable(true);
                DeleteAlbumButton.IsEnabled = false;
                SaveAlbumButton.IsEnabled = false;
                currentAlbum = (Album)AlbumGridView.SelectedItem;
                HeaderTextBlock.Text = currentAlbum.Name;
                photoList.Clear();
                foreach (var photo in currentAlbum.ListofPhotos)
                {
                    photoList.Add(photo);
                }
            }  
            
             */

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            SaveAlbumButton.IsEnabled = true;
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
            if (selectedPhotos == null)
            {
                selectedPhotos = new List<Photo>();
            }
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
            AlbumManager.deleteUserAlbum(currentAlbum.Name, userName);

        }

        private void ClearAlbumGridViewSelection()
        {
            if (AlbumGridView.SelectedItems != null && AlbumGridView.SelectedItems.Count > 0)
            {
                AlbumGridView.SelectedItem = null;
            }
        }

        private void ChangeCoverPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var setcoverphoto = (Photo)PhotoGridView.SelectedItem;
            var selectedAlbum = (Album)AlbumGridView.SelectedItem;
            if (selectedAlbum == null || setcoverphoto == null)
            {
                return;
            }
            foreach (var album in albumList)
            {
                if (album.Name == selectedAlbum.Name && album.Description == selectedAlbum.Description)
                {
                    album.CoverPhoto = setcoverphoto;
                    break;
                }
            }
        }


        private void EditEnableDisable(bool isenabled)
        {
            SaveAlbumButton.IsEnabled = isenabled;
            AddPhotoButton.IsEnabled = isenabled;
            RemovePhotoButton.IsEnabled = isenabled;
            ChangeCoverPhotoButton.IsEnabled = isenabled;
        }

        private void AlbumEnableDisable(bool isenabled)
        {
            NewAlbumButton.IsEnabled = isenabled;
            DeleteAlbumButton.IsEnabled = isenabled;
            EditAlbumButton.IsEnabled = isenabled;
        }

        private void EditAlbumDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AlbumNewNameTxt.Text) && !string.IsNullOrWhiteSpace(AlbumNewDescriptionTxt.Text))
            {
                string previousName = currentAlbum.Name;
                currentAlbum.Name = AlbumNewNameTxt.Text;
                currentAlbum.Description = AlbumNewDescriptionTxt.Text;
                               
                AlbumManager.saveToFile(currentAlbum, userName);
                AlbumNewNameTxt.Text = "";
                AlbumNewDescriptionTxt.Text = "";
                AlbumManager.deleteUserAlbum(previousName, userName);
                if (AlbumGridView.Visibility != Visibility.Visible)
                {
                    AlbumGridView.Visibility = Visibility.Visible;
                    PhotoGridView.Visibility = Visibility.Collapsed;
                }

                if (EditAlbumPopup.IsOpen)
                {
                    EditAlbumPopup.IsOpen = false;
                }
            }
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

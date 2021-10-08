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
            AlbumManager.readUserAlbum(userName, albumList);
            AlbumEnableDisable(false);
            EditEnableDisable(false);
        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ClickedItem = (string)e.ClickedItem;
            if (ClickedItem == "MyPhotos")
            {
                AlbumGridView.Visibility = Visibility.Collapsed;
                PhotoGridView.Visibility = Visibility.Visible;
                photoList.Clear();
                ClearAlbumGridViewSelection();
                HeaderTextBlock.Text = "My Photos";
                AlbumEnableDisable(false);
                EditEnableDisable(false);
            }
            else if (ClickedItem == "MyAlbums")
            {
                AlbumGridView.Visibility = Visibility.Visible;
                PhotoGridView.Visibility = Visibility.Collapsed;
                HeaderTextBlock.Text = "My Albums";
                NewAlbumButton.IsEnabled = true;
            }
        }

        private void AlbumGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentAlbum = (Album)e.ClickedItem;
            AlbumEnableDisable(true);
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
        

        private void AlbumGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NewAlbumButton.IsEnabled = !(AlbumGridView.SelectedItems.Count > 0);
            DeleteAlbumButton.IsEnabled = !NewAlbumButton.IsEnabled;
            EditAlbumButton.IsEnabled = AlbumGridView.SelectedItems.Count == 1;
        }

        private void PhotoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NewAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NewAlbumPopup.IsOpen)
            {
                NewAlbumPopup.IsOpen = true;
            }
        }
        private void CreateAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AlbumNameTxt.Text) && !string.IsNullOrWhiteSpace(AlbumDescriptionTxt.Text))
            {
                Album newAlbum = new Album(AlbumNameTxt.Text, AlbumDescriptionTxt.Text);
                albumList.Add(newAlbum);
                AlbumManager.saveToFile(newAlbum, userName);
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
        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            // Popup should close on "Close" button click
            if (NewAlbumPopup.IsOpen)
            {
                NewAlbumPopup.IsOpen = false;
            }
        }

        private void EditAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EditAlbumPopup.IsOpen)
            {
               EditAlbumPopup.IsOpen = true;
            }
        }

        private void EditAlbumDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AlbumNewNameTxt.Text) && !string.IsNullOrWhiteSpace(AlbumNewDescriptionTxt.Text))
            {
                var previousAlbum = currentAlbum;
                currentAlbum.Name = AlbumNewNameTxt.Text;
                currentAlbum.Description = AlbumNewDescriptionTxt.Text;

                AlbumManager.saveToFile(currentAlbum, userName);
                albumList.Add(currentAlbum);
                AlbumNewNameTxt.Text = "";
                AlbumNewDescriptionTxt.Text = "";
                AlbumManager.deleteUserAlbum(previousAlbum, userName);
                albumList.Remove(previousAlbum);
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

        private void DeleteAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumManager.deleteUserAlbum(currentAlbum, userName);
            albumList.Remove(currentAlbum);
        }

        private void AlbumGridView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            EditEnableDisable(true);
            AlbumManager.displayUserPhotosByAlbum(currentAlbum, photoList);
            HeaderTextBlock.Text = $"Your Photos in {currentAlbum.Name} Album";
        }

        private void RemovePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var photo in selectedPhotos)
            {
                currentAlbum.ListofPhotos.Remove(photo);
            }
            AlbumManager.saveToFile(currentAlbum, userName);
            AlbumManager.readUserAlbum(userName, albumList);
            selectedPhotos = null;
        }

        private void ChangeCoverPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            currentAlbum.CoverPhoto = currentPhoto;
            AlbumManager.saveToFile(currentAlbum, userName);
            AlbumManager.displayUserPhotosByAlbum(currentAlbum, photoList);
            AlbumManager.readUserAlbum(userName, albumList);
            selectedPhotos = null;
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            SaveAlbumButton.IsEnabled = true;
            PhotoManager.GetAllPhotos(photoList);
        }

        private void SaveAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            AlbumManager.addPhotosToAlbum(selectedPhotos, currentAlbum);
            AlbumManager.saveToFile(currentAlbum, userName);
            AlbumManager.readUserAlbum(userName, albumList);
            AlbumGridView.Visibility = Visibility.Visible;
            PhotoGridView.Visibility = Visibility.Collapsed;
            HeaderTextBlock.Text = "Your Albums";
            selectedPhotos = null;
        }

        private void ClearAlbumGridViewSelection()
        {
            if (AlbumGridView.SelectedItems != null && AlbumGridView.SelectedItems.Count > 0)
            {
                AlbumGridView.SelectedItem = null;
            }
        }
        
        private void AlbumEnableDisable(bool isenabled)
        {
            NewAlbumButton.IsEnabled = isenabled;
            DeleteAlbumButton.IsEnabled = isenabled;
            EditAlbumButton.IsEnabled = isenabled;
        }
        private void EditEnableDisable(bool isenabled)
        {
            SaveAlbumButton.IsEnabled = isenabled;
            AddPhotoButton.IsEnabled = isenabled;
            RemovePhotoButton.IsEnabled = isenabled;
            ChangeCoverPhotoButton.IsEnabled = isenabled;
        }
    }
}

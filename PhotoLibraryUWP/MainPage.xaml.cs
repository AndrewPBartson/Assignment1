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
        static User CurrentUser;
        private String APPUsername;
        public MainPage()
        {
            // string Currentuser = UserManagement.CurrentUser;
            CurrentUser = UserManagement.CurrentAppUser;
            APPUsername = CurrentUser.Name;
            this.InitializeComponent();
           
            albumList = new ObservableCollection<Album>();
            photoList = new ObservableCollection<Photo>();

           
            AlbumEnableDisable(false);
            EditEnableDisable(false);

        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {   
            var ClickedItem = (string)e.ClickedItem;
            if (ClickedItem == "AllPhotos")
            {
                ShowPhotoinGrid();
             //   PhotoGridView.IsItemClickEnabled = false;
             //  PhotoGridView.IsMultiSelectCheckBoxEnabled = false;
             // PhotoGridView.IsEnabled = false;


            }
            else if (ClickedItem == "MyPhotos")
            {
                PhotoManager.GetMyPhotos(photoList);
                AlbumGridView.Visibility = Visibility.Collapsed;
                PhotoGridView.Visibility = Visibility.Visible;
                PhotoGridView.IsItemClickEnabled = false;
                PhotoGridView.IsMultiSelectCheckBoxEnabled = false;
                //photoList.Clear();
                ClearAlbumGridViewSelection();
                HeaderTextBlock.Text = "My Photos";
                AlbumEnableDisable(false);
                EditEnableDisable(false);
                

            }
            else if (ClickedItem == "MyAlbums")
            {
                showAlbuminGrid();
            }
        }

        private void showAlbuminGrid()
        {
            AlbumManager.GetMyAlbums(albumList);
            AlbumGridView.Visibility = Visibility.Visible;
            PhotoGridView.Visibility = Visibility.Collapsed;
            HeaderTextBlock.Text = "My Albums";
            AlbumEnableDisable(true);
            EditEnableDisable(false);
        }
        private void ShowPhotoinGrid()
        {
            PhotoManager.GetAllPhotos(photoList);
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;

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
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;

            if (AlbumGridView.SelectedItem != null)
            {
                PhotoGridView.IsItemClickEnabled = true;
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

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            PhotoGridView.IsItemClickEnabled = true;
            PhotoGridView.IsMultiSelectCheckBoxEnabled = true;
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
            currentAlbum.addPhotos(selectedPhotos);

            UserDataFile newAlbum = new UserDataFile();

            newAlbum.SavingPhotoAlbum(CurrentUser, currentAlbum, selectedPhotos, selectedPhotos.FirstOrDefault());
           
            
            AlbumGridView.Visibility = Visibility.Visible;
            PhotoGridView.Visibility = Visibility.Collapsed;
        }

        private void DeleteAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            ManageDataFile  newAlbum = new ManageDataFile();
            if ( newAlbum.DeletePhotoAlbum(CurrentUser, currentAlbum))
            {
                showAlbuminGrid();
            } 

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
                    ManageDataFile NewCoverPhoto = new ManageDataFile();
                    NewCoverPhoto.ChangeCoverPhoto(CurrentUser, currentAlbum , setcoverphoto);
                    AlbumGridView.Visibility = Visibility.Visible;
                    PhotoGridView.Visibility = Visibility.Collapsed;
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

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
             
            ManageDataFile newPhoto = new ManageDataFile();
            if (currentPhoto is null) return;
            
            if (newPhoto.RemovePhotoFromAlbum(CurrentUser, currentAlbum, currentPhoto))
            {

                photoList.Clear();
                foreach (var photo in currentAlbum.ListofPhotos)
                {
                    if (photo != currentPhoto)
                    photoList.Add(photo);
                }

            }
            else
            {
            }

            
        }
    }
}

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
using System.Threading.Tasks;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoLibraryUWP
{

    public sealed partial class MainPage : Page
    {



        private ObservableCollection<Album> albumList;
        private ObservableCollection<Photo> photoList;

        private Album currentAlbum;
        private Photo currentPhoto;
        private List<Photo> selectedPhotos;
        static User CurrentUser;
        private String APPUsername;
        public MainPage()
        {

            CurrentUser = UserManagement.CurrentAppUser;
            APPUsername = CurrentUser.Name;


            this.InitializeComponent();

            albumList = new ObservableCollection<Album>();
            photoList = new ObservableCollection<Photo>();
            UsernameTextbox.Text = $"Welcome { CurrentUser.Name} ";
            AlbumEnableDisable(false);
            EditEnableDisable(false);
            HeaderTextBlock.Text = "";
        }

        private void MainFeatureListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ClickedItem = (string)e.ClickedItem;

            if (ClickedItem == "All Photos")
            {
                PhotoGridView.IsItemClickEnabled = false;
                PhotoGridView.IsMultiSelectCheckBoxEnabled = false;
                HeaderTextBlock.Text = "All Photos";
                ShowPhotoinGrid();



            }
            else if (ClickedItem == "My Photos")
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
            else if (ClickedItem == "My Albums")
            {
                PhotoGridView.IsItemClickEnabled = true;
                PhotoGridView.IsMultiSelectCheckBoxEnabled = true;
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
            if (AlbumGridView.SelectedItem != null && !((Album)AlbumGridView.SelectedItem).ListofPhotos.Any())
            {
                EditAlbumButton.IsEnabled = false;
            }
            else
            {
                EditAlbumButton.IsEnabled = AlbumGridView.SelectedItems.Count == 1;
            }
        }

        private void PhotoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AlbumGridView.SelectedItem != null && PhotoGridView.SelectedItems != null && PhotoGridView.SelectedItems.Count == 1)
            {
                ChangeCoverPhotoButton.IsEnabled = true;
            }
            else
            {
                ChangeCoverPhotoButton.IsEnabled = false;
            }
        }

        private void CreateAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AlbumNameTxt.Text) && !string.IsNullOrWhiteSpace(AlbumDescriptionTxt.Text))
            {
                var newAlbum = new Album(AlbumNameTxt.Text, AlbumDescriptionTxt.Text);
                albumList.Add(newAlbum);
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

            if (currentAlbum != null && currentAlbum.ListofPhotos.Any())
            {

                if (!EditAlbumPopup.IsOpen)
                {
                    EditAlbumPopup.IsOpen = true;
                    AlbumNewNameTxt.Text = currentAlbum.Name;
                    AlbumNewDescriptionTxt.Text = currentAlbum.Description;

                }
            }
        }

        private void EditAlbumDetailsButton_Click(object sender, RoutedEventArgs e)
        {

            if (AlbumNewNameTxt.Text == "" || AlbumNewDescriptionTxt.Text == "")
            {

                DisplayMessage("Album Name and Description need values! ", "EditingAlbumError");
                return;
            }


            UserDataFile newAlbum = new UserDataFile();

            newAlbum.DeleteAlbum(CurrentUser, currentAlbum);


            currentAlbum.Name = AlbumNewNameTxt.Text;
            currentAlbum.Description = AlbumNewDescriptionTxt.Text;

            newAlbum.SavingPhotoAlbum(CurrentUser, currentAlbum, currentAlbum.ListofPhotos, currentAlbum.CoverPhoto);

            AlbumNewNameTxt.Text = "";
            AlbumNewDescriptionTxt.Text = "";
            if (EditAlbumPopup.IsOpen)
            {
                EditAlbumPopup.IsOpen = false;
            }
            showAlbuminGrid();
            DisplayMessage("You Album name an descriotion successfully changed", "Edit ALbum ");
        }

        private void CloseEditPopupButton_Click(object sender, RoutedEventArgs e)
        {
            // Popup should close on "Close" button click
            if (EditAlbumPopup.IsOpen)
            {
                EditAlbumPopup.IsOpen = false;
            }
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            selectedPhotos = null;
            PhotoGridView.IsItemClickEnabled = true;
            PhotoGridView.IsMultiSelectCheckBoxEnabled = true;
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            SaveAlbumButton.IsEnabled = true;
            AddPhotoButton.IsEnabled = false;
            PhotoManager.GetAllPhotos(photoList);

        }

        private async void AlbumGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentAlbum = (Album)e.ClickedItem;
            selectedPhotos = new List<Photo>();

            var gridView = sender as GridView;
            if (e.ClickedItem == gridView.SelectedItem)
            {
                await Task.Delay(10);
                gridView.SelectedItem = null;
            }
        }

        private void AlbumGridView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            AlbumGridView.Visibility = Visibility.Collapsed;
            PhotoGridView.Visibility = Visibility.Visible;
            EditEnableDisable(true);
            AlbumEnableDisable(false);

            AlbumManager.displayUserPhotosByAlbum(currentAlbum, photoList);

            HeaderTextBlock.Text = $"Your Photos in {currentAlbum.Name} Album";
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
            AlbumEnableDisable(true);
            EditEnableDisable(false);

        }

        private void DeleteAlbumButton_Click(object sender, RoutedEventArgs e)
        {

            ManageDataFile newAlbum = new ManageDataFile();

            if (newAlbum.DeletePhotoAlbum(CurrentUser, currentAlbum))
            {
                showAlbuminGrid();
                DisplayMessage("You album  deleted successfully ", "Delete ALbum ");
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

            if (currentAlbum == null || setcoverphoto == null)

            {

                return;
            }

            currentAlbum.CoverPhoto = setcoverphoto;
            ManageDataFile NewCoverPhoto = new ManageDataFile();
            NewCoverPhoto.ChangeCoverPhoto(CurrentUser, currentAlbum, setcoverphoto);
            AlbumGridView.Visibility = Visibility.Visible;
            PhotoGridView.Visibility = Visibility.Collapsed;
            DisplayMessage("You album cover photo has been successfully changed", "Cover photo Changed ");
        }


        private void EditEnableDisable(bool isenabled)
        {
            SaveAlbumButton.IsEnabled = isenabled;
            AddPhotoButton.IsEnabled = isenabled;
            RemovePhotoButton.IsEnabled = isenabled;
        }

        private void AlbumEnableDisable(bool isenabled)
        {
            NewAlbumButton.IsEnabled = isenabled;
            DeleteAlbumButton.IsEnabled = isenabled;
            EditAlbumButton.IsEnabled = isenabled;
            ChangeCoverPhotoButton.IsEnabled = isenabled;
            if (AlbumGridView.SelectedItem == null && isenabled)
            {
                EditAlbumButton.IsEnabled = false;
                ChangeCoverPhotoButton.IsEnabled = false;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

            if (PhotoGridView.SelectedItems == null) return;
            if (selectedPhotos is null || selectedPhotos.Count() == 0)
            {
                DisplayMessage("Select photo(s) for deteting!", "Delete Alarm");
                return;
            }
            ManageDataFile newPhoto = new ManageDataFile();
            if (newPhoto.RemovePhotoFromAlbum(CurrentUser, currentAlbum, selectedPhotos))
            {

                foreach (var photo in selectedPhotos)
                {
                    currentAlbum.ListofPhotos.Remove(photo);
                    photoList.Remove(photo);
                }
                DisplayMessage("Your photo(s) have been deleted successfully!!", "Delete Successful");
                selectedPhotos = null;
            }
            else
            {

            }


        }

        private void DisplayMessage(string message, string Title)
        {

            MessageDialog messageDialog = new MessageDialog(message, Title);
            messageDialog.ShowAsync();
        }

    }
}

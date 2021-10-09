using System;
using System.Collections.Generic;
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
using Windows.UI.Popups;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PhotoLibraryUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserLogin : Page
    {
        public static MainPage Current;
        public UserLogin()
        {
            this.InitializeComponent();
            //Cur  rent = this;
            UsernameTextBox.TabIndex = 0;
            PasswordTextBox.TabIndex = 1;
            LoginButton.TabIndex = 3;
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String Username;
            String Password;

            Username = UsernameTextBox.Text.Trim();
            Password = PasswordTextBox.Password.Trim();


            if ((Username == "") || (Password == ""))
            {
                string message = "Username or Password are Empty";

                // Initialize a new MessageDialog instance
                MessageDialog messageDialog = new MessageDialog(message, "Authentication Error");
                 messageDialog.ShowAsync();

                return;

            }


            UserManagement Usermatch = new UserManagement();
            if (Usermatch.IsvalidUser(Username, Password))
            {
                 

                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainPage));
            }
            else {
                string message = "Username or Password does not exist";

                // Initialize a new MessageDialog instance
                MessageDialog messageDialog = new MessageDialog(message, "Authentication Error");


                messageDialog.ShowAsync();

                return;
            }
        }
    }
}
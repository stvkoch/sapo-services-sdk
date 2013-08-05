using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Videos.Pages;
using Videos.SapoServices;
using Videos.VideosReference;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Videos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App app = Application.Current as App;

            if (app == null) return;

            this.tbEsbUsername.Text = app.EsbUsername;
            this.tbEsbPassword.Text = app.EsbPassword;
            this.tbEsbAccessKey.Text = app.EsbAccessKey;
        }

        private void UpdateCredentials()
        {
            App app = Application.Current as App;

            if (app == null) return;

            app.EsbUsername = this.tbEsbUsername.Text;
            app.EsbPassword = this.tbEsbPassword.Text;
            app.EsbAccessKey = this.tbEsbAccessKey.Text;
        }

        private void BtAddVideoClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(AddVideo));
        }

        private void BtDeleteVideoClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(DeleteVideo));
        }

        private void BtEditVideoClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(EditVideo));
        }

        private void BtGetVideoClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(GetVideo));
        }

        private void BtGetUserClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(GetUser));
        }

        private void BtGetUserVideosClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof(GetUserVideos));
        }
    }
}

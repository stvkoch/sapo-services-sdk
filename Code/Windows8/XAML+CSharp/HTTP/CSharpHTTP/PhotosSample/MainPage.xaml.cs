using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PhotosSample.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PhotosSample
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
        }

        private void UpdateCredentials()
        {
            App app = Application.Current as App;

            if (app == null) return;

            app.EsbUsername = this.tbEsbUsername.Text;
            app.EsbPassword = this.tbEsbPassword.Text;
            app.EsbAccessKey = this.tbEsbAccessKey.Text;
        }

        private void BtDummyEchoClick(object sender, RoutedEventArgs e)
        {
            this.UpdateCredentials();
            this.Frame.Navigate(typeof (DummyEcho));
        }

        private void BtCreatePhotoClick(object sender, RoutedEventArgs e)
        {
            const string baseUri = "http://services.sapo.pt/Photos/";

            Uri uri = new Uri(baseUri);

            UriBuilder uriBuilder = new UriBuilder(uri);
            uriBuilder.Path += "DummyEcho";

            uriBuilder.Query = "ESBUsername=user&ESBPassword=pass";



        }

        private void BtDeletePhotoClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetPhotoDetailsClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtCreateAlbumClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtAddPhotoToALbumClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetPhotosByTermsClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetPhotosByTagsClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetUserPhotosClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetUserAlbumsClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtGetUserAlbumsPhotosClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

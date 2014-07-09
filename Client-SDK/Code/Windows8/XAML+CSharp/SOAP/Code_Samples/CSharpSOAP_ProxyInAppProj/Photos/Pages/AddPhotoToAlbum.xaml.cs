using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photos.SapoServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Photos.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddPhotoToAlbum : Photos.Common.LayoutAwarePage
    {
        public AddPhotoToAlbum()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void BtAddPhotoClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;

            if (app == null)
                return;

            var client = new PhotosServiceClient(app.EsbUsername, app.EsbPassword, app.EsbAccessKey);


            //You MUST provide to albums for this example to work
            bool result =
                await
                client.AddImageToAlbums(this.tb_PhotoUidForAddPhotoToAlbum.Text,
                                                     new int[]
                                                         {
                                                             Convert.ToInt32(this.tb_Album1AidForAddPhotoToAlbum.Text),
                                                             Convert.ToInt32(this.tb_Album2AidForAddPhotoToAlbum.Text)
                                                         });

            this.tblock_Result.Text = result ? "Photo sucessfully added to the specified albums" : "Error!";
        }
    }
}

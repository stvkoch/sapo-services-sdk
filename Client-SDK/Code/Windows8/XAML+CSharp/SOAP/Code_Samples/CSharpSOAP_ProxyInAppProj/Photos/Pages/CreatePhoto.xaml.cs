using System;
using System.Collections.Generic;
using Photos.Common;
using Photos.SapoServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Image = Photos.PhotosReference.Image;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Photos.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CreatePhoto : Photos.Common.LayoutAwarePage
    {
        public CreatePhoto()
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

        private async void BtPickPhotoClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
                                            {
                                                ViewMode = PickerViewMode.Thumbnail,
                                                SuggestedStartLocation = PickerLocationId.PicturesLibrary
                                            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                App app = Application.Current as App;

                if (app == null)
                    return;

                var client = new PhotosServiceClient(app.EsbUsername, app.EsbPassword, app.EsbAccessKey);

                Image img = new Image
                {
                    title = file.DisplayName,
                    tags = file.DisplayName
                };

                CreateImageResult createImageResult = await client.CreateImageAsync(file, img);
                this.tblock_Result.Text = createImageResult.ResponseResult;
                if (createImageResult.Img != null)
                    this.tblock_Result.Text += " -> uri: " + createImageResult.Img.url;
            }
            else
            {
                this.tblock_Result.Text = "Error reading file";
            }
        }
    }
}

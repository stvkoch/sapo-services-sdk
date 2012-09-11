using System;
using System.Collections.Generic;
using PhotosProxy.PhotosServiceReference;
using PhotosProxy.SapoServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Image = PhotosProxy.PhotosServiceReference.Image;

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


        //internal bool EnsureUnsnapped()
        //{
        //    // FilePicker APIs will not work if the application is in a snapped state.
        //    // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
        //    bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
        //    if (!unsnapped)
        //    {

        //    }

        //    return unsnapped;
        //}

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
                // Application now has read/write access to the picked file

                //PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                //    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);
                //this.tblock_dummyEchoResult.Text = await photosServiceClient
                //    .DummyEchoAsync("Windows 8");
                //PostImage(file);

                PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

                Image img = new Image
                {
                    title = file.DisplayName,
                    tags = file.DisplayName
                };

                CreateImageResult createImageResult = await photosServiceClient.CreateImageOperationAsync(file, img);
                this.tblock_PostPhotoResult.Text = createImageResult.ResponseResult;
                if (createImageResult.Img != null)
                    this.tblock_PostPhotoResult.Text += " -> uri: " + createImageResult.Img.url;
            }
            else
            {
                this.tblock_PostPhotoResult.Text = "Error reading file";
            }
        }

        private async void BtDeletePhotoClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            bool result = await photosServiceClient.DeleteImageOperationAsync(this.tb_photoId.Text);

            this.tblock_deletePhotoResult.Text = result ? "Photo sucessfully deleted" : "Error!";

            //this.tblock_deletePhotoResult.Text = await photosServiceClient.DummyEchoAsync("Benfica");
        }

        private async void BtGetUserClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            User user = await photosServiceClient.GetUserDetailsOperationAsync(this.tb_username.Text);

            this.tblock_getUserResult.Text = String.Format("username: {0}, url: {1}, date: {2}",
                                                           user.username, user.url, user.creationDate);

        }

        //Object Views has the photos url
        private async void BtGetImageDetailsClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            Image img = await photosServiceClient.GetImageDetailsOperationAsync(this.tb_imageUid.Text);


            if (img != null)
                this.tblock_getImageDetailsResult.Text =
                    String.Format("Uri: {0}, username: {1}, title: {2}, date: {3}",
                        img.url, img.user.username, img.title,
                        img.creationDate);
            else
                this.tblock_getImageDetailsResult.Text = "Photo not found";

        }

        private async void BtCreateAlbumClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            Album album = new Album
            {
                title = this.tb_albumName.Text
            };

            album = await photosServiceClient.CreateAlbumOperationAsync(album);

            this.tblock_createAlbumResult.Text = String.Format("Uri: {0}, username: {1}", album.url, album.user.username);
        }

        private async void BtAddImageToAlbumClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            bool result =
                await
                photosServiceClient.AddImageToAlbumsArrayIdsOperationAsync(this.tb_imageUidAddImageToAlbum.Text,
                                                     new int[] { Convert.ToInt32(this.tb_albumIdToAlbum.Text) });

            this.tblock_addImageToAlbumResult.Text = result ? "Photo sucessfully added to album" : "Error!";
        }

        private async void BtGetUserAlbumsListClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            IEnumerable<Album> albums = await photosServiceClient.GetUserAlbumsListOperationAsync();

            //bool result =
            //    await
            //    photosServiceClient.AddImageToAlbums("ck0nHnR3fdDeLtp42SHw",
            //                                         albums);
        }

        private async void BtDummyEchoClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);
            string result = await photosServiceClient.DummyEchoAsync("Windows 8");
        }

        private async void BtGetUserImageList(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            var images = await photosServiceClient.GetUserImageListOperationAsync();

        }

        private async void BtGetUserTagsClick(object sender, RoutedEventArgs e)
        {
            PhotosServiceClient photosServiceClient = new PhotosServiceClient(
                    this.tb_authUsername.Text, this.tb_authPassword.Text, this.tb_authAccessKey.Text);

            var tags = await photosServiceClient.GetUserTagsOperationAsync();
        }
    }
}

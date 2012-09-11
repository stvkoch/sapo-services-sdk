using System.Collections.Generic;
using System.IO;
using PhotosProxy.PhotosServiceReference;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using PhotosProxy.SapoServices.Utils;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using System.Linq;

namespace PhotosProxy.SapoServices
{
    public sealed class PhotosServiceClient
    {
        private const string FotosSapoPtInterface = "fotos.sapo.pt";
        private const string HttpFotosSapoPtUploadpostHtmlUri = "http://fotos.sapo.pt/uploadPost.html";
        private readonly PhotosSoapSecureClient _client;
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessKey { get; set; }

        public PhotosServiceClient(string username, string password, string accesskey)
        {
            this.Username = username;
            this.Password = password;
            this.AccessKey = accesskey;
            this._client = new PhotosSoapSecureClient();
        }

        #region Test Operations

        public IAsyncOperation<string> DummyEchoAsync(string echoStr)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return _client
                        .DummyEchoAsync(echoStr)
                        .ContinueWith(t => t.Result.DummyEchoResult)
                        .AsAsyncOperation();
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        #endregion

        #region Image Related

        #region Auxiliar

        private static async Task<UploadOperation> CreateUploadOperationForCreateImage(
            IStorageFile file, string token, BackgroundUploader uploader)
        {
            const string boundary = "imgboundary";

            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();

            BackgroundTransferContentPart metadataPart = new BackgroundTransferContentPart("token");

            metadataPart.SetText(token);
            //metadataPart.SetText("iamatoken");
            parts.Add(metadataPart);

            BackgroundTransferContentPart imagePart = new BackgroundTransferContentPart("photo", file.Name);
            imagePart.SetFile(file);
            imagePart.SetHeader("Content-Type", file.ContentType);
            parts.Add(imagePart);

            return
                await uploader.CreateUploadAsync(
                    new Uri(HttpFotosSapoPtUploadpostHtmlUri),
                    parts,
                    "form-data",
                    boundary);
        }

        #endregion

        #region CRUD

        internal async Task<CreateImageResult> CreateImageAsync(IStorageFile file, Image img)
        {
            ImageCreateResult res = null;
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    res = await this._client.ImageCreateAsync(img, FotosSapoPtInterface);
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            if (res != null && res.result.ok)
            {
                BackgroundUploader uploader = new BackgroundUploader();

                //Default HTTP Method is POST (http://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.networking.backgroundtransfer.backgrounduploader.method.aspx)
                //uploader.Method = "POST";

                UploadOperation uploadOperation = 
                    await PhotosServiceClient.CreateUploadOperationForCreateImage(file, res.token, uploader);

                await uploadOperation.StartAsync();

                ResponseInformation responseInformation = uploadOperation.GetResponseInformation();

                uint contentLength = Convert.ToUInt32(responseInformation.Headers["Content-Length"]);

                uint statusCode = responseInformation.StatusCode;

                IInputStream resultStreamAt = uploadOperation.GetResultStreamAt(0);

                IBuffer result = await resultStreamAt.ReadAsync(
                    new Windows.Storage.Streams.Buffer(contentLength),
                    contentLength,
                    InputStreamOptions.None);

                Stream responseStream = result.AsStream();

                XDocument xDocument = XDocument.Load(responseStream);

                /*
                 
                 * Bug!! Root element ("Upload") as diferent cases in error and in success responses
                 
                 * Must ask SAPO Photos Team about the latency at User creation
                 
                 * How can an application post photos for a user not being that user? Seems to be possible.
                 
                 * Return usefull information 
                 
                 */

                string resultCode =
                    xDocument
                        .Descendants("uploadPost")
                        .Select(e => e.Element("Result").Value)
                        .SingleOrDefault();
                return new CreateImageResult(res.image, resultCode);
            }
            throw new Exception("Request cannot be fulfilled");
        }


        /*
         * this operation has a bug. If it is the first one to be invoke since App start, it will crash with Communicationxception
         * http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/676f2908-48a7-413e-9340-caa22956363e 
         */
        internal async Task<bool> DeleteImageAsync(string imageUid)
        {
            Image img = new Image {uid = imageUid, idSpecified = true};

            //this._client.OpenAsync();
            //new System.Threading.ManualResetEvent(false).WaitOne(5000);

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client.ImageDeleteAsync(img).ContinueWith(t =>
                    {
                        if (t.Result.ok)
                        {
                            //this._client.CloseAsync().Wait();
                            return t.Result.ok;
                        }
                        return false;
                    });
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        internal async Task<Image> GetImageDetailsAsync(string imageUid)
        {
            Image img = new Image { uid = imageUid };

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client.ImageDetailsAsync(img).ContinueWith(t =>
                    {
                        this._client.CloseAsync().Wait();
                        if (t.Result.result.ok)
                            return t.Result.image;
                        //throw new Exception("Server returned error");
                        return null;
                    });
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        /* Information
         * Requires Image::uid
         * Can update:
         * * Image::title
         * * Image::active
         * * Image::password
         * * Image::m18
         * * Image::synopsis
         * * Image::tags
         * 
        */

        internal async Task<Image> EditImageDetailsAsync(Image img)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                        .ImageEditAsync(img)
                        .ContinueWith(t =>
                                        {
                                            if (t.Result.result.ok)
                                                return t.Result.image;
                                            throw new Exception("Server returned error"); 
                                        });
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");    
        }

        #endregion

        #endregion

        #region User Related

        //Tested providing username

        internal async Task<User> GetUserDetailsAsync(string username)
        {
            User user = new User
                            {
                                username = username,
                            };
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                        .UserDetailsAsync(user, new Format())
                        .ContinueWith(t =>
                                          {
                                              if(t.Result.result.ok)
                                                return t.Result.user;
                                              throw new Exception("Server returned error");
                                          });
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");      
        }

        #endregion

        #region Album Related

        internal async Task<Album> CreateAlbumAsync(Album album)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                                     .AlbumCreateAsync(album)
                                     .ContinueWith(t =>
                                                       {
                                                           if (t.Result.result.ok)
                                                               return t.Result.album;
                                                           throw new Exception("Server returned error");
                                                       });
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        //ImageAddToAlbumAsync requires not only the Album::id but also Album::idSpecified

        internal async Task<bool> AddImageToAlbums(string imageId,  int[] albunsIds)
        {
            Image img = new Image {uid = imageId, albums = new Album[albunsIds.Length]};

            for (int i = 0; i < img.albums.Length; i++)
            {
                img.albums[i] = new Album {id = albunsIds[i], idSpecified = true};
            }

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                                     .ImageAddToAlbumAsync(img, PhotosServiceClient.FotosSapoPtInterface)
                                     .ContinueWith(t => t.Result.result.ok);
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        internal async Task<bool> AddImageToAlbums(string imageId, Album[] albums)
        {
            Image img = new Image { uid = imageId, albums = albums };

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                                     .ImageAddToAlbumAsync(img, PhotosServiceClient.FotosSapoPtInterface)
                                     .ContinueWith(t => t.Result.result.ok);
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        internal async Task<IEnumerable<Album>> GetUserAlbumsList()
        {
            User user = new User();

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
                {
                    return await this._client
                        .AlbumGetListByUserAsync(user, 0, null, FotosSapoPtInterface)
                        .ContinueWith(t => t.Result.albums);
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();

                if (messageFault.HasDetail)
                {
                    string innerErrorXml;
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        innerErrorXml = xmlReader.ReadInnerXml();
                    }
                    if (innerErrorXml != null)
                        throw new Exception(innerErrorXml);
                    throw;
                }
            }
            throw new Exception("Request cannot be fulfilled");
        }

        internal async Task<IEnumerable<Image>> GetUserImageList()
        {
            using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
            {
                return await this._client
                    .ImageGetListByUserAsync(0, new User { username = "test8" })
                    .ContinueWith(t => t.Result.images);
            }
        }

        internal async Task<IEnumerable<string>> GetUserTags()
        {
            using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.AccessKey, _client))
            {
                return await this._client
                                 .UserGetTagsAsync(new User { username = "test8" }, null)
                                 .ContinueWith(t => t.Result.tags);
            }
        }

        #endregion

        #region Wrappers

        public IAsyncOperation<CreateImageResult> CreateImageOperationAsync(IStorageFile file, Image img)
        {
            return CreateImageAsync(file, img).AsAsyncOperation();
        }

        public IAsyncOperation<bool> DeleteImageOperationAsync(string imageUid)
        {
            return DeleteImageAsync(imageUid).AsAsyncOperation();
        }

        public IAsyncOperation<Image> GetImageDetailsOperationAsync(string imageUid)
        {
            return GetImageDetailsAsync(imageUid).AsAsyncOperation();
        }

        public IAsyncOperation<Image> EditImageDetailsOperationAsync(Image img)
        {
            return EditImageDetailsAsync(img).AsAsyncOperation();
        }

        public IAsyncOperation<User> GetUserDetailsOperationAsync(string username)
        {
            return GetUserDetailsAsync(username).AsAsyncOperation();
        }

        public IAsyncOperation<Album> CreateAlbumOperationAsync(Album album)
        {
            return CreateAlbumAsync(album).AsAsyncOperation();
        }

        public IAsyncOperation<bool> AddImageToAlbumsArrayIdsOperationAsync(string imageId, [ReadOnlyArray] int[] albunsIds)
        {
            return AddImageToAlbums(imageId, albunsIds).AsAsyncOperation();
        }

        public IAsyncOperation<bool> AddImageToAlbumsArrayOperationAsync(string imageId, [ReadOnlyArray] Album[] albums)
        {
            return AddImageToAlbums(imageId, albums).AsAsyncOperation();
        }

        public IAsyncOperation<IEnumerable<Album>> GetUserAlbumsListOperationAsync()
        {
            return GetUserAlbumsList().AsAsyncOperation();
        }

        public IAsyncOperation<IEnumerable<string>> GetUserTagsOperationAsync()
        {
            return GetUserTags().AsAsyncOperation();
        }

        public IAsyncOperation<IEnumerable<Image>> GetUserImageListOperationAsync()
        {
            return GetUserImageList().AsAsyncOperation();
        }

        #endregion
    }
}



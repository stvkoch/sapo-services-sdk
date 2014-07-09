using System.Collections.Generic;
using System.IO;
using Photos.PhotosReference;
using Photos.SapoServices.Utils;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using System.Linq;

namespace Photos.SapoServices
{
    public class PhotosServiceClient
    {
        private const string FotosSapoPtInterface = "fotos.sapo.pt";
        private const string HttpFotosSapoPtUploadpostHtmlUri = "http://fotos.sapo.pt/uploadPost.html";
        private readonly PhotosSoapSecureClient _client;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Accesskey { get; set; }

        public PhotosServiceClient(string username, string password, string accesskey)
        {
            this.Username = username;
            this.Password = password;
            this.Accesskey = accesskey;
            this._client = new PhotosSoapSecureClient();
        }


        #region Test Operations

        public IAsyncOperation<string> DummyEchoAsync(string echoStr)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<CreateImageResult> CreateImageAsync(IStorageFile file, Image img)
        {
            ImageCreateResult res = null;
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<bool> DeleteImageAsync(string imageUid)
        {
            Image img = new Image {uid = imageUid};

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
                {
                    return await this._client.ImageDeleteAsync(img).ContinueWith(t =>
                    {
                        if (t.Result.ok)
                            return t.Result.ok;
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

        public async Task<Image> GetImageDetailsAsync(string imageUid)
        {
            Image img = new Image { uid = imageUid };

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
                {
                    return await this._client.ImageDetailsAsync(img).ContinueWith(t =>
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
        public async Task<Image> EditImageDetailsAsync(Image img)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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
        public async Task<User> GetUserDetailsAsync(string username)
        {
            User user = new User
                            {
                                username = username,
                            };
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<Album> CreateAlbumAsync(Album album)
        {
            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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
        public async Task<bool> AddImageToAlbums(string imageId, int[] albunsIds)
        {
            Image img = new Image {uid = imageId, albums = new Album[albunsIds.Length]};

            for (int i = 0; i < img.albums.Length; i++)
            {
                img.albums[i] = new Album {id = albunsIds[i], idSpecified = true};
            }

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<bool> AddImageToAlbums(string imageId, Album[] albums)
        {
            Image img = new Image { uid = imageId, albums = albums };

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<Album[]> GetUserAlbumsList()
        {
            User user = new User()
            {
                username = "test8",
            };

            try
            {
                using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
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

        public async Task<Image[]> GetUserImageList()
        {
            using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
            {
                return await this._client
                    .ImageGetListByUserAsync(0, null)
                    .ContinueWith(t => t.Result.images);
            }
        }

        public async Task<string[]> GetUserTags()
        {
            using (EnsureCredentialsUseContext context = new EnsureCredentialsUseContext(
                    this.Username, this.Password, this.Accesskey, _client.InnerChannel))
            {
                return await this._client
                                 .UserGetTagsAsync(null, null)
                                 .ContinueWith(t => t.Result.tags);
            }
        }

        #endregion

    }
}

//string firstOrDefault = xDocument
//    .Descendants("uploadPost")
//    .Where(e => e.Name == "Result")
//    .Select(e => e.Value)
//    .FirstOrDefault();

//IEnumerable<XElement> xElements = xDocument.Descendants("UploadPost");

//foreach (var xElement in xElements)
//{
//    string s = xElement.Value;
//}


//byte[] content = result.ToArray();

//string s = Encoding.UTF8.GetString(content, 0, content.Length);

//string s1 = Encoding.Unicode.GetString(content, 0, content.Length);





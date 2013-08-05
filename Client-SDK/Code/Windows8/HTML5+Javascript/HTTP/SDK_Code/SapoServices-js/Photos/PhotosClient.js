(function () {
    WinJS.Namespace.define("Photos", {
        ServiceClient: WinJS.Class.define(
            function (username, password, accessKey) {

                if (!(username && password && accessKey))
                    throw SdkExceptions.Client.NonProvidedCredentialsException;
                this.username = username;
                this.password = password;
                this.accessKey = accessKey;
                this.baseUri = "https://services.sapo.pt/Photos/";
            },
            {
                asyncDummyEcho: function (echoStr) {
                    /// <summary>Do a dummy echo request to SAPO Photos Service.</summary>
                    /// <param name="echoStr" type="String">An object with the parameters to be included in request.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    if (!echoStr)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var dummyEchoAllowedParams = ["echoStr", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.echoStr = echoStr;

                    return Utils.doGetRequestHelper(this, params, dummyEchoAllowedParams, "DummyEcho");
                },

                asyncUserDetails: function () {
                    /// <summary>Do a request to SAPO Photos Service to obtain the authenticated user details.</summary>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    var userDetailsAllowedParams = ["json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, undefined, userDetailsAllowedParams, "UserDetails");
                },
                
                asyncImageCreate: function (file, image) {
                    /// <summary>Do a request to SAPO Photos Service to submit a photo to the service.</summary>
                    /// <param name="file" type="Object">The StorageFile of the photo to be submited.</param>
                    /// <param name="image" type="Object">An object with the image metadata. At least "title" and "tags" MUST be provided. A complete list of the properties of the image object can be found at the service documentation.</param>
                    /// <returns type="Object">A promisse Object with a string with the operation result code. A complete list of result codes cand be found in the service documentation (https://store.services.sapo.pt/en/Catalog/social/free-api-photos/technical-description#service-Photos-operation-ImageCreate)</returns>

                    if (!(file && image))
                        throw SdkExceptions.Client.InsuffientParametersException;
                    var imageCreateAllowedParams = ["json", "ESBUsername", "ESBPassword"];
                    var imageToBeSerialized = {};
                    imageToBeSerialized.image = image;

                    var imageStr = JSON.stringify(imageToBeSerialized);

                    var params = {};

                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.baseUri, params,
                            imageCreateAllowedParams, "ImageCreate"))
                        .absoluteCanonicalUri;

                    var headers = {};

                    headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                    return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: imageStr })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler)
                        .then(function (responseText) {
                            
                            var response = JSON.parse(responseText);

                            var imageCreateResponse = response.ImageCreateResponse.ImageCreateResult;

                            var uploadPhotoUri = new Windows.Foundation.Uri("http://fotos.sapo.pt/uploadPost.html");

                            var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader();

                            var parts = [];

                            var tokenPart =
                                new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("token");
                            tokenPart.setText(imageCreateResponse.token);
                            parts.push(tokenPart);

                            var photoPart =
                                new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("photo", file.name);
                            photoPart.setFile(file);
                            photoPart.setHeader("Content-Type", file.contentType);
                            parts.push(photoPart);

                            return uploader.createUploadAsync(uploadPhotoUri, parts, "form-data", "imgboundary")
                                .then(function (uploadOperation) {
                                    return uploadOperation.startAsync();
                                })
                                .then(function (uploadOperation) {
                                    var responseInformation = uploadOperation.getResponseInformation();

                                    var contentLenght = responseInformation.headers["Content-Length"];

                                    var inputStream = uploadOperation.getResultStreamAt(0);

                                    var reader = new Windows.Storage.Streams.DataReader(inputStream);

                                    return reader.loadAsync(contentLenght).then(function () {
                                        var resultText = reader.readString(contentLenght);

                                        var xmlDocument = new Windows.Data.Xml.Dom.XmlDocument();

                                        xmlDocument.loadXml(resultText);
                                        var resultTag = xmlDocument.getElementsByTagName("Result");

                                        return resultTag[0].innerText;
                                    });
                                });

                        }, function (e) { throw e; });
                },

                asyncImageDetails: function (uid) {
                    /// <summary>Do request to SAPO Photos Service to get the details of the photo with the given id.</summary>
                    /// <param name="uid" type="String">Photo uid</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!uid)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageDetailsAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.uid = uid;

                    return Utils.doGetRequestHelper(this, params, imageDetailsAllowedParams, "ImageDetails");
                },

                asyncImageGetListBySearch: function (params) {
                    /// <summary>Do request to SAPO Photos Service to search for photos by terms.</summary>
                    /// <param name="params" type="Object">An object with the parameters to be included in request. The "string" parameter holds the terms.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListBySearchAllowedParams =
                        ["string", "page", "interface", "datefrom", "dateto", "json", "ESBUsername", "ESBPassword"];
                    
                    //Convert dates to Strings
                    if (params.datefrom)
                        params.datefrom = Utils.dateToString(params.datefrom);
                    if (params.dateto)
                        params.dateto = Utils.dateToString(params.dateto);

                    return Utils.doGetRequestHelper(this, params, imageGetListBySearchAllowedParams, "ImageGetListBySearch");
                },

                asyncImageGetListByTags: function (params) {
                    /// <summary>Do request to SAPO Photos Service to search for photos by tags.</summary>
                    /// <param name="params" type="Object">An object with the parameters to be included in request. The "tag" parameter holds the tags.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListByTagsAllowedParams =
                        ["tag", "page", "orderby", "m18", "username", "interface", "json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, params, imageGetListByTagsAllowedParams, "ImageGetListByTags");
                },

                asyncImageDelete: function (uid) {
                    /// <summary>Do request to SAPO Photos Service to delete the photo with the given id.</summary>
                    /// <param name="uid" type="String">Photo uid</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!uid)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageDeleteAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.uid = uid;

                    return Utils.doGetRequestHelper(this, params, imageDeleteAllowedParams, "ImageDelete");
                },

                asyncImageGetListByUser: function (user) {
                    /// <summary>Do request to SAPO Photos Service to get the photos of a given user.</summary>
                    /// <param name="user" type="String">User's username</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!user)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListByUserAllowedParams = ["username", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.username = user;

                    return Utils.doGetRequestHelper(this, params, imageGetListByUserAllowedParams, "ImageGetListByUser");
                },

                asyncImageGetListByUserAlbum: function (albumid, params) {
                    /// <summary>Do request to SAPO Photos Service to get the photos of a given album of the authenticated user.</summary>
                    /// <param name="albumid" type="String">The album id.</param>
                    /// <param name="params" type="Object">User's username.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    if (!albumid)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListByUserAlbumAllowedParams = ["page", "orderby", "id", "username", "json",
                        "ESBUsername", "ESBPassword"];

                    if (!params)
                        params = {};
                    params.id = albumid;

                    return Utils.doGetRequestHelper(this, params,
                        imageGetListByUserAlbumAllowedParams, "ImageGetListByUserAlbum");
                },

                asyncImageAddToAlbum: function (imageuid, albumids) {
                    /// <summary>Do request to SAPO Photos Service to add a given photo to a list of albums of the authenticated user.</summary>
                    /// <param name="albumids" type="Array">An array with the album ids.</param>
                    /// <param name="imageuid" type="String">The id of the photo to be added.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    if (albumids == undefined || imageuid == undefined)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageAddToAlbumAllowedParams = ["imageuid", "albumid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.imageuid = imageuid;
                    params.albumid = albumids;

                    return Utils.doGetRequestHelper(this, params, imageAddToAlbumAllowedParams, "ImageAddToAlbum");
                },

                asyncAlbumCreate: function (album) {
                    /// <summary>Do a request to SAPO Photos Service to create an album in service.</summary>
                    /// <param name="album" type="Object">An object with the album metadata. At least "title" MUST be provided. A complete list of the properties of the album object can be found at the service documentation.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    var albumCreateAllowedParams = ["json", "ESBUsername", "ESBPassword"];
                    if (album) {
                        var albumToBeSerialized = {};
                        albumToBeSerialized.album = album;

                        var albumStr = JSON.stringify(albumToBeSerialized);

                        var params = {};
                        params.json = "true";
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        var uri =
                            Windows.Foundation.Uri(
                                Utils.buildUri(this.baseUri, params, albumCreateAllowedParams, "AlbumCreate"))
                            .absoluteCanonicalUri;

                        var headers = {};
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        headers["Content-Type"] = "application/json";
                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: albumStr })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },
                
                asyncAlbumGetListByUser: function (user) {
                    /// <summary>Do request to SAPO Photos Service to get the albums of a given user.</summary>
                    /// <param name="user" type="String">User's username</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    if (!user)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    
                    var albumGetListByUserAllowedParams = ["username", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.username = user;

                    return Utils.doGetRequestHelper(this, params, albumGetListByUserAllowedParams, "AlbumGetListByUser");
                }
            }
        )
    });

})();
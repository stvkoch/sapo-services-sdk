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
                asyncDummyEcho: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var dummyEchoAllowedParams = ["echoStr", "json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, params, dummyEchoAllowedParams, "DummyEcho");
                },

                asyncUserDetails: function () {
                    var userDetailsAllowedParams = ["json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, undefined, userDetailsAllowedParams, "UserDetails");
                },

                asyncImageCreate: function (file, image) {
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
                    if (!uid)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageDetailsAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.uid = uid;

                    return Utils.doGetRequestHelper(this, params, imageDetailsAllowedParams, "ImageDetails");
                },

                asyncImageGetListBySearch: function (params) {
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
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListByTagsAllowedParams =
                        ["tag", "page", "orderby", "m18", "username", "interface", "json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, params, imageGetListByTagsAllowedParams, "ImageGetListByTags");
                },

                asyncImageDelete: function (uid) {
                    if (!uid)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageDeleteAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.uid = uid;

                    return Utils.doGetRequestHelper(this, params, imageDeleteAllowedParams, "ImageDelete");
                },

                asyncImageGetListByUser: function (user) {
                    if (!user)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageGetListByUserAllowedParams = ["username", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.username = user;

                    return Utils.doGetRequestHelper(this, params, imageGetListByUserAllowedParams, "ImageGetListByUser");
                },

                asyncimageGetListByUserAlbum: function (albumid, params) {
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
                    if (albumids == undefined || imageuid == undefined)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var imageAddToAlbumAllowedParams = ["imageuid", "albumid", "json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.imageuid = imageuid;
                    params.albumid = albumids;

                    return Utils.doGetRequestHelper(this, params, imageAddToAlbumAllowedParams, "ImageAddToAlbum");
                },

                asyncAlbumCreate: function (album) {
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
                
                asyncAlbumGetListByUser: function(user) {
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
function PhotosServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var photosBaseUri = "http://services.sapo.pt/Photos/";

    var dummyEchoAllowedParams = ["echoStr", "json", "ESBUsername", "ESBPassword"];
    this.asyncDummyEcho = function(params) {
        if (params) {
            
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, dummyEchoAllowedParams, "DummyEcho"))
                .absoluteCanonicalUri;
            
            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var imageCreateAllowedParams = ["json", "ESBUsername", "ESBPassword"];
    this.asyncImageCreate = function (file, image) {

        var imageToBeSerialized = {};
        imageToBeSerialized.image = image;

        var imageStr = JSON.stringify(imageToBeSerialized);

        var params = { };

        params.json = "true";
        params.ESBUsername = username;
        params.ESBPassword = password;
        var uri =
            Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageCreateAllowedParams, "ImageCreate"))
            .absoluteCanonicalUri;

        var headers = {};

        headers["Content-Type"] = "application/json";
        headers["Authorization"] = "ESB AccessKey=" + accessKey;

        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: imageStr})
            .then(function (xhr) {
                if (xhr.status == 200 && xhr.responseText)
                    return xhr.responseText;
                return null;
            })
            .then(function (responseText) {
                if (responseText == null)
                    return null;
                var response = JSON.parse(responseText);

                var imageCreateResponse = response.ImageCreateResponse.ImageCreateResult;

                var uploadPhotoUri = new Windows.Foundation.Uri("http://fotos.sapo.pt/uploadPost.html");

                var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader();

                var parts = [];

                var tokenPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("token");
                tokenPart.setText(imageCreateResponse.token);
                parts.push(tokenPart);

                var photoPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("photo", file.name);
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

            });
    };

    var imageDetailsAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
    this.asyncImageDetails = function(uid) {
        if(uid) {
            var params = {};
            params.uid = uid;
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageDetailsAllowedParams, "ImageDetails"))
                .absoluteCanonicalUri;
            
            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var imageGetListBySearchAllowedParams =
        ["string", "page", "interface", "datefrom", "dateto", "json", "ESBUsername", "ESBPassword"];
    this.asyncImageGetListBySearch = function(params) {
        if (params) {
            
            //Convert dates to Strings
            if (params.datefrom)
                params.datefrom = dateToString(params.datefrom);
            if (params.dateto)
                params.dateto = dateToString(params.dateto);

            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageGetListBySearchAllowedParams, "ImageGetListBySearch"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var imageGetListByTagsAllowedParams =
        ["tag", "page", "orderby", "m18", "username", "interface", "json", "ESBUsername", "ESBPassword"];
    this.asyncImageGetListByTags = function(params) {
        if(params) {
            
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageGetListByTagsAllowedParams, "ImageGetListByTags"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });

        }
        throw "MUST specify parameters";
    };

    var imageDeleteAllowedParams = ["uid", "json", "ESBUsername", "ESBPassword"];
    this.asyncImageDelete = function(uid) {
        if (uid) {
            var params = { };
            params.uid = uid;
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageDeleteAllowedParams, "ImageDelete"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify uid parameter";
    };

    var imageGetListByUserAllowedParams = ["username", "json", "ESBUsername", "ESBPassword"];
    this.asyncImageGetListByUser = function(user) {
        if (user) {
            var params = {};
            params.username = user;
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, params, imageGetListByUserAllowedParams, "ImageGetListByUser"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify username parameter";
    };
    
    //ImageGetListByUserAlbum Not tested
    var imageGetListByUserAlbumAllowedParams = ["id", "username", "json", "ESBUsername", "ESBPassword"];
    this.asyncimageGetListByUserAlbum = function(albumid, params) {
        if (albumid) {

            if (!params)
                params = { };
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(
                    buildUri(photosBaseUri, params, imageGetListByUserAlbumAllowedParams, "ImageGetListByUserAlbum"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify album id parameter";
    };

    //ImageAddToAlbum Not tested
    var imageAddToAlbumAllowedParams = ["imageuid","id","json", "ESBUsername", "ESBPassword"];
    this.asyncImageAddToAlbum = function(imageuid, albumids) {
        if (albumids != undefined && imageuid != undefined) {

            var params = {};
            params.imageuid = imageuid;
            params.id = albumids;
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(
                    buildUri(photosBaseUri, params, imageAddToAlbumAllowedParams, "ImageAddToAlbum"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify album ids and image uid parameters";
    };

    //AlbumCreate Not tested
    var albumCreateAllowedParams = ["json", "ESBUsername", "ESBPassword"];
    this.asyncImageAddToAlbum = function (album) {
        if (album) {
            var albumToBeSerialized = { };
            albumToBeSerialized.album = album;

            var albumStr = JSON.stringify(albumToBeSerialized);

            var params = {};
            params.json = "true";
            params.ESBUsername = username;
            params.ESBPassword = password;
            var uri =
                Windows.Foundation.Uri(
                    buildUri(photosBaseUri, params, albumCreateAllowedParams, "AlbumCreate"))
                .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + accessKey;
            headers["Content-Type"] = "application/json";
            return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: albumStr })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify album object";
    };
}

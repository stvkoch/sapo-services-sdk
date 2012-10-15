(function () {
    "use strict";

    WinJS.Namespace.define("Videos", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.videosBaseUri = "https://services.sapo.pt/Videos/";
                }
            ,
            {
                asyncAddVideoPost: function (file, video) {
                    var addVideoPostAllowedParams = ["json", "ESBUsername", "ESBPassword"];

                    //Ensure server expected format
                    var videoToBeSerialized = {};
                    videoToBeSerialized.Video = video;
                    var videoStr = JSON.stringify(videoToBeSerialized);

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params, addVideoPostAllowedParams, "AddVideoPost"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                    return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: videoStr })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler)
                        .then(function (responseText) {
                            var response = JSON.parse(responseText);

                            var addVideoPostResponse = response.AddVideoPostResponse.AddVideoPostResult;

                            var uploadVideoUri = new Windows.Foundation.Uri("http://upload.videos.sapo.pt/upload_token.html");

                            var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader();

                            var parts = [];

                            var tokenPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("token");
                            tokenPart.setText(addVideoPostResponse.Token);
                            parts.push(tokenPart);

                            var videoPart =
                                new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("content_file", file.name);
                            videoPart.setFile(file);
                            videoPart.setHeader("Content-Type", file.contentType);
                            parts.push(videoPart);

                            return uploader.createUploadAsync(uploadVideoUri, parts, "form-data", "videoboundary");
                        })
                        .then(function (uploadOperation) {
                            return uploadOperation.startAsync();
                        })
                        .then(function (uploadOperation) {
                            var responseInformation = uploadOperation.getResponseInformation();

                            //responseInformation cannot get Content-Length
                            var contentLength = uploadOperation.progress.bytesReceived;

                            var inputStream = uploadOperation.getResultStreamAt(0);

                            var reader = new Windows.Storage.Streams.DataReader(inputStream);

                            return reader.loadAsync(contentLength).then(function () {
                                var resultText = reader.readString(contentLength);

                                var xmlDocument = new Windows.Data.Xml.Dom.XmlDocument();

                                xmlDocument.loadXml(resultText);
                                var resultTag = xmlDocument.getElementsByTagName("Ok");

                                return resultTag[0].localName;
                            });
                        }, function (e) { throw e; });
                },

                asyncDeleteVideo: function (randname) {
                    if (!randname)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    var deleteVideoAllowedParams = ["Randname", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    params.Randname = randname;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params, deleteVideoAllowedParams, "DeleteVideo"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    //headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },

                asyncCheckVideo: function (randname) {
                    if (!randname)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var checkVideoAllowedParams = ["VideoRandname", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    params.VideoRandname = randname;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params, checkVideoAllowedParams, "CheckVideo"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    //headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                       .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },

                asyncGetUserInfo: function () {
                    var getUserInfoAllowedParams = ["Email", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    params.Email = this.username;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params, getUserInfoAllowedParams, "GetUserInfo"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    //headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },

                asyncListUserVideos: function () {
                    var listUserVideosAllowedParams = ["json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params,
                            listUserVideosAllowedParams, "ListUserVideos"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },

                asyncGetHighlights: function () {
                    var allowedParams = ["json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params,
                            allowedParams, "JSON2/Highlights"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },

                asyncSearchVideos: function (search, user, page, limit, order) {
                    if (!(search && user))
                        throw SdkExceptions.Service.InsuffientParametersException;

                    var allowedParams = ["search", "page", "limit", "order", "user",
                        "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    params.search = search;
                    params.user = user;
                    params.page = page;
                    params.limit = limit;
                    params.order = order;
                    
                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params,
                            allowedParams, "JSON2/Query"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                },
            }
        )
    });

})();


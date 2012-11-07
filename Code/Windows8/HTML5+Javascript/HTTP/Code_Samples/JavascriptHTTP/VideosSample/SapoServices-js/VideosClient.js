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
                    this.baseUri = "https://services.sapo.pt/Videos/";
                }
            ,
            {
                asyncAddVideoPost: function (file, video) {
                    /// <summary>Do a request to SAPO Videos Service to submit a video to the service.</summary>
                    /// <param name="file" type="Object">The StorageFile of the video to be submited.</param>
                    /// <param name="video" type="Object">An object with the video metadata. At least "Title", "Tags" and "Synopse" MUST be provided. A complete list of the properties of the video object can be found at the service documentation.</param>
                    /// <returns type="String">A string with "Ok" if video is successfully submited.</returns>
                    if (!(file && video))
                        throw SdkExceptions.Client.InsuffientParametersException;
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
                        Windows.Foundation.Uri(Utils.buildUri(this.baseUri, params, addVideoPostAllowedParams, "AddVideoPost"))
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
                    /// <summary>Do a request to SAPO Videos Service to delete the given video in the service.</summary>
                    /// <param name="file" type="Object">The StorageFile of the video to be submited.</param>
                    /// <param name="video" type="Object">An object with the video metadata. At least "Title", "Tags" and "Synopse" MUST be provided. A complete list of the properties of the video object can be found at the service documentation.</param>
                    
                    if (!randname)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    var deleteVideoAllowedParams = ["Randname", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.Randname = randname;

                    return Utils.doGetRequestHelper(this, params, deleteVideoAllowedParams, "DeleteVideo");
                },

                asyncCheckVideo: function (randname) {
                    /// <summary>Do a request to SAPO Videos Service to get the details of a given video of the service.</summary>
                    /// <param name="randname" type="String">The video's randname.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!randname)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var checkVideoAllowedParams = ["VideoRandname", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.VideoRandname = randname;

                    return Utils.doGetRequestHelper(this, params, checkVideoAllowedParams, "CheckVideo");
                },

                asyncGetUserInfo: function () {
                    /// <summary>Do a request to SAPO Videos Service to get the details of the authenticated user.</summary>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    var getUserInfoAllowedParams = ["Email", "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.Email = this.username;

                    return Utils.doGetRequestHelper(this, params, getUserInfoAllowedParams, "GetUserInfo");
                },

                asyncListUserVideos: function () {
                    /// <summary>Do a request to SAPO Videos Service to get the video's list of the authenticated user.</summary>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    var listUserVideosAllowedParams = ["json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, undefined, listUserVideosAllowedParams, "ListUserVideos");
                },

                asyncGetHighlights: function () {
                    /// <summary>Do a request to SAPO Videos Service to get the list of highlighted videos.</summary>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    var allowedParams = ["json", "ESBUsername", "ESBPassword"];

                    return Utils.doGetRequestHelper(this, undefined, allowedParams, "JSON2/Highlights");
                },

                asyncSearchVideos: function (search, user, page, limit, order) {
                    /// <summary>Do a request to SAPO Videos Service to get the details of a given video of the service.</summary>
                    /// <param name="search" type="String">The search terms.</param>
                    /// <param name="user" type="String">You can provide the SAPO Videos username if you want to search videos of a specific user.</param>
                    /// <param name="page" type="String">The page number.</param>
                    /// <param name="limit" type="String">Number of videos per page.</param>
                    /// <param name="order" type="String">Ordering criteria of the returned videos.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (search == undefined && user == undefined)
                        throw SdkExceptions.Service.InsuffientParametersException;

                    var allowedParams = ["search", "page", "limit", "order", "user",
                        "json", "ESBUsername", "ESBPassword"];

                    var params = {};
                    params.search = search;
                    params.user = user;
                    params.page = page;
                    params.limit = limit;
                    params.order = order;

                    return Utils.doGetRequestHelper(this, params, allowedParams, "JSON2/Query");
                },
            }
        )
    });

})();


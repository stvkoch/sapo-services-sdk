(function () {
    "use strict";

    WinJS.Namespace.define("Videos", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw "MUST provide username, password and accessKey";
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
                        .then(function (xhr) {
                            if (xhr.status == 200 && xhr.responseText)
                                return xhr.responseText;
                            return null;
                        })
                        .then(function (responseText) {
                            if (responseText == null)
                                return null;
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
                        });
                },
                
                asyncDeleteVideo: function (randname) {
                    var deleteVideoAllowedParams = ["json", "ESBUsername", "ESBPassword"];
                    
                    var params = {};
                    params.json = "true";
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.videosBaseUri, params, deleteVideoAllowedParams, "AddVideoPost"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    //headers["Content-Type"] = "application/json";
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                },
                
                asyncCheckVideo: function () {
                    
                },
                
                asynGetUserInfo: function () {
                    
                },
                
                asyncListUserVideos: function () {
                    
                }

            }
        )
    });

})();


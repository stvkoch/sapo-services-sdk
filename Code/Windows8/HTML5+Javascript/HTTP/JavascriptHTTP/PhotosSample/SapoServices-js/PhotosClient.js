function PhotosServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var credentials = {};
    credentials.username = username;
    credentials.password = password;
    credentials.accessKey = accessKey;

    var photosBaseUri = "https://services.sapo.pt/Photos/";

    var dummyEchoAllowedParams = ["echoStr", "json"];
    this.asyncDummyEcho = function(params) {
        if (params) {
            
            params.json = "true";

            var uri =
                Windows.Foundation.Uri(buildUri(photosBaseUri, credentials, params, dummyEchoAllowedParams, "DummyEcho"))
                .absoluteCanonicalUri;
            
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };


    this.asyncImageCreate = function(file) {
        var uploadPhotoUri = new Windows.Foundation.Uri("http://fotos.sapo.pt/uploadPost.html");

        var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader();

        var parts = [];

        var tokenPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("token");
        tokenPart.setText("iamatoken");
        parts.push(tokenPart);

        var photoPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("photo", file.name);
        photoPart.setFile(file);
        photoPart.setHeader("Content-Type", file.contentType);
        parts.push(photoPart);

        uploader.createUploadAsync(uploadPhotoUri, parts, "form-data", "imgboundary")
            .then(function (uploadOperation) {

                uploadOperation.startAsync();

        });

        //var obj = {};

        //obj.image = { id: "a", xpto: "bb"};

        //WinJS.xhr({ type: "POST", url: "http//:www.", data: JSON.stringify(obj)})
        //        .then(function (xhr) {
        //            if (xhr.status == 200 && xhr.responseText)
        //                return xhr.responseText;
        //            return null;
        //        });
    };

}
function PhotosServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var baseUri = "https://services.sapo.pt/Photos/";

    this.asyncCreateImage = function() {

        var obj = {};

        obj.image = { id: "a", xpto: "bb"};

        WinJS.xhr({ type: "POST", url: "http//:www.", data: JSON.stringify(obj)})
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
    };

}
(function () {
    "use strict";

    WinJS.Namespace.define("PunyURL", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw "MUST provide username, password and accessKey";
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.punyUrlBaseUri = "https://services.sapo.pt/PunyURL/";
                }
            ,
            {
                asyncGetCompressedURLByURL: function (url) {
                    var allowedParams = ["url", "ESBUsername", "ESBPassword"];

                    if (url != null) {
                        var params = {};
                        params.url = url;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.punyUrlBaseUri, params,
                                allowedParams, "GetCompressedURLByURLJSON"))
                            .absoluteCanonicalUri;

                        var headers = {};
                        //headers["Content-Type"] = "application/json";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                            .then(function (xhr) {
                                if (xhr.status == 200 && xhr.responseText)
                                    return xhr.responseText;
                                return "ERROR";
                            }, function (xhr) {
                                return "ERROR";
                            });
                    }

                },

                asyncGetURLByCompressedURL: function (url) {
                    var allowedParams = ["json", "url", "ESBUsername", "ESBPassword"];

                    if (url != null) {
                        
                        var params = {};
                        params.url = url;
                        params.json = true;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.punyUrlBaseUri, params,
                                allowedParams, "GetURLByCompressedURL"))
                            .absoluteCanonicalUri;

                        var headers = {};
                        //headers["Content-Type"] = "application/json";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                            .then(function (xhr) {
                                if (xhr.status == 200 && xhr.responseText)
                                    return xhr.responseText;
                                return "ERROR";
                            }, function (xhr) {
                                return "ERROR";
                            });
                    }
                    throw "MUST specify parameters."
                }
            }
        )
    });

})();


(function () {
    "use strict";

    WinJS.Namespace.define("PunyURL", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.punyUrlBaseUri = "https://services.sapo.pt/PunyURL/";
                }
            ,
            {
                asyncGetCompressedURLByURL: function (url) {
                    var allowedParams = ["url", "ESBUsername", "ESBPassword", "json"];

                    if (url != null) {
                        var params = {};
                        params.url = url;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.json = "true";

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.punyUrlBaseUri, params,
                                allowedParams, "GetCompressedURLByURLJSON"))
                            .absoluteCanonicalUri;

                        var headers = {};
                        //headers["Content-Type"] = "application/json";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncGetURLByCompressedURL: function (url) {
                    var allowedParams = ["url", "ESBUsername", "ESBPassword", "json"];

                    if (url != null) {
                        
                        var params = {};
                        params.url = url;
                        params.json = "true";
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
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


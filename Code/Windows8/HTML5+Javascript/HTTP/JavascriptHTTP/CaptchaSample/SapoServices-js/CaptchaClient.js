(function () {
    "use strict";

    WinJS.Namespace.define("Captcha", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.captchaBaseUri = "https://services.sapo.pt/Captcha/";
                }
            ,
            {
                asyncGet: function (params) {
                    
                    var allowedParams = ["r","ttl", "length", "mode", "ESBUsername", "ESBPassword"];

                    if (!params) 
                        params = {};
                    params.ESBUsername = this.username;
                    params.ESBPassword = this.password;
                    //send a value to force client to do the request instead of use the cached response
                    params.r = String(Date.now());

                    var uri =
                        Windows.Foundation.Uri(Utils.buildUri(this.captchaBaseUri, params,
                            allowedParams, "Get"))
                        .absoluteCanonicalUri;

                    var headers = {};
                    headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                    return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        .then(function (xhr) {
                            if (xhr.status == 200 && xhr.responseText) {
                                var parser = new DOMParser();
                                var xmlDoc = parser.parseFromString(xhr.responseText, "text/xml");

                                var result = {};
                                result.id = xmlDoc.getElementsByTagName("id").item(0).textContent;
                                result.code = xmlDoc.getElementsByTagName("code").item(0).textContent;
                                return result;
                            }
                            return "ERROR";
                        }, function (xhr) {
                            return "ERROR";
                        });

                },

                //TODO
                asyncPlay: function (id) {
                    var allowedParams = ["id", "ESBUsername", "ESBPassword"];

                    if (id) {
                        var params = {};
                        params.id = id;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.captchaBaseUri, params,
                                allowedParams, "Play"))
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
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                //TODO
                asyncShow: function (id, params) {
                    var allowedParams =
                        ["id", "font", "textcolor", "size", "background", "url", "ESBUsername", "ESBPassword"];

                    if (id) {
                        if("params")
                            params = {};
                        params.id = id;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.captchaBaseUri, params,
                                allowedParams, "Show"))
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
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                buildShowURI: function (id, params) {
                    var allowedParams =
                       ["id", "font", "textcolor", "size", "background", "url", "ESBUsername", "ESBPassword",
                           "ESBAccessKey"];

                    if (id) {
                        if (!params)
                            params = {};
                        params.id = id;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.ESBAccessKey = this.accessKey;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.captchaBaseUri, params,
                                allowedParams, "Show"));
                        return uri;
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                buildPlayURI: function (id) {
                    var allowedParams = ["id", "ESBUsername", "ESBPassword", "ESBAccessKey"];

                    if (id) {
                        var params = {};
                        params.id = id;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.ESBAccessKey = this.accessKey;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.captchaBaseUri, params,
                                allowedParams, "Play"));
                        return uri;
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


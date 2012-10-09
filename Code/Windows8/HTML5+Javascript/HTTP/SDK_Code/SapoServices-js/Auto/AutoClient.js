(function () {
    "use strict";

    WinJS.Namespace.define("Auto", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw "MUST provide username, password and accessKey";
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.autoBaseUri = "https://services.sapo.pt/Auto/";
                }
            ,
            {
                asyncSearchByTerms: function (params) {
                    var allowedParams = ["q", "start", "rows", "sort", "ESBUsername", "ESBPassword"];

                    if (params != null && allowedParams[0] in params && params[allowedParams[0]]) {

                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.autoBaseUri, params,
                                allowedParams, "JSON"))
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

                asyncSearchByBrandModelPrice: function (params) {
                    var allowedParams = ["q", "start", "rows", "sort", "ESBUsername", "ESBPassword"];

                    function tryExtractBrandModelPrice(params) {
                        var qParams = ["Brand","Model","Price"];
                        var sb = new Utils.StringBuilder();
                        
                        for (var i = 0; i < qParams.length; ++i) {
                            if ((qParams[i] in params) && params[qParams[i]]) {
                                if (i != 0)
                                    sb.append("+");
                                sb.append(qParams[i]);
                                sb.append(":");
                                sb.append(params[qParams[i]]);
                            }
                        }
                        if (sb.content.length > 0)
                            return sb.toString();
                        return null;
                    }

                    if (params != null) {
                        var res = tryExtractBrandModelPrice(params);

                        if (!res)
                            throw "At least one of Brand, Model or Price MUST be specified."
                        params.q = res;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.autoBaseUri, params,
                                allowedParams, "JSON"))
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


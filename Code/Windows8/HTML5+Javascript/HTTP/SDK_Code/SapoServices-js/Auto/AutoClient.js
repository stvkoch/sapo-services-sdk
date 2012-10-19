(function () {
    "use strict";

    WinJS.Namespace.define("Auto", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.baseUri = "https://services.sapo.pt/Auto/";
                }
            ,
            {
                asyncSearchByTerms: function (params) {
                    var allowedParams = ["q", "start", "rows", "sort", "ESBUsername", "ESBPassword", "json"];

                    if (params != null && allowedParams[0] in params && params[allowedParams[0]]) {

                        return Utils.doGetRequestHelper(this, params, allowedParams, "JSON");

                        //params.ESBUsername = this.username;
                        //params.ESBPassword = this.password;
                        //params.json = "true";

                        //var uri =
                        //    Windows.Foundation.Uri(Utils.buildUri(this.baseUri, params,
                        //        allowedParams, "JSON"))
                        //    .absoluteCanonicalUri;

                        //var headers = {};
                        ////headers["Content-Type"] = "application/json";
                        //headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        //return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        //    .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncSearchByBrandModelPrice: function (params) {
                    var allowedParams = ["q", "start", "rows", "sort", "ESBUsername", "ESBPassword", "json"];

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
                            throw SdkExceptions.Client.InsuffientParametersException;
                        params.q = res;

                        return Utils.doGetRequestHelper(this, params, allowedParams, "JSON");

                        //params.ESBUsername = this.username;
                        //params.ESBPassword = this.password;
                        //params.json = "true";

                        //var uri =
                        //    Windows.Foundation.Uri(Utils.buildUri(this.baseUri, params,
                        //        allowedParams, "JSON"))
                        //    .absoluteCanonicalUri;

                        //var headers = {};
                        ////headers["Content-Type"] = "application/json";
                        //headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        //return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                        //    .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


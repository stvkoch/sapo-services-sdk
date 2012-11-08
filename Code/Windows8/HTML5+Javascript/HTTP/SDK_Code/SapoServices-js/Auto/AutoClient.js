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
                    /// <summary>Do a request to SAPO Auto Service to search auto vehicles by terms.</summary>
                    /// <param name="params" type="Object">An object with the parameters to be included in request.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    //Allowed parameters for this request
                    var allowedParams = ["q", "start", "rows", "sort", "ESBUsername", "ESBPassword", "json"];

                    if (params != null && allowedParams[0] in params && params[allowedParams[0]]) {

                        return Utils.doGetRequestHelper(this, params, allowedParams, "JSON");
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncSearchByBrandModelPrice: function (params) {
                    /// <summary>Do a request to SAPO Auto Service to search auto vehicles by brand, model and price.</summary>
                    /// <param name="params" type="Object">An object with the parameters to be included in request.</param>
                    /// <returns type="String">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    //Allowed parameters for this request
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
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


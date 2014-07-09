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
                    this.baseUri = "https://services.sapo.pt/PunyURL/";
                }
            ,
            {
                asyncGetCompressedURLByURL: function (url) {
                    /// <summary>Do a request to SAPO PunyURL Service to obtain a compressed URI to the given URI.</summary>
                    /// <param name="url" type="String">The URI to compress.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!url)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var allowedParams = ["url", "ESBUsername", "ESBPassword", "json"];

                    var params = {};
                    params.url = url;

                    return Utils.doGetRequestHelper(this, params, allowedParams, "GetCompressedURLByURLJSON");
                },

                asyncGetURLByCompressedURL: function (url) {
                    /// <summary>Do a request to SAPO PunyURL Service to obtain the URI that corresponds to the given compressed URI.</summary>
                    /// <param name="url" type="String">The compressed URI.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    if (!url)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    
                    var allowedParams = ["url", "ESBUsername", "ESBPassword", "json"];

                    var params = {};
                    params.url = url;
                    
                    return Utils.doGetRequestHelper(this, params, allowedParams, "GetURLByCompressedURL");
                }
            }
        )
    });

})();


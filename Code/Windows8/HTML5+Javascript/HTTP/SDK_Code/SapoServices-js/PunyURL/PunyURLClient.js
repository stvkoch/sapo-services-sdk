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
                    if (!url)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var allowedParams = ["url", "ESBUsername", "ESBPassword", "json"];

                    var params = {};
                    params.url = url;

                    return Utils.doGetRequestHelper(this, params, allowedParams, "GetCompressedURLByURLJSON");
                },

                asyncGetURLByCompressedURL: function (url) {
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


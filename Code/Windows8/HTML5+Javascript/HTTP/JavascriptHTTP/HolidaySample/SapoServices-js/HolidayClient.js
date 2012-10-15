(function () {
    "use strict";

    WinJS.Namespace.define("Holiday", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.holidayBaseUri = "https://services.sapo.pt/Holiday/";
                }
            ,
            {
                asyncGetAllHolidays: function (year) {
                    return this.doRequestHelper(year, "GetAllHolidays");
                },

                asyncGetCarnival: function (year) {
                    return this.doRequestHelper(year, "GetCarnival");
                },

                asyncGetCorpusChristi: function (year) {
                    return this.doRequestHelper(year, "GetCorpusChristi");
                },

                asyncGetEaster: function (year) {
                    return this.doRequestHelper(year, "GetEaster");
                },

                asyncGetGoodFriday: function (year) {
                    return this.doRequestHelper(year, "GetGoodFriday");
                },

                asyncGetLocalHolidays: function (year) {
                    return this.doRequestHelper(year, "GetLocalHolidays");
                },

                asyncGetNationalHolidays: function (year) {
                    return this.doRequestHelper(year, "GetNationalHolidays");
                },

                asyncGetRegionalHolidays: function (year) {
                    return this.doRequestHelper(year, "GetRegionalHolidays");
                },

                /*Helper method*/
                doRequestHelper: function (year, operation) {
                    var allowedParams = ["jsonText", "year", "ESBUsername", "ESBPassword", "json"];

                    if (year) {
                        var params = {};
                        params.year = year;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.jsonText = "false";
                        params.jsonText = "true";

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.holidayBaseUri, params,
                                allowedParams, operation))
                            .absoluteCanonicalUri;

                        var headers = {};
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


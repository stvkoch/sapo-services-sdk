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
                    this.baseUri = "https://services.sapo.pt/Holiday/";
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
                    if (!year)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    
                    var allowedParams = ["jsonText", "year", "ESBUsername", "ESBPassword", "json"];
                    
                    var params = {};
                    params.year = year;
                    params.jsonText = "false";

                    return Utils.doGetRequestHelper(this, params, allowedParams, operation);
                }
            }
        )
    });

})();


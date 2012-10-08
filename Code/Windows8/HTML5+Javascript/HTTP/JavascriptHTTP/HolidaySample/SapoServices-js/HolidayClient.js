(function () {
    "use strict";

    WinJS.Namespace.define("Holiday", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw "MUST provide username, password and accessKey";
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
                    var allowedParams = ["jsonText", "year", "ESBUsername", "ESBPassword"];

                    if (year) {
                        var params = {};
                        params.year = year;
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.jsonText = "false";

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.holidayBaseUri, params,
                                allowedParams, operation))
                            .absoluteCanonicalUri;

                        var headers = {};
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
                    throw "MUST specify year";
                }
            }
        )
    });

})();


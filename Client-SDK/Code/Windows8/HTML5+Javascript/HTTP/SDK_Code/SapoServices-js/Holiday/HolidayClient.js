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
                    /// <summary>Do a request to SAPO Holiday Service to obtain all holidays.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetAllHolidays");
                },

                asyncGetCarnival: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain Carnival holiday.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetCarnival");
                },

                asyncGetCorpusChristi: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain Corpus Christi holiday.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetCorpusChristi");
                },

                asyncGetEaster: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain Easter holiday.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetEaster");
                },

                asyncGetGoodFriday: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain Good Friday holiday.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetGoodFriday");
                },

                asyncGetLocalHolidays: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain all local holidays.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetLocalHolidays");
                },

                asyncGetNationalHolidays: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain all national holidays.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    return this.doRequestHelper(year, "GetNationalHolidays");
                },

                asyncGetRegionalHolidays: function (year) {
                    /// <summary>Do a request to SAPO Holiday Service to obtain all regional holidays.</summary>
                    /// <param name="year" type="String">The year in yyyy format.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
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


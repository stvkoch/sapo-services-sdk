(function () {
    "use strict";

    WinJS.Namespace.define("Verbetes", {
        ServiceClient: WinJS.Class.define(
            function (username, password, accessKey) {

                if (!(username && password && accessKey))
                    throw SdkExceptions.Client.NonProvidedCredentialsException;
                this.username = username;
                this.password = password;
                this.accessKey = accessKey;
                this.baseUri = "https://services.sapo.pt/InformationRetrieval/Verbetes/";
            },
            {
                asyncWhoIs: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var whoIsAllowedParams = ["name", "name_like", "job", "job_like", "date", "margin",
                        "min", "format", "context", "ESBAccessKey", "ESBUsername", "ESBPassword", "json"];

                    //transform date to string with the correct format
                    if (params.date)
                        params.date = Utils.dateToString(params.date);

                    return Utils.doGetRequestHelper(this, params, whoIsAllowedParams, "WhoIs");
                },

                asyncGetPersonalities: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    
                    var getPersonalitiesAllowedParams = ["min", "format", "ESBAccessKey",
                        "ESBUsername", "ESBPassword", "json"];

                    return Utils.doGetRequestHelper(this, params, getPersonalitiesAllowedParams, "GetPersonalities");
                },

                asyncGetErgos: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;
                    
                    var getErgosAllowedParams = ["min", "format", "ESBAccessKey", "ESBUsername", "ESBPassword", "json"];

                    return Utils.doGetRequestHelper(this, params, getErgosAllowedParams, "GetErgos");
                },

                asyncGetEgoNet: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var getEgoNetAllowedParams = ["depth", "minFrequencyEdges", "name", "beginDate", "endDate",
                        "ESBAccessKey", "ESBUsername", "ESBPassword", "json"];
                    
                    //Convert dates to Strings
                    if (params.beginDate)
                        params.beginDate = Utils.dateToString(params.beginDate);
                    if (params.endDate)
                        params.endDate = Utils.dateToString(params.endDate);

                    return Utils.doGetRequestHelper(this, params, getEgoNetAllowedParams, "GetEgoNet");
                },

                asyncGetCoOccurrencesTrends: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var getCoOccurrencesTrendsAllowedParams = ["name1", "name2", "begin_date", "end_date", "format",
                        "ESBAccessKey", "ESBUsername", "ESBPassword", "json"];
                    
                    //Convert dates to Strings
                    if (params.begin_date)
                        params.begin_date = Utils.dateToString(params.begin_date);
                    if (params.end_date)
                        params.end_date = Utils.dateToString(params.end_date);

                    return Utils.doGetRequestHelper(this, params, getCoOccurrencesTrendsAllowedParams, "GetCoOccurrencesTrends");
                },

                asyncGetCoOccurrences: function (params) {
                    if (!params)
                        throw SdkExceptions.Client.InsuffientParametersException;

                    var getCoOcurrencesAllowedParams = ["name", "begin_date", "end_date", "format",
                        "ESBAccessKey", "ESBUsername", "ESBPassword", "json"];
                    
                    //Convert dates to Strings
                    if (params.begin_date)
                        params.begin_date = Utils.dateToString(params.begin_date);
                    if (params.end_date)
                        params.end_date = Utils.dateToString(params.end_date);

                    return Utils.doGetRequestHelper(this, params, getCoOcurrencesAllowedParams, "GetCoOccurrences");
                }
            }
        )
    });
})();
// !Bug at xhr.getAllResponseHeaders()

//var contentLength = xhr.getAllResponseHeaders();
//if (xhr.status == 200 && contentLength > 0)
//    return xhr.responseText;

//var res = eval("(" + xhr.responseText + ")");

//var res1 = JSON.parse(xhr.responseText);
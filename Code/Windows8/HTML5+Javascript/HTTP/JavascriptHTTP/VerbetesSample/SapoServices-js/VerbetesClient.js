function VerbetesServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var credentials = {};
    credentials.username = username;
    credentials.password = password;
    credentials.accessKey = accessKey;

    var verbetesBaseUri = "http://services.sapo.pt/InformationRetrieval/Verbetes/";

    var whoIsAllowedParams = ["name", "name_like", "job", "job_like", "date", "margin", "min", "format", "context"];
    this.asyncWhoIs = function (params) {
        if (params) {
            //transform date to string with the correct format
            if (params.date)
                params.date = dateToString(params.date);

            var uri = new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, whoIsAllowedParams, "WhoIs"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }

        throw "MUST specify parameters";
    };

    var getPersonalitiesAllowedParams = ["min", "format"];
    this.asyncGetPersonalities = function (params) {
        if(params) {
            var uri = new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, getPersonalitiesAllowedParams, "GetPersonalities"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function(xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var getErgosAllowedParams = ["min", "format"];
    this.asyncGetErgos = function (params) {
        if(params) {
            var uri = new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, getErgosAllowedParams, "GetErgos"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    //response is always in json format
    var getEgoNetAllowedParams = ["depth", "minFrequencyEdges", "name", "beginDate", "endDate"];
    this.asyncGetEgoNet = function (params) {
        if (params) {
            //Convert dates to Strings
            if (params.beginDate)
                params.beginDate = dateToString(params.beginDate);
            if (params.endDate)
                params.endDate = dateToString(params.endDate);

            var uri = new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, getEgoNetAllowedParams, "GetEgoNet"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var getCoOccurrencesTrendsAllowedParams = ["name1", "name2", "begin_date", "end_date", "format"];
    this.asyncGetCoOccurrencesTrends = function (params) {
        if (params) {
            //Convert dates to Strings
            if (params.begin_date)
                params.begin_date = dateToString(params.begin_date);
            if (params.end_date)
                params.end_date = dateToString(params.end_date);
            var uri =
                new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, getCoOccurrencesTrendsAllowedParams, "GetCoOccurrencesTrends"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };

    var getCoOcurrencesAllowedParams = ["name", "begin_date", "end_date", "format"];
    this.asyncGetCoOccurrences = function (params) {
        if (params) {
            //Convert dates to Strings
            if (params.begin_date)
                params.begin_date = dateToString(params.begin_date);
            if (params.end_date)
                params.end_date = dateToString(params.end_date);

            var uri = new Windows.Foundation
                    .Uri(buildUri(verbetesBaseUri, credentials, params, getCoOcurrencesAllowedParams, "GetCoOccurrences"));
            uri = uri.absoluteCanonicalUri;
            return WinJS.xhr({ type: "GET", url: uri })
                .then(function (xhr) {
                    if (xhr.status == 200 && xhr.responseText)
                        return xhr.responseText;
                    return null;
                });
        }
        throw "MUST specify parameters";
    };
}

// !Bug at xhr.getAllResponseHeaders()

//var contentLength = xhr.getAllResponseHeaders();
//if (xhr.status == 200 && contentLength > 0)
//    return xhr.responseText;

//var res = eval("(" + xhr.responseText + ")");

//var res1 = JSON.parse(xhr.responseText);
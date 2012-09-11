function VerbetesServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var baseUri = "http://services.sapo.pt/InformationRetrieval/Verbetes/";

    function buildUri(params, allowedParams, operation) {

        var sb = new StringBuilder();
        sb.append(baseUri);
        sb.append(operation);
        sb.append("?ESBUserName=");
        sb.append(username);
        sb.append("&ESBPassword=");
        sb.append(password);
        sb.append("&ESBAccessKey=");
        sb.append(accessKey);

        for (var i = 0; i < allowedParams.length; ++i) {
            if ((allowedParams[i] in params) && params[allowedParams[i]]) {
                sb.append("&");
                sb.append(allowedParams[i]);
                sb.append("=");
                sb.append(params[allowedParams[i]]);
            }
        }

        //if (params.name) {
        //    sb.append("&name=");
        //    sb.append(params.name);
        //    //uriWithAutentication += "&name=" + params.name;
        //}
        //if (params.nameLike) {
        //    sb.append("&name_like=");
        //    sb.append(params.nameLike);
        //    //uriWithAutentication += "&name_like=" + params.nameLike;
        //}
        //if (params.job) {
        //    sb.append("&job=");
        //    sb.append(params.job);
        //    //uriWithAutentication += "&job=" + params.job;
        //}
        //if (params.jobLike) {
        //    sb.append("&job_like=");
        //    sb.append(params.jobLike);
        //    //uriWithAutentication += "&job_like=" + params.jobLike;
        //}
        //if (params.margin) {
        //    sb.append("&margin=");
        //    sb.append(params.margin);
        //}
        //if (params.min) {
        //    sb.append("&min=");
        //    sb.append(params.min);
        //}
        //if (params.format) {
        //    sb.append("&format=");
        //    sb.append(params.format);
        //}
        //if (params.date) {
        //    sb.append("&date=");
        //    sb.append(params.date.getFullYear().toString());
        //    sb.append("-");
        //    var month = Number(params.date.getMonth().toString());
        //    month += 1;
        //    sb.append(String(month));
        //    sb.append("-");
        //    sb.append(params.date.getDate().toString());
        //}

        return sb.toString();
    }

    function dateToString(date) {
        var sb = new StringBuilder();
        sb.append(date.getFullYear().toString());
        sb.append("-");
        var month = Number(date.getMonth().toString());
        month += 1;
        if (month < 10)
            sb.append("0");
        sb.append(String(month));
        sb.append("-");
        var monthDay = Number(date.getDate().toString());
        if (monthDay < 10)
            sb.append("0");
        sb.append(String(monthDay));
        return sb.toString();
    }

    var whoIsAllowedParams = ["name", "name_like", "job", "job_like", "date", "margin", "min", "format", "context"];
    this.asyncWhoIs = function (params) {
        if (params) {
            //transform date to string with the correct format
            if (params.date)
                params.date = dateToString(params.date);

            var uri = new Windows.Foundation
                    .Uri(buildUri(params, whoIsAllowedParams, "WhoIs"));
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
                    .Uri(buildUri(params, getPersonalitiesAllowedParams, "GetPersonalities"));
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
                    .Uri(buildUri(params, getErgosAllowedParams, "GetErgos"));
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
                    .Uri(buildUri(params, getEgoNetAllowedParams, "GetEgoNet"));
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
                    .Uri(buildUri(params, getCoOccurrencesTrendsAllowedParams, "GetCoOccurrencesTrends"));
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
                    .Uri(buildUri(params, getCoOcurrencesAllowedParams, "GetCoOccurrences"));
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
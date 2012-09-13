
/*
    Common Utils 
*/


/* StringBuilder */

function StringBuilder() {
    this.content = [];

    this.append = function (string) {
        if (string && string.length > 0)
            this.content.push(string);
    };

    this.toString = function () {
        return this.content.join("");
    };

    this.clear = function () {
        this.content = [];
    };
}

/* Uri Utils */

function buildUri(baseUri, esbcredentials, params, allowedParams, operation) {

    var sb = new StringBuilder();
    sb.append(baseUri);
    sb.append(operation);
    sb.append("?ESBUserName=");
    sb.append(esbcredentials.username);
    sb.append("&ESBPassword=");
    sb.append(esbcredentials.password);
    sb.append("&ESBAccessKey=");
    sb.append(esbcredentials.accessKey);

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

/* Date Utils */

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
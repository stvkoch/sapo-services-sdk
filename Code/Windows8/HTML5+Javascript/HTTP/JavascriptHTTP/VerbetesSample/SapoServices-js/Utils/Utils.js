
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

function buildUri(baseUri, params, allowedParams, operation) {

    var sb = new StringBuilder();
    sb.append(baseUri);
    sb.append(operation);
    sb.append("?");
    //sb.append("?ESBUserName=");
    //sb.append(esbcredentials.username);
    //sb.append("&ESBPassword=");
    //sb.append(esbcredentials.password);
    //sb.append("&ESBAccessKey=");
    //sb.append(esbcredentials.accessKey);

    for (var i = 0; i < allowedParams.length; ++i) {
        if ((allowedParams[i] in params) && params[allowedParams[i]]) {
            if (i != 0)
                sb.append("&");
            sb.append(allowedParams[i]);
            sb.append("=");
            sb.append(params[allowedParams[i]]);
        }
    }

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
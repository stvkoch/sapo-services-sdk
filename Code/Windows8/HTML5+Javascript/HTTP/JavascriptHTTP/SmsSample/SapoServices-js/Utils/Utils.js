
/*
    Common Utils 
*/
(function () {

    WinJS.Namespace.define("Utils", {

        /* StringBuilder */

        StringBuilder: WinJS.Class.define(
            function () { this.content = [];},
        {
            append: function (string) {
                if (string && string.length > 0)
                    this.content.push(string);
            },

            toString: function () {
                return this.content.join("");
            },

            clear: function () {
                this.content = [];
            },
        }),

        /* Uri Utils */

        buildUri: function (baseUri, params, allowedParams, operation) {

            var sb = new Utils.StringBuilder();
            sb.append(baseUri);
            sb.append(operation);
            sb.append("?");
            //sb.append("?ESBUsername=");
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
        },

        wwwFormUrlEncode: function (params, allowedParams) {
            var sb = new Utils.StringBuilder();

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

        },

        /* Date Utils */

        dateToString: function (date) {
            var sb = new Utils.StringBuilder();
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
    });
})();
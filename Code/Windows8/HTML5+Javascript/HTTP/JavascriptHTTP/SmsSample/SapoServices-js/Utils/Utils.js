
/*
    Common Utils 
*/
(function () {

    WinJS.Namespace.define("Utils", {

        /* StringBuilder */
        //A simple string builder
        StringBuilder: WinJS.Class.define(
            function () { this.content = []; },
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
        //Function to build a URI from a base URI, a list of params and and operation. Only includes the allowed params
        buildUri: function (baseUri, params, allowedParams, operation) {

            var sb = new Utils.StringBuilder();
            sb.append(baseUri);
            sb.append(operation);
            sb.append("?");

            for (var i = 0; i < allowedParams.length; ++i) {
                var currParamName = allowedParams[i];
                var currParamValue = params[currParamName];
                if ((currParamName in params) && currParamValue) {
                    if (i != 0)
                        sb.append("&");
                    sb.append(currParamName);
                    sb.append("=");
                    sb.append(currParamValue);
                }
            }
            return sb.toString();
        },

        //Function to url-encode a list of params that are allowed
        wwwFormUrlEncode: function (params, allowedParams) {
            var sb = new Utils.StringBuilder();

            for (var i = 0; i < allowedParams.length; ++i) {
                var currParamName = allowedParams[i];
                var currParamValue = params[currParamName];
                if ((currParamName in params) && currParamValue) {
                    if (typeof (currParamValue) !== "string") {
                        for (var j = 0; j < currParamValue.length; ++j) {
                            if (i != 0)
                                sb.append("&");
                            sb.append(currParamName);
                            sb.append("=");
                            sb.append(currParamValue[j]);
                        }
                    }
                    else {
                        if (i != 0)
                            sb.append("&");
                        sb.append(currParamName);
                        sb.append("=");
                        sb.append(currParamValue);
                    }
                }
            }
            return sb.toString();

        },

        /* Date Utils */
        //Converts a date object to a string in the yyyy-mm-dd format
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
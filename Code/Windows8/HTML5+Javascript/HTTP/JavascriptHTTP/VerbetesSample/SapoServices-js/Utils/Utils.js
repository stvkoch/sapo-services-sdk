
/*
    Common Utils 
*/
(function () {

    WinJS.Namespace.define("SdkExceptions", {

        Client: WinJS.Namespace.define("SdkExceptions.Client", {

            NonProvidedCredentialsException: {
                name: "NonProvidedCredentialsException",
                message: "MUST provide username, password and accessKey"
            },
            InsuffientParametersException: {
                name: "InsuffientParametersException",
                message: "Parameters not specified or insufficient parameters"
            }
        }),
        Service: WinJS.Namespace.define("SdkExceptions.Service", {

            ServiceUnavailableException: {
                name: "ServiceUnavailableException",
                message: "Service temporarily unavailable, retry after"
            },
            InvalidRequestOrCredentialsException: {
                name: "InvalidRequestOrCredentialsException",
                message: "Bad request or invalid credentials"
            },
            RequestTimeoutException: {
                name: "RequestTimeoutException",
                message: "The service took to long to response, try after"
            },
            UnspecifiedServiceException: {
                name: "UnspecifiedServiceException",
                message: "Unspecified server error"
            }
        })
    });

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

        //Function to url-encode a list of params that are allowed
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
        },

        //WinJS.xhr requests error handling
        serviceErrorHandler: function (xhr) {
            if (xhr.status == 503)
                throw SdkExceptions.Service.ServiceUnavailableException;
            if (xhr.status == 500) {
                var e = JSON.parse(xhr.responseText);

                if ("fault" in e) {
                    var code = e.fault.detail["tns:exceptionInfo"]["tns:code"];

                    switch (code) {
                        case "1010":
                            throw SdkExceptions.Service.InvalidRequestOrCredentialsException;
                        case "2550":
                            throw SdkExceptions.Service.RequestTimeoutException;
                    }
                }
            }
            throw SdkExceptions.Service.UnspecifiedServiceException;
        },
        requestCompletedHandler: function (xhr) {
            if (xhr.status == 200 && xhr.responseText)
                return xhr.responseText;
            throw SdkExceptions.Service.UnspecifiedServiceException;
        }

    });
})();
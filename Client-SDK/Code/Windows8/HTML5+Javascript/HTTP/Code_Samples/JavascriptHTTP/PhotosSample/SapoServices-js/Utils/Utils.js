
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
                /// <summary>Appends the specified string to this instance.</summary>
                /// <param name="string" type="String">The string to append.</param>
                if (string && string.length > 0)
                    this.content.push(string);
            },

            toString: function () {
                /// <summary>Converts the value of this instance to a String.</summary>
                /// <returns type="String">A string whose value is the same as this instance.</returns>
                return this.content.join("");
            },

            clear: function () {
                /// <summary>Removes all strings from the current StringBuilder instance</summary>
                this.content = [];
            },
        }),

        /* Uri Utils */

        buildUri: function (baseUri, params, allowedParams, operation) {
            /// <summary>Function to build a URI from a base URI, a list of parameters and an operation. Only includes the allowed params in the returned URI.</summary>
            /// <param name="baseUri" type="String">The base URI.</param>
            /// <param name="params" type="Object">An object with the parameters to be included in the query string.</param>
            /// <param name="allowedParams" type="Array">Array with the allowed parameters for the query string.</param>
            /// <param name="operation" type="String">Value to be added to the path of the baseURI</param>
            /// <returns type="String">A string with the constructed URI</returns>

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

        wwwFormUrlEncode: function (params, allowedParams) {
            /// <summary>Function to www-form-url-encode the allowed properties of an object.</summary>
            /// <param name="params" type="Object">An object with the parameters to be included in the www-form-url-encoded content.</param>
            /// <param name="allowedParams" type="Array">Array with the allowed parameters for the www-form-url-encoded content.</param>
            /// <returns type="String">A string with the constructed www-form-url-encode content.</returns>

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
            /// <summary>Converts a date object to a string in the yyyy-mm-dd format.</summary>
            /// <param name="date" type="Object">Date object to be converted.</param>
            /// <returns type="String">A string with the specified date in the yyyy-mm-dd format.</returns>

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

        //WinJS.xhr requests error handler
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
        //WinJS.xhr requests completion handler
        requestCompletedHandler: function (xhr) {
            if (xhr.status == 200)
                return xhr.responseText;
            throw SdkExceptions.Service.UnspecifiedServiceException;
        },
        //Do GET request helper
        doGetRequestHelper: function (client, params, allowedParams, operation) {
            if (!params)
                params = {};

            params.json = "true";
            params.ESBUsername = client.username;
            params.ESBPassword = client.password;

            var uri =
                    Windows.Foundation.Uri(Utils.buildUri(client.baseUri, params, allowedParams,
                        operation))
                    .absoluteCanonicalUri;

            var headers = {};
            headers["Authorization"] = "ESB AccessKey=" + client.accessKey;
            return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
        }

    });
})();
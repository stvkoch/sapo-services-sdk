(function () {
    "use strict";

    WinJS.Namespace.define("Sms", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.smsBaseUri = "https://services.sapo.pt/OneAPI/SMS/SMSMessaging/";
                }
            ,
            {
                asyncSendSMSToOne: function (address, message, senderName, senderAddress) {
                    /// <summary>Do a request to SAPO SMS Service to send a SMS to a single address.</summary>
                    /// <param name="address" type="String">The address to which the message is sent.</param>
                    /// <param name="message" type="String">The message content.</param>
                    /// <param name="senderName" type="String">The sender name subject.</param>
                    /// <param name="senderAddress" type="String">The sender address.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    var allowedParams = ["senderAddress", "senderName", "message", "address",
                        "ESBUsername", "ESBPassword"];

                    if (address && message && senderName && senderAddress) {
                        var params = {};
                        params.senderAddress = encodeURIComponent("tel:"+senderAddress);
                        params.senderName = senderName;
                        params.message = encodeURIComponent(message);
                        params.address = encodeURIComponent("tel:"+address);
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        //build URI
                        var sb = new Utils.StringBuilder();
                        sb.append(this.smsBaseUri);
                        sb.append("outbound");
                        sb.append("/");
                        sb.append(params.senderAddress);
                        sb.append("/");
                        sb.append("requests");

                        var uri = Windows.Foundation.Uri(sb.toString()).absoluteCanonicalUri;

                        var data = Utils.wwwFormUrlEncode(params, allowedParams);

                        var headers = {};
                        headers["Content-Type"] = "application/x-www-form-urlencoded";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncSendSMSToMany: function (addresses, message, senderName, senderAddress) {
                    /// <summary>Do a request to SAPO SMS Service to send a SMS to multiple addresses.</summary>
                    /// <param name="addresses" type="Array">The addresses to which the message is sent.</param>
                    /// <param name="message" type="String">The message content.</param>
                    /// <param name="senderName" type="String">The sender name subject.</param>
                    /// <param name="senderAddress" type="String">The sender address.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    var allowedParams = ["senderAddress", "senderName", "message", "address",
                        "ESBUsername", "ESBPassword"];

                    if (addresses && message && senderName && senderAddress) {
                        var params = {};
                        params.senderAddress = encodeURIComponent("tel:"+senderAddress);
                        params.senderName = senderName;
                        params.message = encodeURIComponent(message);

                        var address = [];
                        for (var i = 0; i < addresses.length; ++i) {
                            if (addresses[i])
                                address.push(encodeURIComponent("tel:" + addresses[i]));
                        }
                        params.address = address;

                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        //build URI
                        var sb = new Utils.StringBuilder();
                        sb.append(this.smsBaseUri);
                        sb.append("outbound");
                        sb.append("/");
                        sb.append(params.senderAddress);
                        sb.append("/");
                        sb.append("requests");

                        var uri = Windows.Foundation.Uri(sb.toString()).absoluteCanonicalUri;

                        var data = Utils.wwwFormUrlEncode(params, allowedParams);

                        var headers = {};
                        headers["Content-Type"] = "application/x-www-form-urlencoded";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncGetDeliveryInfos: function (requestId, senderAddress) {
                    /// <summary>Do a request to SAPO SMS Service to get the delivery infos of a given message.</summary>
                    /// <param name="requestId" type="String">The request id of the message.</param>
                    /// <param name="senderAddress" type="String">The message sender address.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    if (requestId && senderAddress) {

                        var sb = new Utils.StringBuilder();
                        sb.append(this.smsBaseUri);
                        sb.append("outbound");
                        sb.append("/");
                        sb.append(encodeURIComponent("tel:" + senderAddress));
                        sb.append("/");
                        sb.append("requests");
                        sb.append("/");
                        sb.append(requestId);
                        sb.append("/");
                        sb.append("deliveryInfos");
                        sb.append("?");
                        sb.append("ESBUsername=");
                        sb.append(this.username);
                        sb.append("&");
                        sb.append("ESBPassword=");
                        sb.append(this.password);
                        
                        var uri = sb.toString();

                        var headers = {};
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


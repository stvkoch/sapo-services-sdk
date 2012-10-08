(function () {
    "use strict";

    WinJS.Namespace.define("Sms", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw "MUST provide username, password and accessKey";
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.smsBaseUri = "http://services.sapo.pt/OneAPI/SMS/SMSMessaging/";
                }
            ,
            {
                asyncsSendSMS: function (address, message, senderName, senderAddress) {
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
                        //headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(function (xhr) {
                                if (xhr.status == 201 && xhr.responseText)
                                    return xhr.responseText;
                                if (xhr.status == 503)
                                    return "SERVICE_UNAVAILABLE_RETRY_AFTER"
                                return "ERROR";
                            }, function (xhr) {
                                return "ERROR";
                            });
                    }
                    throw "MUST specify address, message, senderName and senderAddress."
                },
                asyncGetDeliveryInfos: function (requestID, senderAddress) {
                    //var allowedParams = ["ESBUsername", "ESBPassword"];

                    if (requestID && senderAddress) {
                        //var params = {};
                        //params.ESBUsername = this.username;
                        //params.ESBPassword = this.password;

                        //var uri =
                        //    Windows.Foundation.Uri(Utils.buildUri(this.smsBaseUri, params,
                        //        allowedParams, "GetURLByCompressedURL"))
                        //    .absoluteCanonicalUri;

                        var sb = new Utils.StringBuilder();
                        sb.append(this.smsBaseUri);
                        sb.append("outbound");
                        sb.append("/");
                        sb.append(encodeURIComponent("tel:" + senderAddress));
                        sb.append("/");
                        sb.append("requests");
                        sb.append("/");
                        sb.append(requestID);
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
                        //headers["Content-Type"] = "application/json";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri, headers: headers })
                            .then(function (xhr) {
                                if (xhr.status == 200 && xhr.responseText)
                                    return xhr.responseText;
                                if (xhr.status == 503)
                                    return "SERVICE_UNAVAILABLE_RETRY_AFTER"
                                return "ERROR";
                            }, function (xhr) {
                                return "ERROR";
                            });
                    }
                    throw "MUST specify parameters."
                }
            }
        )
    });

})();


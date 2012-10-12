(function () {
    "use strict";

    WinJS.Namespace.define("Mms", {
        ServiceClient: WinJS.Class.define(
                function (username, password, accessKey) {

                    if (!(username && password && accessKey))
                        throw SdkExceptions.Client.NonProvidedCredentialsException;
                    this.username = username;
                    this.password = password;
                    this.accessKey = accessKey;
                    this.mmsBaseUri = "http://services.sapo.pt/OneAPI/MMS/";
                }
            ,
            {
                /*
                    Subject and senderAddress are optional. Attachments is an array of attachment objects.
                    Attachment has two propertys, type (MIME type of the attachment) 
                    and body (The attachment contents base64 encoded).

                    Example:
                    var attachment = {
                        type: "text/plain;charset=utf-8",
                        body: "VGhlIGF0dGFjaG1lbnQgY29udGVudHMgKGJhc2U2NCBlbmNvZGVkKQ=="
                    };
                */
                asyncSendMessageWithInlineAttachmentsToOne: function (address, attachments,
                    subject, senderAddress) {
                    var allowedParams = ["ESBUsername", "ESBPassword"];

                    if (address && attachments) {
                        var params = {};
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;

                        //construct request body
                        var requestBody = {};
                        requestBody.sendMessageWithInlineAttachments = { };
                        if (subject)
                            requestBody.sendMessageWithInlineAttachments.subject = subject;
                        if (senderAddress)
                            requestBody.sendMessageWithInlineAttachments.senderAddress = senderAddress;
                        requestBody.sendMessageWithInlineAttachments.priority = 'Default';
                        requestBody.sendMessageWithInlineAttachments.format = 'MMS';

                        requestBody.sendMessageWithInlineAttachments.addresses = "tel:" + address;

                        for (var i = 0; i < attachments.length; ++i) {
                            attachments[i].id = String("<a" + i + "@local>");
                        }

                        requestBody.sendMessageWithInlineAttachments.attachments = {
                            attachment:attachments
                        };

                        var data = JSON.stringify(requestBody);

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.mmsBaseUri, params, allowedParams,
                                "SendMessageInlineAttachments/sendMessageWithInlineAttachments"))
                                 .absoluteCanonicalUri;

                        var headers = {};
                        headers["Content-Type"] = "application/json";
                        //headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncSendMessageWithInlineAttachmentsToMany: function (addresses, message, senderName, senderAddress) {
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
                        sb.append(this.mmsBaseUri);
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
                                return "ERROR";
                            }, function (xhr) {
                                if (xhr.status == 503)
                                    return "SERVICE_UNAVAILABLE_RETRY_AFTER";
                                return "ERROR";
                            });
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncGetMessageDeliveryStatus: function (requestId) {
                    var allowedParams = ["requestIdentifier", "ESBUsername", "ESBPassword", "json"];

                    if (requestId) {
                        var params = {};
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.json = "true";
                        params.requestIdentifier = String(requestId);

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.mmsBaseUri, params,
                                allowedParams, "SendMessage/getMessageDeliveryStatus"))
                            .absoluteCanonicalUri;

                        //var headers = {};
                        //headers["Authorization"] = "ESB AccessKey=" + this.accessKey;
                        return WinJS.xhr({ type: "GET", url: uri/*, headers: headers*/ })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                }
            }
        )
    });

})();


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
                    this.mmsBaseUri = "https://services.sapo.pt/OneAPI/MMS/";
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
                    /// <summary>Do a request to SAPO MMS Service to send a MMS to a single address.</summary>
                    /// <param name="address" type="String">The address to which the message is sent.</param>
                    /// <param name="attachments" type="Object">An object with the attachments of the message.</param>
                    /// <param name="subject" type="String">The message subject.</param>
                    /// <param name="senderAddress" type="String">The sender address.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>

                    var allowedParams = ["ESBUsername", "ESBPassword", "json"];

                    if (address && attachments) {
                        var params = {};
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.json = "true";

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
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },


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
                asyncSendMessageWithInlineAttachmentsToMany: function (addresses, attachments,
                    subject, senderAddress) {
                    /// <summary>Do a request to SAPO MMS Service to send a MMS to multiple addresses.</summary>
                    /// <param name="addresses" type="Array">The addresses to which the message is sent.</param>
                    /// <param name="attachments" type="Object">An object with the attachments of the message.</param>
                    /// <param name="subject" type="String">The message subject.</param>
                    /// <param name="senderAddress" type="String">The sender address.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
                    var allowedParams = ["ESBUsername", "ESBPassword", "json"];

                    if (addresses && attachments) {
                        var params = {};
                        params.ESBUsername = this.username;
                        params.ESBPassword = this.password;
                        params.json = "true";

                        //construct request body
                        var requestBody = {};
                        requestBody.sendMessageWithInlineAttachments = {};
                        if (subject)
                            requestBody.sendMessageWithInlineAttachments.subject = subject;
                        if (senderAddress)
                            requestBody.sendMessageWithInlineAttachments.senderAddress = senderAddress;
                        requestBody.sendMessageWithInlineAttachments.priority = 'Default';
                        requestBody.sendMessageWithInlineAttachments.format = 'MMS';

                        for (var i = 0; i < addresses.length; ++i)
                            addresses[i] = "tel:" + addresses[i];

                        requestBody.sendMessageWithInlineAttachments.addresses = addresses;

                        for (var i = 0; i < attachments.length; ++i) 
                            attachments[i].id = String("<a" + i + "@local>");

                        requestBody.sendMessageWithInlineAttachments.attachments = {
                            attachment: attachments
                        };

                        var data = JSON.stringify(requestBody);

                        var uri =
                            Windows.Foundation.Uri(Utils.buildUri(this.mmsBaseUri, params, allowedParams,
                                "SendMessageInlineAttachments/sendMessageWithInlineAttachments"))
                                 .absoluteCanonicalUri;

                        var headers = {};
                        headers["Content-Type"] = "application/json";
                        headers["Authorization"] = "ESB AccessKey=" + this.accessKey;

                        return WinJS.xhr({ type: "POST", url: uri, headers: headers, data: data })
                            .then(Utils.requestCompletedHandler, Utils.serviceErrorHandler);
                    }
                    throw SdkExceptions.Client.InsuffientParametersException;
                },

                asyncGetMessageDeliveryStatus: function (requestId) {
                    /// <summary>Do a request to SAPO MMS Service get the delivery status of a given message.</summary>
                    /// <param name="requestId" type="String">The request id of the message.</param>
                    /// <returns type="Object">A promisse Object with a string with the response body of the request in JSON format.</returns>
                    
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


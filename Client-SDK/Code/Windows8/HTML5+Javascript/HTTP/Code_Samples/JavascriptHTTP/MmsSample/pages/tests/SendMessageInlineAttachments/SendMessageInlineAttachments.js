// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/SendMessageInlineAttachments/SendMessageInlineAttachments.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        }
    });
})();

function sendMms() {
    var address = document.getElementById("addressParam").value || undefined;
    var senderAddress = document.getElementById("senderAddressParam").value || undefined;
    var subject = document.getElementById("subjectParam").value || undefined;

    var attachment1 = {
        type: "text/plain;charset=utf-8",
        body: "VGhlIGF0dGFjaG1lbnQgY29udGVudHMgKGJhc2U2NCBlbmNvZGVkKQ=="
    };
    var attachment2 = {
        type: "text/plain;charset=utf-8",
        body: "VGhlIGF0dGFjaG1lbnQgY29udGVudHMgKGJhc2U2NCBlbmNvZGVkKQ=="
    };

    var attachments = [];
    attachments.push(attachment1);
    attachments.push(attachment2);

    try {
        var client = new Mms.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);
        client.asyncSendMessageWithInlineAttachmentsToOne(address, attachments, subject, senderAddress).then(
            function (resultText) {
                var elementById = document.getElementById("sendMmsResult");
                elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
            },
            function (e) {
                var exceptionName = e.name;
                var exceptionMessage = e.message;
            });
    }
    catch (e) {
        //Add catch exception logic here
        var exceptionName = e.name;
        var exceptionMessage = e.message;
    }

}

// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/SendSms/SendSms.html", {
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

function sendSms() {
    var client = new Sms.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

        var senderName = document.getElementById("senderNameParam").value || undefined;
        var senderAddress = document.getElementById("senderAddressParam").value || undefined;
        var address = document.getElementById("addressParam").value || undefined;
        var message = document.getElementById("messageParam").value || undefined;

        client.asyncsSendSMSToOne(address, message, senderName, senderAddress).then(function (resultText) {
            var elementById = document.getElementById("sendSmsResult");
            elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
        });
}

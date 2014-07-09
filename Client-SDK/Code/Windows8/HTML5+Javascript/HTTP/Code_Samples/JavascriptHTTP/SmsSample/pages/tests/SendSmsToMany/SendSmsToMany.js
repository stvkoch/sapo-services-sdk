// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/SendSmsToMany/SendSmsToMany.html", {
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
    var senderName = document.getElementById("senderNameParam").value || undefined;
    var senderAddress = document.getElementById("senderAddressParam").value || undefined;
    var address1 = document.getElementById("address1Param").value || undefined;
    var address2 = document.getElementById("address2Param").value || undefined;
    var address3 = document.getElementById("address3Param").value || undefined;
    var message = document.getElementById("messageParam").value || undefined;

    var addresses = [];
    if(address1)
        addresses.push(address1);
    if(address2)
        addresses.push(address2);
    if(address3)
        addresses.push(address3);

    try {
        var client = new Sms.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);
        client.asyncSendSMSToMany(addresses, message, senderName, senderAddress).then(function (resultText) {
            var elementById = document.getElementById("sendSmsResult");
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

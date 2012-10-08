// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/GetDeliveryInfos/GetDeliveryInfos.html", {
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

function getDeliveryInfos() {
    var client = new Sms.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

    var senderAddress = document.getElementById("senderAddressParam").value || undefined;
    var requestId = document.getElementById("requestIdParam").value || undefined;

    client.asyncGetDeliveryInfos(requestId, senderAddress).then(function (resultText) {
        var elementById = document.getElementById("getDeliveryInfosResult");
        elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
    });
}
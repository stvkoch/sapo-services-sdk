// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/CheckVideo/CheckVideo.html", {
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

function checkVideo() {
    var client = new Videos.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

    var randname = document.getElementById("randnameParam").value;

    client.asyncCheckVideo(randname || undefined).then(function (resultText) {
        var elementById = document.getElementById("checkVideoResult");
        elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
    });
}

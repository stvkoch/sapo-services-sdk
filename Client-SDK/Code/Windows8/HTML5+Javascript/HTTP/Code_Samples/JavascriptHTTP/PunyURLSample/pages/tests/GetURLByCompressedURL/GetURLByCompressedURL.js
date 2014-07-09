// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/GetURLByCompressedURL/GetURLByCompressedURL.html", {
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

function getURLByCompressedURL() {
    var url = document.getElementById("urlParam").value;

    try {
        var client = new PunyURL.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);
        client.asyncGetURLByCompressedURL(url).then(function (resultText) {
            var elementById = document.getElementById("getURLByCompressedURLResult");
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

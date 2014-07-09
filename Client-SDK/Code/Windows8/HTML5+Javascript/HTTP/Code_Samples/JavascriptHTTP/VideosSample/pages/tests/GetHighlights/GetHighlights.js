// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/GetHighlights/GetHighlights.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            var elementById = document.getElementById("btGetHighlights");
            elementById.addEventListener("click", this.getHighlights);
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        },
        getHighlights: function () {
            try {
                var client = new Videos.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

                client.asyncGetHighlights().then(function (resultText) {
                    var elementById = document.getElementById("getHighlightsResult");
                    elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
                },
                function (e) {
                    var exceptionName = e.name;
                    var exceptionMessage = e.message;
                });
            }
            catch (e) {
                var exceptionName = e.name;
                var exceptionMessage = e.message;
            }
        }
    });
})();

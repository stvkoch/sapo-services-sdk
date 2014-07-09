// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/SearchVideos/SearchVideos.html", {
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


function searchVideos() {
    try {
        var client = new Videos.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

        var search = document.getElementById("searchParam").value || undefined;
        var user = document.getElementById("userParam").value || undefined;
        var page = document.getElementById("pageParam").value || undefined;
        var limit = document.getElementById("limitParam").value || undefined;
        var order = document.getElementById("orderParam").value || undefined;

        client.asyncSearchVideos(search, user, page, limit, order).then(function (resultText) {
            var elementById = document.getElementById("searchVideosResult");
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
// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/ImageAddToAlbum/ImageAddToAlbum.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            authenticationData = options;
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

function addImageToAlbum() {
    //Albums MUST be separated by commas with none blank spaces
    var albumids = document.getElementById("albumidsParam").value || undefined;
    var imageuid = document.getElementById("imageuidParam").value || undefined;

    var client = new Photos.ServiceClient(
        authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client.asyncImageAddToAlbum(imageuid, albumids).then(function (resultText) {
        var resultDiv = document.getElementById("addImageToAlbumResult");

        resultDiv.innerHTML = resultText;
    });
}



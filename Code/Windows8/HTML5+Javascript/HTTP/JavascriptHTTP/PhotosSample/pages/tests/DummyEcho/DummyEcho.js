// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/DummyEcho/DummyEcho.html", {
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

function dummyEcho() {
    var echoStrParam = document.getElementById("echoStrParam").value;

    var params = {};
    params.echoStr = echoStrParam || undefined;

    var client = new Photos.ServiceClient(
        authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client.asyncDummyEcho(params).then(function (resultText) {
        var resultDiv = document.getElementById("dummyEchoResult");

        resultDiv.innerHTML = resultText;
    });

}

// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/getPersonalities/getPersonalities.html", {
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

function getPersonalities() {
    var minParam = document.getElementById("minParam").value;
    var formatParam = document.getElementById("formatParam").value;

    var params = {};
    params.min = minParam || undefined;
    params.format = formatParam || undefined;
    
    var client = new Verbetes.ServiceClient(authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client.asyncGetPersonalities(params)
        .then(function (resultText) {
            var resultDiv = document.getElementById("getPersonalitiesResult");

            resultDiv.innerHTML = resultText;
        });
}

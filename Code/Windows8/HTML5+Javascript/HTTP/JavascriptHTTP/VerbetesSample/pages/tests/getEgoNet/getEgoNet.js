// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/getEgoNet/getEgoNet.html", {
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

function getEgoNet() {
    var nameParam = document.getElementById("nameParam").value;
    var depthParam = document.getElementById("depthParam").value;
    var minFrequencyEdgesParam = document.getElementById("minFrequencyEdgesParam").value;

    var beginDatePicker = document.getElementById("beginDateParam").winControl;
    var beginDateParam = beginDatePicker.current;

    var endDatePicker = document.getElementById("endDateParam").winControl;
    var endDateParam = endDatePicker.current;

    var params = {};

    params.name = nameParam || undefined;
    params.depth = depthParam || undefined;
    params.minFrequencyEdges = minFrequencyEdgesParam || undefined;
    params.beginDate = beginDateParam || undefined;
    params.endDate = endDateParam || undefined;
    
    var client = new VerbetesServiceClient(authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client
        .asyncGetEgoNet(params)
        .then(function (resultText) {
            var resultDiv = document.getElementById("getEgoNetResult");

            resultDiv.innerHTML = resultText;
        });
}

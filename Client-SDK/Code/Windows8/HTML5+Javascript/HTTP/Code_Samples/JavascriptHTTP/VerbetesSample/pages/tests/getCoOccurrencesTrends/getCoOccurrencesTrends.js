// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/getCoOccurrencesTrends/getCoOccurrencesTrends.html", {
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

function getCoOccurencesTrends() {
    var name1Param = document.getElementById("name1Param").value;
    var name2Param = document.getElementById("name2Param").value;
    var formatParam = document.getElementById("formatParam").value;
    var beginDatePicker = document.getElementById("beginDateParam").winControl;
    var beginDateParam = beginDatePicker.current;

    var endDatePicker = document.getElementById("endDateParam").winControl;
    var endDateParam = endDatePicker.current;

    var params = {};

    params.name1 = name1Param || undefined;
    params.name2 = name2Param || undefined;
    params.format = formatParam || undefined;
    params.begin_date = beginDateParam || undefined;
    params.end_date = endDateParam || undefined;

    try {
        var client =
            new Verbetes.ServiceClient(authenticationData.username, authenticationData.password, authenticationData.accessKey);

        client
            .asyncGetCoOccurrencesTrends(params)
            .then(function (resultText) {
                var resultDiv = document.getElementById("getCoOccurencesTrendsResult");

                resultDiv.innerHTML = resultText;
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

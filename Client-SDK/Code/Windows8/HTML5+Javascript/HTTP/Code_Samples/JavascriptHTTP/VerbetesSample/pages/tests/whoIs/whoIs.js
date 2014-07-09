// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/whoIs/whoIs.html", {
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


//function getXMLHttpRequest() {
//    if (window.XMLHttpRequest) {
//        return new window.XMLHttpRequest;
//    }
//    else {
//        try {
//            return new ActiveXObject("MSXML2.XMLHTTP.3.0");
//        }
//        catch (ex) {
//            return null;
//        }
//    }
//}


function whoIs() {
    var nameParam = document.getElementById("nameParam").value;
    var nameLikeParam = document.getElementById("name_likeParam").value;
    var jobParam = document.getElementById("jobParam").value;
    var jobLikeParam = document.getElementById("job_likeParam").value;
    var marginParam = document.getElementById("marginParam").value;
    var minParam = document.getElementById("minParam").value;
    var formatParam = document.getElementById("formatParam").value;

    var datePicker = document.getElementById("dateParam").winControl;
    var dateParam = datePicker.current;

    var params = {};

    params.name = nameParam || undefined;
    params.name_like = nameLikeParam || undefined;
    params.job = jobParam || undefined;
    params.job_like = jobLikeParam || undefined;
    params.margin = marginParam || undefined;
    params.min = minParam || undefined;
    params.format = formatParam || undefined;
    params.date = dateParam || undefined;


    try {
        var client = new Verbetes.ServiceClient(
            authenticationData.username, authenticationData.password, authenticationData.accessKey);

        client.asyncWhoIs(params)
            .then(function (resultText) {
                var resultDiv = document.getElementById("getWhoIsResult");

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

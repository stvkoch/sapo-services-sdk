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

    var client = new Verbetes.ServiceClient(
        authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client.asyncWhoIs(params)
        .then(function (resultText) {
            var resultDiv = document.getElementById("getWhoIsResult");

            resultDiv.innerHTML = resultText;
        });


		
		

    //var uriWithAutentication = "https://services.sapo.pt/InformationRetrieval/Verbetes/WhoIs?ESBUserName=&ESBPassword=&ESBAccessKey=";

    //var finalUri = uriWithAutentication+"&name="+nameParam; 

    //WinJS.xhr({ type: "GET", url: finalUri })
    //    .then(function (xhr) {
    //        //var resultDiv = document.getElementById("getWhoIsResult");

    //        //resultDiv.innerHTML = xhr.responseText;

    //        var res = eval("("+xhr.responseText+")");

    //        var res1 = JSON.parse(xhr.responseText);

    //        return xhr.responseText;
    //    })
    //    .then(function (text) {
    //        var resultDiv = document.getElementById("getWhoIsResult");

    //        resultDiv.innerHTML = text;
    //});

    //WinJS.xhr({ data: "{\"user\":\"slb\"} ", type: "POST", url: "http://127.0.0.1:8080/api/users" }).then(function(xhr) {
    //        var resultDiv = document.getElementById("getWhoIsResult");

    //        resultDiv.innerHTML = xhr.responseText;    
    //});

    //var oReq = getXMLHttpRequest();
    //if (oReq != null) {
    //    oReq.open("GET", finalUri, true);
    //    oReq.onreadystatechange = handler;
    //    oReq.send();
    //}
    //function handler() {
    //    var resultDiv = document.getElementById("getWhoIsResult");

    //    resultDiv.innerHTML = oReq.responseText;
    //}
}

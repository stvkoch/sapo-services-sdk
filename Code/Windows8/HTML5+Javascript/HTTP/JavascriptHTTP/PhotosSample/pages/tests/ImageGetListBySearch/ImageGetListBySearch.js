// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/ImageGetListBySearch/ImageGetListBySearch.html", {
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

function imageGetListBySearch() {
    var stParam = document.getElementById("stParam").value;
    var pageParam = document.getElementById("pageParam").value;
    
    var fromDatePicker = document.getElementById("datefromParam").winControl;
    var datefromParam = fromDatePicker.current;

    var toDatePicker = document.getElementById("datetoParam").winControl;
    var datetoParam = toDatePicker.current;

    var params = {};

    //this parameter cannot have blank spaces.
    params.string = stParam || undefined;
    params.page = pageParam || undefined;
    params.datefrom = datefromParam || undefined;
    params.dateto = datetoParam || undefined;
    
    var client = new Photos.ServiceClient(
        authenticationData.username, authenticationData.password, authenticationData.accessKey);

    try {
        client.asyncImageGetListBySearch(params).then(function(resultText) {
            var resultDiv = document.getElementById("imageGetListBySearchResult");

            var response = JSON.parse(resultText);

            var imageGetListBySearchResult = response.ImageGetListBySearchResponse.ImageGetListBySearchResult;

            var images = imageGetListBySearchResult.images.image;


            //views.view has the various thumbnails of the photos.
            for (var i = 0; i < images.length; ++i) {
                resultDiv.innerHTML += images[i].views.view[0].url;
            }
            resultDiv.innerHTML += "</br>";
            resultDiv.innerHTML += resultText;

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

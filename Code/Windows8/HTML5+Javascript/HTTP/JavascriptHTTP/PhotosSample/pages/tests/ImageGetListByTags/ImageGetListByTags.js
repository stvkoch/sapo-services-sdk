// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/ImageGetListByTags/ImageGetListByTags.html", {
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

function imageGetListByTags() {
    var tagParam = document.getElementById("tagParam").value;
    var orderbyParam = document.getElementById("orderbyParam").value;
    var pageParam = document.getElementById("pageParam").value;
    var m18Param = document.getElementById("m18Param").value;
    var usernameParam = document.getElementById("usernameParam").value;

    var params = {};

    params.tag = tagParam || undefined;
    params.orderby = orderbyParam || undefined;
    params.page = pageParam || undefined;
    params.m18 = m18Param || undefined;
    params.username = usernameParam || undefined;
    
    var client = new PhotosServiceClient(
        authenticationData.username, authenticationData.password, authenticationData.accessKey);

    client.asyncImageGetListByTags(params).then(function(resultText) {
        var resultDiv = document.getElementById("imageGetListByTagResult");
        
        resultDiv.innerHTML = resultText;
    });
}

// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/GetUserInfo/GetUserInfo.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            var elementById = document.getElementById("btGetUserInfo");
            elementById.addEventListener("click", this.getUserInfo);
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        },
        getUserInfo: function () {
            var client = new Videos.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

            client.asyncGetUserInfo().then(function (resultText) {
                var elementById = document.getElementById("getUserInfoResult");
                elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));
            });
        }
    });
})();

// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/SearchByBrandModelPrice/SearchByBrandModelPrice.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
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

function searchByBrandModelPrice() {
    //document.getElementById("searchByBrandModelPriceResult").innerHTML = "Search results";
    var params = {};
    params.Brand = document.getElementById("brandParam").value || undefined;
    params.Model = document.getElementById("modelParam").value || undefined;
    params.Price = document.getElementById("priceParam").value || undefined;
    params.start = document.getElementById("startParam").value || undefined;
    params.sort = document.getElementById("sortParam").value || undefined;
    params.rows = document.getElementById("rowsParam").value || undefined;

    try {
        var client = new Auto.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);
        client.asyncSearchByBrandModelPrice(params).then(function (resultText) {
            var elementById = document.getElementById("searchByBrandModelPriceResult");
            elementById.appendChild(elementById.ownerDocument.createTextNode(resultText));

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

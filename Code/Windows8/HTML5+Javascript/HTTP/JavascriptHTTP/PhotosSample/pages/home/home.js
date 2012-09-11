(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
        }
    });
})();

function test () {
    var obj = {};

    obj.image = { title: "microsoft", tags: "microsoft" };

    //var s = JSON.stringify(obj);

    var headers = {};
    headers["Content-Type"] = "application/json";

    WinJS.xhr({
        type: "POST", headers: headers, url:
            "http://services.sapo.pt/Photos/ImageCreate?ESBUsername=username&ESBPassword=password&interface=fotos.sapo.pt",
        data: JSON.stringify(obj)
    }).then(function (xhr) {
            if (xhr.status == 200 && xhr.responseText)
                return xhr.responseText;
            return null;
        });
}

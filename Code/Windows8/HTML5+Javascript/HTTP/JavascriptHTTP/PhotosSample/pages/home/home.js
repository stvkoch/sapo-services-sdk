var globalAuthData =
                    {
                        username: undefined,
                        password: undefined,
                        accessKey: undefined
                    };

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            document.getElementById('username').value = globalAuthData.username || "";
            document.getElementById('password').value = globalAuthData.password || "";
            document.getElementById('accessKey').value = globalAuthData.accessKey || "";
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

function navigateTo(page) {
    globalAuthData.username = document.getElementById('username').value;
    globalAuthData.password = document.getElementById('password').value;
    globalAuthData.accessKey = document.getElementById('accessKey').value;

    WinJS.Navigation.navigate(page,
        {
            username: globalAuthData.username,
            password: globalAuthData.password,
            accessKey: globalAuthData.accessKey
        });
}


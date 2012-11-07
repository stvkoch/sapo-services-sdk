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


(function () {
    "use strict";

    WinJS.Namespace.define("GlobalAuth", {
        username: undefined,
        password: undefined,
        accessKey: undefined
    });

    function navigateTo(page) {
        GlobalAuth.username = document.getElementById('username').value;
        GlobalAuth.password = document.getElementById('password').value;
        GlobalAuth.accessKey = document.getElementById('accessKey').value;

        WinJS.Navigation.navigate(page);
    } 

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            document.getElementById('username').value = GlobalAuth.username || "";
            document.getElementById('password').value = GlobalAuth.password || "";
            document.getElementById('accessKey').value = GlobalAuth.accessKey || "";

            var elementById = document.getElementById("btSearchByTerms");
            elementById.addEventListener("click",
                function () { navigateTo('/pages/tests/SearchByTerms/SearchByTerms.html'); });

            elementById = document.getElementById("btSearchByBrandModelPrice");
            elementById.addEventListener("click",
                function () { navigateTo('/pages/tests/SearchByBrandModelPrice/SearchByBrandModelPrice.html'); });
        }
    });
})();

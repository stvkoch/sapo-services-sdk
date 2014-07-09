(function () {
    "use strict";

    //var globalAuthData =
    //                {
    //                    username: undefined,
    //                    password: undefined,
    //                    accessKey: undefined
    //                };

    WinJS.Namespace.define("GlobalAuth", {
        username: undefined,
        password: undefined,
        accessKey: undefined
    });

    function navigateTo(page) {
        GlobalAuth.username = document.getElementById('username').value;
        GlobalAuth.password = document.getElementById('password').value;
        GlobalAuth.accessKey = document.getElementById('accessKey').value;

        WinJS.Navigation.navigate(page,
            {
                username: GlobalAuth.username,
                password: GlobalAuth.password,
                accessKey: GlobalAuth.accessKey
            });
    }


    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            document.getElementById('username').value = GlobalAuth.username || "";
            document.getElementById('password').value = GlobalAuth.password || "";
            document.getElementById('accessKey').value = GlobalAuth.accessKey || "";

            var elementById = document.getElementById("btAddVideoPost");
            elementById.addEventListener("click", function () { navigateTo('/pages/tests/AddVideoPost/AddVideoPost.html'); });

            elementById = document.getElementById("btDeleteVideo");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/DeleteVideo/DeleteVideo.html'); });
            
            elementById = document.getElementById("btCheckVideo");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/CheckVideo/CheckVideo.html'); });
            
            elementById = document.getElementById("btGetUserInfo");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/GetUserInfo/GetUserInfo.html'); });
            
            elementById = document.getElementById("btListUserVideos");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/ListUserVideos/ListUserVideos.html'); });
            
            elementById = document.getElementById("btGetHighlights");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/GetHighlights/GetHighlights.html'); });
            
            elementById = document.getElementById("btSearchVideos");
            elementById.addEventListener("click", function () { navigateTo('pages/tests/SearchVideos/SearchVideos.html'); });
        }
    });
})();

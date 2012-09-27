// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511
(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/CaptchaAsAudio/CaptchaAsAudio.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            var elementById = document.getElementById("btGenerateCaptcha");
            elementById.addEventListener("click", this.generateCaptcha);
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        },
        generateCaptcha: function () {
            var client = new Captcha.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

            client.asyncGet().then(function (result) {
                if (result != "ERROR") {
                    var captchaImg = document.getElementById("captchaImg");
                    captchaImg.src = client.buildPlayURI(result.id).absoluteCanonicalUri;

                    document.getElementById("captchaSolution").innerHTML =
                        "Captcha Solution: " + result.code;
                }
            });


        }
    });
})();

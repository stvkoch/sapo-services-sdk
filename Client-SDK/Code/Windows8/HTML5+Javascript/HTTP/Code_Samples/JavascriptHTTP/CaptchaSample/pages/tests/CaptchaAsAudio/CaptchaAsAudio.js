// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var playCaptchaEvtHandler;

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

            try {
                var client = new Captcha.ServiceClient(GlobalAuth.username, GlobalAuth.password, GlobalAuth.accessKey);

                client.asyncGet().then(function (result) {
                    if (result != "ERROR") {
                        var btPlayCaptcha = document.getElementById("btPlayCaptcha");

                        if (playCaptchaEvtHandler)
                            btPlayCaptcha.removeEventListener("click", playCaptchaEvtHandler, false);

                        playCaptchaEvtHandler = function () {
                            //play the audio captcha
                            var audio = new Audio(client.buildPlayURI(result.id).absoluteCanonicalUri);
                            audio.load();
                            audio.play();
                        };

                        btPlayCaptcha.addEventListener("click", playCaptchaEvtHandler);

                        btPlayCaptcha.disabled = "";

                        document.getElementById("captchaSolution").innerHTML =
                            "Captcha Solution: " + result.code;
                    }
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

        },
    });
})();

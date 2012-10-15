// For an introduction to the Page Control template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232511

var authenticationData;

(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/tests/AddVideoPost/AddVideoPost.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            authenticationData = options;
            var elementById = document.getElementById("btPickVideo");
            elementById.addEventListener("click", this.videoCreate);
        },

        unload: function () {
            // TODO: Respond to navigations away from this page.
        },

        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            // TODO: Respond to changes in viewState.
        },
        videoCreate: function () {
            var currentState = Windows.UI.ViewManagement.ApplicationView.value;
            if (currentState === Windows.UI.ViewManagement.ApplicationViewState.snapped &&
                !Windows.UI.ViewManagement.ApplicationView.tryUnsnap()) {
                // Fail silently if we can't unsnap
                return;
            }

            // Create the picker object and set options
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.viewMode = Windows.Storage.Pickers.PickerViewMode.thumbnail;
            openPicker.suggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.picturesLibrary;
            // Users expect to have a filtered view of their folders depending on the scenario.
            // For example, when choosing a documents folder, restrict the filetypes to documents for your application.
            openPicker.fileTypeFilter.replaceAll([".avi", ".mpeg", ".3gp", ".mp4", ".mov", ".wmv"]);

            openPicker.pickSingleFileAsync().then(function (file) {
                if (file) {
                    var video = {};
                    //Important 1: MUST specify at least Title, Tags and Synopse.
                    //Important 2:All video attribute names begin with capital letter.
                    video.Title = file.displayName;
                    video.Tags = file.displayName;
                    video.Synopse = file.displayName;
                    try {
                        var client =
                            new Videos.ServiceClient(
                                GlobalAuth.username,
                                GlobalAuth.password,
                                GlobalAuth.accessKey);


                        client.asyncAddVideoPost(file, video).then(function(resultCode) {
                            document.getElementById("addVideoPostResult").innerHTML = resultCode;
                        },
                        function (e) {
                            var exceptionName = e.name;
                            var exceptionMessage = e.message;
                        });
                    }
                    catch(e) {
                        var exceptionName = e.name;
                        var exceptionMessage = e.message;
                    }

                } else {
                    // The picker was dismissed with no selected file

                }
            });
        }
    });
})();

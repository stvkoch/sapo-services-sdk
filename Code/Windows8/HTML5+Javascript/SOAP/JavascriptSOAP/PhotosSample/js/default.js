// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched. Initialize
                // your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
    };

    app.start();
})();

function pickAndSendPhoto() {
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
    openPicker.fileTypeFilter.replaceAll([".png", ".jpg", ".jpeg"]);

    // Open the picker for the user to pick a file
    openPicker.pickSingleFileAsync().then(function (file) {
        if (file) {
            // Application now has read/write access to the picked file
            var proxy = new PhotosProxy.SapoServices.PhotosServiceClient(document.getElementById('username').value,
                document.getElementById('password').value, document.getElementById('accessKey').value);

            var img = new PhotosProxy.PhotosServiceReference.Image();

            img.title = file.displayName;
            img.tags = file.displayName;

            proxy.createImageOperationAsync(file, img).then(function (result) {
                var resultSpan = document.getElementById("photoCreateResult");
                resultSpan.innerHTML = result.responseResult;
            });

        } else {
            // The picker was dismissed with no selected file

        }
    });
}

function deletePhoto() {
    var proxy = new PhotosProxy.SapoServices.PhotosServiceClient(document.getElementById('username').value,
                document.getElementById('password').value, document.getElementById('accessKey').value);

    var photoId = document.getElementById("input_photoToDeleteId").value;

    //proxy.getUserDetailsOperationAsync("").then(function(u) {
    //        var resultSpan = document.getElementById("photoDeleteResult");
    //        resultSpan.innerHTML = u.url;
    //});

    proxy.getImageDetailsOperationAsync(photoId)
        .then(
            function (res) {
                var resultSpan = document.getElementById("photoDeleteResult");
                resultSpan.innerHTML = u.url;
            },
            function (error) {
                var e = error;
            });

    //proxy.deleteImageOperationAsync(photoId).then(function (result) {
    //    var resultSpan = document.getElementById("photoDeleteResult");
    //    resultSpan.innerHTML = result ? "Success" : "Error";
    //});
    
}

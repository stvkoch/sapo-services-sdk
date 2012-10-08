(function () {
    "use strict";

    function getAllHolidays() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetAllHolidays(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getCarnival() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetCarnival(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getCorpusChristi() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetCorpusChristi(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getEaster() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetEaster(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getGoodFriday() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetGoodFriday(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getLocalHolidays() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetLocalHolidays(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getNationalHolidays() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetNationalHolidays(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    function getRegionalHolidays() {
        var username = document.getElementById("username").value || undefined;
        var password = document.getElementById("password").value || undefined;
        var accessKey = document.getElementById("accessKey").value || undefined;
        var year = document.getElementById("year").value || undefined;

        var client = new Holiday.ServiceClient(username, password, accessKey);

        client.asyncGetRegionalHolidays(year).then(function (result) {
            document.getElementById("resultsDiv").innerHTML = result;
        });
    }

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            // TODO: Initialize the page here.
            var button = document.getElementById("btGetAllHolidays");
            button.addEventListener("click", getAllHolidays);

            button = document.getElementById("btGetCarnival");
            button.addEventListener("click", getCarnival);

            button = document.getElementById("btGetCorpusChristi");
            button.addEventListener("click", getCorpusChristi);

            button = document.getElementById("btGetEaster");
            button.addEventListener("click", getEaster);

            button = document.getElementById("btGetGoodFriday");
            button.addEventListener("click", getGoodFriday);

            button = document.getElementById("btGetLocalHolidays");
            button.addEventListener("click", getLocalHolidays);

            button = document.getElementById("btGetNationalHolidays");
            button.addEventListener("click", getNationalHolidays);

            button = document.getElementById("btGetRegionalHolidays");
            button.addEventListener("click", getRegionalHolidays);
        }
    });
})();

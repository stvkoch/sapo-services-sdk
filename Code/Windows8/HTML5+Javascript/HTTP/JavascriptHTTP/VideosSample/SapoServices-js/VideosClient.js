function VideosServiceClient(username, password, accessKey) {

    if (!(username && password && accessKey))
        throw "MUST provide username, password and accessKey";

    var videosBaseUri = "https://services.sapo.pt/Videos/";

}
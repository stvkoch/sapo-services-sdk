function StringBuilder() {
    this.content = [];

    this.append = function (string) {
        if (string && string.length > 0)
            this.content.push(string);
    };

    this.toString = function () {
        return this.content.join("");
    };

    this.clear = function () {
        this.content = [];
    };
}
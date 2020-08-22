function callWebService(httpMethod, uri, data, success, failure) {
    var request = new XMLHttpRequest();
    request.open(httpMethod, uri, true);
    request.setRequestHeader('Content-Type', 'application/json');
    request.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE && this.status === 200)
            success(JSON.parse(this.response));
        else
            failure(JSON.parse(this.response));
    }
    if (data != null)
        request.send(data);
    else
        request.send();
}
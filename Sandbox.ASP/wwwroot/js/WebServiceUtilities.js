function callWebService(httpMethod, uri, data, success, failure) {
    var request = new XMLHttpRequest();
    request.open(httpMethod, uri, true);
    request.setRequestHeader('Content-Type', 'application/json');
    request.onreadystatechange = function () {
        var result = this.response;
        if (this.status === 200) {
            if (this.readyState === XMLHttpRequest.DONE) {
                try {
                    result = JSON.parse(this.response);
                } catch { }

                success(result);
            }
        } else {
            try {
                result = JSON.parse(this.response);
            } catch { }

            failure(result);
        }
    }
    if (data != null)
        request.send(data);
    else
        request.send();
}
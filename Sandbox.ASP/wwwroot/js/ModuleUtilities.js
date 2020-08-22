function getModule(id, success, failure) {
    callWebService("POST", "Modules/GetDetails", JSON.stringify({ id: id }), function (result) {
        success(result);
    }, function (result) {
        failure(result);
    });
}
function getModuleParameters(id, success, failure) {
    callWebService("POST", "Modules/GetParameters", JSON.stringify({ id: id }), function (result) {
        success(result);
    }, function (result) {
        failure(result);
    });
}
function executeModule(request, success, failure) {
    callWebService("POST", "Modules/Execute", request, function (result) {
        success(result);
    }, function (result) {
        failure(result);
    });
}
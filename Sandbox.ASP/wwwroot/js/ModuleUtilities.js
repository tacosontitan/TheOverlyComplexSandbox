function getModule(id, success, failure) {
    callWebService("POST", "Modules/GetDetails", JSON.stringify({ id: id }), success, failure);
}
function getModuleParameters(id, success, failure) {
    callWebService("POST", "Modules/GetParameters", JSON.stringify({ id: id }), success, failure);
}
function executeModule(request, success, failure) {
    callWebService("POST", "Modules/Execute", JSON.stringify(request), success, failure);
}
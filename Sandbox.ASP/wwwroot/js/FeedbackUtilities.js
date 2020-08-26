function submitFeedback(displayName, feedback, success, failure) {
    callWebService("POST", "Feedback/Submit", JSON.stringify({ displayName: displayName, feedback: feedback }), success, failure);
}
function fetchFeedback(success, failure) {
    callWebService("POST", "Feedback/Fetch", null, success, failure);
}
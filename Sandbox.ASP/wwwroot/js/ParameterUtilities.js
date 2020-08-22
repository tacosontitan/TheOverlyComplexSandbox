function createDisplayElementForParameter(parameter) {
    var displayElement = undefined;
    var container = createContainer(parameter);

    switch (parameter.displayElement) {
        case DisplayElement.Checkbox: displayElement = createCheckbox(parameter); break;
        case DisplayElement.Slider: displayElement = createSlider(parameter); break;
        case DisplayElement.Textbox: displayElement = createTextbox(parameter); break;
        case DisplayElement.RichTextbox: displayElement = createRichTextbox(parameter); break;
        default: displayElement = createTextbox(parameter); break;
    }

    if (displayElement != undefined)
        container.appendChild(displayElement);

    container.classList.add("mb-3");
    return container;
}

// Display Elements
function createHelpText(helpID, helpText) {
    var container = createContainer();
    container.id = helpID;
    container.classList.add("form-text");
    container.innerText = helpText;
    return container;
}
function createCheckbox(parameter) {
    var container = createContainer();
    container.classList.add("form-check");

    var inputID = "de-cb-" + parameter.name;
    container.appendChild(createInput(inputID, "checkbox", "form-check-input", undefined));
    container.appendChild(createLabel(parameter.requestMessage, inputID, "form-check-label"));
    return container;
}
function createSlider(parameter) {
    var container = createContainer();

    var inputID = "de-rng-" + parameter.name;
    var helpID = inputID + "-help";
    var range = createInput(inputID, "range", "form-range", helpID);
    range.setAttribute("min", parameter.minValue.toString());
    range.setAttribute("max", parameter.maxValue.toString());
    range.setAttribute("step", "1");

    container.appendChild(createLabel(parameter.displayName, inputID, "form-label"));
    container.appendChild(range);
    container.appendChild(createHelpText(helpID, parameter.requestMessage));
    return container;
}
function createTextbox(parameter) {
    var container = createContainer();
    var inputID = "de-tb-" + parameter.name;
    var helpID = inputID + "-help";
    container.appendChild(createLabel(parameter.displayName, inputID, "form-label"));
    container.appendChild(createInput(inputID, "text", "form-control", helpID));
    container.appendChild(createHelpText(helpID, parameter.requestMessage));
    return container;
}
function createRichTextbox(parameter) {
    var container = createContainer();

    var inputID = "de-rtb-" + parameter.name;
    var helpID = inputID + "-help";
    var richTextbox = document.createElement("TEXTAREA");
    richTextbox.id = inputID;
    richTextbox.setAttribute("aria-describedby", helpID);
    richTextbox.classList.add("form-control");
    richTextbox.setAttribute("rows", "3");

    container.appendChild(createLabel(parameter.displayName, inputID, "form-label"));
    container.appendChild(richTextbox);
    container.appendChild(createHelpText(helpID, parameter.requestMessage));
    return container;
}

// Natural Elements
function createContainer() {
    var container = document.createElement("DIV");
    return container;
}
function createInput(id, type, bootstrapClass, describedBy) {
    var input = document.createElement("INPUT");
    input.id = id;
    input.setAttribute("type", type);
    input.classList.add(bootstrapClass);

    if (describedBy != null && describedBy != undefined)
        input.setAttribute("aria-describedby", describedBy);

    return input;
}
function createLabel(innerText, forID, bootstrapClass) {
    var label = document.createElement("LABEL");
    label.setAttribute("for", forID);
    label.classList.add(bootstrapClass);
    label.innerText = innerText;
    return label;
}
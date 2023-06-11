window.ScrollToBottom = (elementName) => {
    element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}

window.getWindowDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.LogConsole = function (data) {
    console.log(data);
};

window.buttonScroll = function scroll(element, x, y) {
    element.scrollLeft += x;
    element.scrollTop += y;
}
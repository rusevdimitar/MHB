window.onload = initAll;
var xhr = false;
var id = 0;

function initAll() {
    $("#btnSearch").live("click", function () {
        id = $("#inputText").attr("value");

        makeRequest();
    });

    $("#inputText").live("keypress", function () {
        id = $("#inputText").attr("value");
        alert(id);
        makeRequest();
    });
}

function makeRequest() {
    if (window.XMLHttpRequest) {
        xhr = new XMLHttpRequest();
    }
    else {
        if (window.ActiveXObject) {
            try {
                xhr = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) { }
        }
    }

    if (xhr) {
        xhr.onreadystatechange = showContents;
        xhr.open("GET", "FlagExpense.aspx?id=" + id, true);
        xhr.send(null);
    } else {
        document.getElementById("statusDiv").innerHTML = "Sorry, I couldn't create an XMLHttpRequest";
    }
}

function showContents() {
    if (xhr.readyState == 4) {
        if (xhr.status == 200) {
            var outMsg = xhr.responseText;
        }
        else {
            var outMsg = "There was a problem with the request " + xhr.status;
        }
        document.getElementById("statusDiv").innerHTML = outMsg;
    }
}
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var isHidden = true;
$(document).ready(function () {
    
    $("#hideSpan").click(function () {
        isHidden = !isHidden;
        $("#hideSpan").text('hide');
        if (isHidden)
            $("#hideSpan").text('view');
    });

    $("#saveSpan").click(function () {
        var text = $("#messageDiv").text();
        $("#saveDiv").text(text)
    });

    setTimeout(timer, 500);
    setInterval(timer, 1000 * 60 * 2);
    setInterval(viewers, 2000);
    
});

var guid = uuidv4();
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
        .replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0,
                v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
}

function viewers() {

    var data = { "value": "some value" };

    //alert('viewers');

    $.ajax({
        type: "GET",
        url: "/api/apiusers",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {
            var viewers = val.viewers;

            $('.viewersDiv').text(`viewers: ${viewers}`);
        }
    });
}

function timer() {

    var data = { "guid": "" + guid };

    //alert('timer');

    $.ajax({
        type: "POST",
        url: "/api/apiusers",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {
            var _val = val.guid;
            //alert(_val);
        }
    });
}


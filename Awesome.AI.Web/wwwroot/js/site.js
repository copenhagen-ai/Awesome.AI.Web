// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var isHidden = true;
var showinfo = false;
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

    $(".sm_info").click(function () {
        showinfo = !showinfo;

        $(".info").hide();
        if (showinfo) {
            $(".info").show();
        }
    });

    $(".sortSpan").click(function (event) { sort(); });
    $(".chatBtn").click(function (event) {
        chat($(".chatTxt").val());
    });
    $(".chatTxt").on("keydown", function () {
        if ( event.which == 13 ) {
            chat($(".chatTxt").val());               
        }
    });

    var isChart1 = true;
    $("#chartSpan").click(function (event) {
        if (isChart1) {
            isChart1 = !isChart1;
            $("#chartSpan").text('unit');

            $(".chart1").hide();
            $(".chart2").show();
        }
        else {
            isChart1 = !isChart1;
            $("#chartSpan").text('index');

            $(".chart2").hide();
            $(".chart1").show();
        }
    });

    //$(".infomain").hover(function () {
    //    $('.infosec').show();
    //}, function () {
    //    $('.infosec').hide();
    //});

    setTimeout(viewer, 500);
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

            $('.viewersDiv').text(`viewers today: ${viewers}`);
        }
    });
}

function viewer() {

    var data = { "value": "new user" };

    //alert('timer');

    $.ajax({
        type: "POST",
        url: "/api/apiusers",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {

            var _val = val.ok;
        }
    });
}

function sort() {
    $.ajax({
        type: "GET",
        url: "/api/sort",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {

            var text = $('.sortSpan').text();

            if (text == 'sort force')
                $('.sortSpan').text('sort index');
            else
                $('.sortSpan').text('sort force');            
        },
        error: function (val) {
            alert('ups, error');
        }
    });    
}

function chat(text) {

    var mind = $('#r1').text() == '[Roberta]' ? 'roberta' : 'andrew';
    var data = { "text": "" + text, "mind": "" + mind };

    var tmp = $('.chatRes').html().trim();
    
    if (tmp == '&gt;&gt; some text<br>') {
        tmp = '';
    }

    $('.chatRes').html(tmp + '>> ' + text + '<br>');

    $.ajax({
        type: "POST",
        url: "/api/apichat",
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {

            var res = val.res;
            var ok = val.ok;
            if (ok) {
                var tmp = '';

                tmp = res.replaceAll('user:', '');
                tmp = tmp.replaceAll('ass:', '');

                $('.chatRes').html(tmp);

                var div4 = document.getElementById("chatTitle");

                div4.classList.remove("i-color-red");                
            }
            else {
                var tmp = '';

                tmp = res.replaceAll('user:', '');
                tmp = tmp.replaceAll('ass:', '');

                $('.chatRes').html(tmp);

                alert('be patient, other users are asking questions..');
            }
        }
    });
}

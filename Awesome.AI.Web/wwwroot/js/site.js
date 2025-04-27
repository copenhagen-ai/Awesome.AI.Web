// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var isHidden = true;
var showinfo = false;
$(document).ready(function () {

    $('.moodinfo').click(function () {
        popup_mood();
    });
    
    $(".hideSpan").click(function () {
        isHidden = !isHidden;
        $(".hideSpan").text('hide');
        if (isHidden)
            $(".hideSpan").text('view');
    });

    $(".saveSpan").click(function () {
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

    $(".r1").click(function (event) {
        //alert('hep1');
        room1();
    });

    $(".r2").click(function (event) {
        //alert('hep2');
        room2();
    });

    //$(".infomain").hover(function () {
    //    $('.infosec').show();
    //}, function () {
    //    $('.infosec').hide();
    //});

    setTimeout(viewer, 500);
    setInterval(server_check, 2000);    
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

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function popup_no_btn(txt) {
    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: txt,
        btnTitle: '',
        themeColor: '#F97316',
        border: true
        //btnColor: '#7CFC00',
        //btnColor: true
    });

    $('#alertBoxBtn').hide();
}

function popup_with_btn(txt) {
    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: txt,
        btnTitle: 'OK',
        themeColor: '#F97316',
        border: true,
        btnColor: '#F97316',
        //btnColor: true
    });

    //$('#alertBoxBtn').hide();
}

function popup_mood() {
    
    var text = '<div class="moodtext1" style="text-align:left;">This is the current mood of the system. If the color is green, momentum is within limits, if not then red. it should not be red. ' +
        'Currently the mood is controlled by a MoodGenerator(changes every 10 seconds or so), this should be replaced with user input like: Pinch and Tickle. <br /> <br />' +
        'Momentum is normalized, like so: <br />' +
        'GENERAL limits: 0 -> 100 <br />' +
        'GOOD limits: 50 -> 100 <br />' +
        'BAD limits: 0 -> 50 <br /> <br /></div>' +

        '<div class="moodtext2 hidden" style="text-align:left;">This is the current mood of the system. If the color is green, momentum is within limits, if not then red. it should not be red. ' +
        'Currently the mood is controlled by a MoodGenerator(changes every 10 seconds or so), this should be replaced with user input like: Pinch and Tickle. <br /> <br />';

    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: text,
        btnTitle: 'OK',
        themeColor: '#60B2FD',
        border: true,
        btnColor: '#60B2FD',
    });

    const style = document.createElement('style');
    style.innerHTML =
        '@media (max-width: 768px) {' +
            '.sa-info {' +
                'display: none !important;' +
            '}' +
            '.moodtext1 {' +
                'display: none !important;' +
            '}' +
            '.moodtext2 {' +
                'display: block !important;' +
            '}' +
        '}';
    document.head.appendChild(style);
    //$('#alertBoxBtn').hide();
}

function server_check() {

    var data = { "value": "some value" };

    //alert('viewers');

    $.ajax({
        type: "GET",
        url: "/api/apiserverchecks",
        data: {},
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (val) {

            var viewers = val.viewers;
            server_running = val.server_running;

            $('.viewersDiv').text(`viewers today: ${viewers}`);
        }
    });
}

function viewer() {

    var data = { "value": "new user" };

    //alert('timer');

    $.ajax({
        type: "POST",
        url: "/api/apiserverchecks",
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

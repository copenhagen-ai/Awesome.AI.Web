// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var isHidden = true;
var showinfo = false;
var isMobile = false;
$(document).ready(function () {

    isMobile = /Mobi|Android|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);

    $('.refreshButton').click(function () {
        popup_welcome();        
    });

    $('.moodinfo').click(function () {
        popup_mood();
    });

    $('.chatinfo').click(function () {
        popup_chat();
    });

    $('.monologueinfo').click(function () {
        popup_monologue();
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
        alertIcon: 'success',
        title: 'INFO',
        message: txt,
        btnTitle: '',
        themeColor: '#22B445',//green
        border: true
        //btnColor: '#7CFC00',
        //btnColor: true
    });

    $('#alertBoxBtn').hide();
}

function popup_with_btn(txt) {
    alertbox.render({
        alertIcon: 'warning',
        title: 'INFO',
        message: txt,
        btnTitle: 'OK',
        themeColor: '#F97316',//orange
        border: true,
        btnColor: '#F97316',
        //btnColor: true
    });

    //$('#alertBoxBtn').hide();
}

function popup_mood() {

    var text = '<div class="moodtext1 float-left">This is the current mood of the system. If the color is green, momentum is within limits, if not then red. it should not be red. ' +
        'Currently the mood is controlled by a MoodGenerator(changes every 10 seconds or so), this should be replaced with user input like: Pinch and Tickle. <br /> <br />' +
        'Momentum is normalized, like so: <br />' +
        'GENERAL limits: 0 -> 100 <br />' +
        'GOOD limits: 50 -> 100 <br />' +
        'BAD limits: 0 -> 50 <br /> <br /></div>' +

        '<div class="moodtext2 hidden float-left">This is the current mood of the system. If the color is green, momentum is within limits, if not then red. it should not be red. ' +
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

function popup_chat() {

    var text = '<span class="">Chat is very buggy.</span>';

    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: text,
        btnTitle: 'OK',
        themeColor: '#60B2FD',
        border: true,
        btnColor: '#60B2FD',
    });
}

function popup_monologue() {

    var text = '<span class="">MONOLOGUE now connects to MOOD, instead of STATS.</span>';

    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: text,
        btnTitle: 'OK',
        themeColor: '#60B2FD',
        border: true,
        btnColor: '#60B2FD',
    });
}

function reload() {
    //location.reload();

    const url = new URL(window.location.href);
    url.searchParams.set('v', Date.now());
    window.location.replace(url.toString());
}

function popup_reload() {

    var text = '<span class="">Reloading page...</span>';

    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: text,
        btnTitle: '',
        themeColor: '#60B2FD',//blue
        border: true,
    });

    $('#alertBoxBtn').hide();

    setTimeout(reload, 2000);
}

function end_welcome() {
    //location.reload();

    $('#alertoverlay').hide();
    $('#alertbox').hide();

    reload();
}

function popup_welcome() {

    var text = '<span class="">Refreshing...</span>';

    alertbox.render({
        alertIcon: 'info',
        title: 'INFO',
        message: text,
        btnTitle: '',
        themeColor: '#60B2FD',//blue
        border: true,
    });

    $('#alertBoxBtn').hide();

    setTimeout(end_welcome, 2000);
}

var server_count = 0;
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

            server_count++;
            if (server_count > 10)
                server_count = 1;

            var viewers = val.viewers;
            server_running = val.server_running;

            if (server_running) {
                $('.servrun').addClass('text-green-500');
                $('.servrun').removeClass('text-red-500');
                $('.serverrunning').text(server_count + ']');
            }
            else {
                $('.servrun').addClass('text-red-500');
                $('.servrun').removeClass('text-green-500');
            }

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

            var _val = val.ip_is_registered;

            if (!_val) {
                popup_reload();
            }
            else {
                ;
            }
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

                //var div4 = document.getElementById("chatTitle");

                //div4.classList.remove("i-color-red");                
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

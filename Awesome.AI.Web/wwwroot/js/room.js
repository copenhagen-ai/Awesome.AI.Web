"use strict";

var room = 'room1';
var connection = new signalR.HubConnectionBuilder().withUrl("/roomhub").build();
const myChart1 = new Chart(document.getElementById('myChart1'), {
    type: "bar",
    data: {
        labels: [],
        datasets: [{
            label: 'Real-time Data',
            backgroundColor: [],
            data: []
        }]
    },
    options: {
        scales: {
            xAxes: [{
                ticks: {
                    //autoSkip: false,
                    maxRotation: 45,
                    minRotation: 20

                }
            }],
            yAxes: [{
                ticks: {
                    max: 30,
                    min: 0
                }
            }]
        }
    }
});

const myChart2 = new Chart(document.getElementById('myChart2'), {
    type: "bar",
    data: {
        labels: [],
        datasets: [{
            label: 'Real-time Data',
            backgroundColor: [],
            data: []
        }]
    },
    options: {
        scales: {
            xAxes: [{
                ticks: {
                    //autoSkip: false,
                    maxRotation: 45,
                    minRotation: 20

                }
            }],
            yAxes: [{
                ticks: {
                    max: 30,
                    min: 0
                }
            }]
        }
    }
});

$(document).ready(function () {

    $(".joinButton").click(function (event) {
        connection.start();
        event.preventDefault();

        onConnect();

        total = $('#totalSpan').text();
        temp = total;

        //if(startup)
        $("#overlay2").fadeIn(300);
        setTimeout(mycrashed, 3000);
        setTimeout(mystart, 5000);
    });    
});

var startup = true;
var total = '';
var temp = '';
var crashed = false;

function mystart() {
    $("#overlay2").fadeOut(100);
    //startup = false;
}

function mycrashed() {
    total = $('#totalSpan').text();

    if (temp == total) {
        //alert('CRASHED1');

        connection = new signalR.HubConnectionBuilder().withUrl("/roomhub").build();

        connection.start().then(function () {
            connection.invoke("Start").catch(function (err) {
                return console.error(err.toString());
            });
        });
        //event.preventDefault();

        onConnect();

        //alert('CRASHED2');
    }
}

function room1() {
    //alert('hello1');
    $("#overlay").fadeIn(300);
    setTimeout(timeout, 300);

    myChart1.data.labels = [];
    myChart1.data.datasets[0].label = 'Real-time Data';
    myChart1.data.datasets[0].data = [];
    myChart1.data.datasets[0].backgroundColor = [];

    myChart2.data.labels = [];
    myChart2.data.datasets[0].label = 'Real-time Data';
    myChart2.data.datasets[0].data = [];
    myChart2.data.datasets[0].backgroundColor = [];

    room = 'room1';
    $('.r1').text('[Roberta]');
    $('.r2').text('Andrew');
    $('.mechSpan').text('ballonhill');
    $('.roomHeader').text('Roberta');
    $('#messageDiv').text('inner monologue.. (press view)');
    $('#dot1Span').text('xxxx');
    $('#dot2Span').text('xxxx');

    $("#overlay").fadeOut(100);
}

function room2() {
    //alert('hello2');
    $("#overlay").fadeIn(300);
    setTimeout(timeout, 300);

    myChart1.data.labels = [];
    myChart1.data.datasets[0].label = 'Real-time Data';
    myChart1.data.datasets[0].data = [];
    myChart1.data.datasets[0].backgroundColor = [];

    myChart2.data.labels = [];
    myChart2.data.datasets[0].label = 'Real-time Data';
    myChart2.data.datasets[0].data = [];
    myChart2.data.datasets[0].backgroundColor = [];

    room = 'room2';
    $('.r1').text('Roberta');
    $('.r2').text('[Andrew]');
    $('.mechSpan').text('tugofwar');
    $('.roomHeader').text('Andrew');
    $('#messageDiv').text('inner monologue.. (press view)');
    $('#dot1Span').text('xxxx');
    $('#dot2Span').text('xxxx');
    

    $("#overlay").fadeOut(100);    
}

function timeout() {
    ;
}

function mychat1(ask) {

    //alert(ask);
    //var div1 = document.getElementById("chatRes");

    //div1.textContent = `${ask}`;

    var tmp = '';

    tmp = ask.replaceAll('user:', '');
    tmp = tmp.replaceAll('ass:', '');

    $('.chatRes').html(`${tmp}`);
}

function myinfo1(epochs, runtime, momentum, dmomentum, cycles, pain, position, ratio, going_down, chat_state) {

    //var div0 = document.getElementById("epochsSpan");
    var div1 = document.getElementById("epochsremainingSpan");
    var div2 = document.getElementById("momentumSpan");
    var div3 = document.getElementById("dmomentumSpan");
    var div4 = document.getElementById("cyclesSpan");
    var div5 = document.getElementById("totalSpan");
    var div6 = document.getElementById("positionSpan");
    var div7 = document.getElementById("ratioYesSpan");
    var div8 = document.getElementById("ratioNoSpan");
    var div9 = document.getElementById("theDownSpan");
    var div10 = document.getElementById("painSpan");
    var div11 = document.getElementById("chatTitle");

    // document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.

    var epochs_remaining = (runtime * 60) - epochs;
    //var pain_out = parseFloat(pain) > 1.0 ? 'OUCH' : `${pain}`;
    var pain_out = `${pain}`;

    //div0.textContent = `${epochs}`;
    div1.textContent = `${epochs_remaining}`;
    div2.textContent = `${momentum}`;
    div3.textContent = `${dmomentum}`;
    div4.textContent = `${cycles[0]}`;
    div5.textContent = `${cycles[1]}`;
    div6.textContent = `${position}`;
    div7.textContent = `${ratio[0]}`;
    div8.textContent = `${ratio[1]}`;
    div9.textContent = `${going_down}`;
    div10.textContent = `${pain_out}`;

    if (going_down == 'NO') {
        div9.classList.remove("i-color-red");
        div9.classList.add("i-color-green");
    }
    else {
        div9.classList.add("i-color-red");
        div9.classList.remove("i-color-green");
    }

    if (parseFloat(pain) > 1.0) {
        div10.classList.add("i-color-red");
    }
    else {
        div10.classList.remove("i-color-red");
    }

    if (epochs_remaining <= 2) {
        div1.classList.add("i-color-red");
    }
    else {
        div1.classList.remove("i-color-red");
    }

    if (chat_state == 'thinking') {
        div11.classList.add("i-color-red");
    }
    else {
        div11.classList.remove("i-color-red");
    }
}


function myinfo2(whistle, occu, location, loc_state) {

    var div1 = document.getElementById("occuSpan");
    var div2a = document.getElementById("locationSpan1");
    var div2b = document.getElementById("locationSpan2");
    var div3a = document.getElementById("stateSpan1");
    var div3b = document.getElementById("stateSpan2");
    var div4 = document.getElementById("quickSpan");

    // document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.

    div1.textContent = `${occu}`;
    div2b.textContent = `${location}`;
    div3b.textContent = `${loc_state}`;
    div4.textContent = `${whistle}`;

    if (whistle.indexOf("?") >= 0) {
        div4.classList.remove("i-color-green");
    }
    else {
        div4.classList.add("i-color-green");
    }

    if (loc_state == 'making a decision') {
        div2a.classList.add("i-color-green");
        div3a.classList.add("i-color-green");
    }
    else {
        div2a.classList.remove("i-color-green");
        div3a.classList.remove("i-color-green");
    }

}

function mymood(mood, moodOK) {

    $("#moodSpan").text(`${mood.replace("MOOD", "") }`);
    
    if (moodOK) {
        $("#moodSpan").addClass("i-color-green");
        $("#moodSpan").removeClass("i-color-red");
    }
    else {
        $("#moodSpan").addClass("i-color-red");
        $("#moodSpan").removeClass("i-color-green");
    }
}

function mymessage(message, dot1, dot2, subject) {
    var div1 = document.getElementById("messageDiv");
    var div2 = document.getElementById("dot1Span");
    var div3 = document.getElementById("dot2Span");
    var div4 = document.getElementById("subjectDiv");

    var messageX = `${message}`.replace(/[^ ]/g, 'x');

    // document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    div1.innerHTML = isHidden ? messageX : `${message}`;
    div2.textContent = `${dot1}`;
    div3.textContent = `${dot2}`;
    div4.textContent = `${subject}`;
}


function onConnect() {
    connection.on("MIND1ChatReceive1", function (ask) {
        //alert('hello1: ' + ask);
        if (room == 'room1')
            mychat1(ask);
    });

    connection.on("MIND2ChatReceive1", function (ask) {
        //alert('hello2: ' + ask);
        if (room == 'room2')
            mychat1(ask);
    });



    connection.on("MIND1InfoReceive1", function (epochs, runtime, momentum, dmomentum, cycles, pain, position, ratio, going_down, chat_state) {

        if (room == 'room1')
            myinfo1(epochs, runtime, momentum, dmomentum, cycles, pain, position, ratio, going_down, chat_state);
    });

    connection.on("MIND2InfoReceive1", function (epochs, runtime, momentum, dmomentum, cycles, pain, position, ratio, going_down, chat_state) {

        if (room == 'room2')
            myinfo1(epochs, runtime, momentum, dmomentum, cycles, pain, position, ratio, going_down, chat_state);
    });

    connection.on("MIND1DecisionReceive1", function (whistle, occu, location, loc_state) {

        if (room == 'room1')
            myinfo2(whistle, occu, location, loc_state);
    });

    connection.on("MIND2DecisionReceive1", function (whistle, occu, location, loc_state) {

        if (room == 'room2')
            myinfo2(whistle, occu, location, loc_state);
    });


    connection.on("MIND1MoodReceive1", function (mood, moodOK) {

        if (room == 'room1')
            mymood(mood, moodOK);
    });

    connection.on("MIND2MoodReceive1", function (whistle, occu, location, loc_state) {

        if (room == 'room2')
            mymood(mood, moodOK);
    });



    connection.on("MIND1MessageReceive", function (message, dot1, dot2, subject) {
    
        if (room == 'room1')
            mymessage(message, dot1, dot2, subject);
    });

    connection.on("MIND2MessageReceive", function (message, dot1, dot2, subject) {
    
        if (room == 'room2')
            mymessage(message, dot1, dot2, subject);
    });



    connection.on("MIND1GraphReceive", function (_labs, _lab, _value, _lab_reset, _value_reset, _col) {

        if (room == 'room1')
            mygraph1(_labs, _lab, _value, _lab_reset, _value_reset, _col);


    });

    connection.on("MIND2GraphReceive", function (_labs, _lab, _value, _lab_reset, _value_reset, _col) {

        if (room == 'room2')
            mygraph1(_labs, _lab, _value, _lab_reset, _value_reset, _col);


    });

    connection.on("MIND3GraphReceive", function (_labs, _lab, _value, _lab_reset, _value_reset, _col) {

        if (room == 'room1')
            mygraph2(_labs, _lab, _value, _lab_reset, _value_reset, _col);


    });

    connection.on("MIND4GraphReceive", function (_labs, _lab, _value, _lab_reset, _value_reset, _col) {

        if (room == 'room2')
            mygraph2(_labs, _lab, _value, _lab_reset, _value_reset, _col);


    });

}

function mygraph1(_labs, _lab, _value, _lab_reset, _value_reset, _col) {

    //alert('graph1' + ' ' + _lab + ' ' + _value + ' ' + _lab_reset + ' ' + _value_reset);

    var lab = `${_lab}`;
    var value = _value;
    var lab_reset = `${_lab_reset}`;
    var value_reset = _value_reset;
    var color = `${_col}`;

    var count = 0;
    myChart1.data.labels.forEach((_l) => {
        count++;
    });

    if (count == 0) {
        _labs.forEach((_l) => {
            myChart1.data.labels.push(_l);
            myChart1.data.datasets[0].data.push(0);
            myChart1.data.datasets[0].backgroundColor.push('orange');
            count++;
        });
    }

    var index = 0;
    myChart1.data.labels.forEach((_l) => {
        if (`${_l}` == lab)
            myChart1.data.datasets[0].data[index] = value;
        if (`${_l}` == lab_reset)
            myChart1.data.datasets[0].data[index] = value_reset;
        index++;
    });

    myChart1.update();

    //alert('graph');
}

function mygraph2(_labs, _lab, _value, _lab_reset, _value_reset, _col) {

    //alert('graph2');

    var lab = `${_lab}`;
    var value = _value;
    var lab_reset = `${_lab_reset}`;
    var value_reset = _value_reset;
    var color = `${_col}`;

    var count = 0;
    myChart2.data.labels.forEach((_l) => {
        count++;
    });

    if (count == 0) {
        _labs.forEach((_l) => {
            myChart2.data.labels.push(_l);
            myChart2.data.datasets[0].data.push(0);
            myChart2.data.datasets[0].backgroundColor.push('orange');
            count++;
        });
    }

    var index = 0;
    myChart2.data.labels.forEach((_l) => {
        if (`${_l}` == lab)
            myChart2.data.datasets[0].data[index] = value;
        if (`${_l}` == lab_reset)
            myChart2.data.datasets[0].data[index] = value_reset;
        index++;
    });

    myChart2.update();

    //alert('graph');
}


//Disable the send button until connection is established.
//document.getElementById("joinButton").disabled = true;

//connection.start().then(function () {
//    document.getElementById("joinButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("joinButton").addEventListener("click", function (event) {

//    //connection.invoke("Start").catch(function (err) {
//    //    return console.error(err.toString());
//    //});
//    event.preventDefault();
//});
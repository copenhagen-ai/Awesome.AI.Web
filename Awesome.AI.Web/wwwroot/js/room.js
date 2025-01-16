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
                    max: 20,
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
                    max: 20,
                    min: 0
                }
            }]
        }
    }
});

$(document).ready(function () {

    $("#joinButton").click(function (event) {
        connection.start();
        event.preventDefault();

        onConnect();

        total = $('#totalSpan').text();
        temp = total;

        setTimeout(mycrashed, 3000);
    });    
});

var total = '';
var temp = '';
var crashed = false;
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
    $('#r1').text('[Roberta]');
    $('#r2').text('Andrew');
    $('.roomHeader').text('Roberta');
    $('#messageDiv').text('inner monologue..');
    $('#dot1Span').text('xxxx');
    $('#dot2Span').text('xxxx');

    $("#overlay").fadeOut(100);
}

function room2() {

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
    $('#r1').text('Roberta');
    $('#r2').text('[Andrew]');
    $('.roomHeader').text('Andrew');
    $('#messageDiv').text('inner monologue..');
    $('#dot1Span').text('xxxx');
    $('#dot2Span').text('xxxx');
    

    $("#overlay").fadeOut(100);    
}

function timeout() {
    ;
}

function myinfo(epochs, runtime, momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise, occu) {

    //var div0 = document.getElementById("epochsSpan");
    var div1 = document.getElementById("epochsremainingSpan");
    var div2 = document.getElementById("momentumDiv");
    var div3 = document.getElementById("cyclesSpan");
    var div4 = document.getElementById("totalSpan");
    var div5 = document.getElementById("positionDiv");
    var div6 = document.getElementById("ratioYesSpan");
    var div7 = document.getElementById("ratioNoSpan");
    var div8 = document.getElementById("theChoiceDiv");
    var div9 = document.getElementById("occuDiv");
    var div10 = document.getElementById("painDiv");

    // document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.

    var epochs_remaining = (runtime * 60) - epochs;
    var pain_out = parseFloat(pain) > 1.0 ? 'OUCH' : `${pain}`;
    
    //div0.textContent = `${epochs}`;
    div1.textContent = `${epochs_remaining}`;
    div2.textContent = `${momentum}`;
    div3.textContent = `${cycles[0]}`;
    div4.textContent = `${cycles[1]}`;
    div5.textContent = `${position}`;
    div6.textContent = `${ratio_yes}`;
    div7.textContent = `${ratio_no}`;
    div8.textContent = `${the_choise}`;
    div9.textContent = `${occu}`;
    div10.textContent = `${pain_out}`;

    if (the_choise == 'NO') {
        div8.classList.remove("i-color-red");
        div8.classList.add("i-color-green");
    }
    else {
        div8.classList.add("i-color-red");
        div8.classList.remove("i-color-green");
    }

    if (parseFloat(pain) > 1.0) {
        div9.classList.add("i-color-red");
    }
    else {
        div9.classList.remove("i-color-red");
    }

    if (epochs_remaining <= 2) {
        div1.classList.add("i-color-red");
    }
    else {
        div1.classList.remove("i-color-red");
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
    connection.on("MIND1InfoReceive", function (epochs, runtime, momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise, occu) {

        if (room == 'room1')
            myinfo(epochs, runtime, momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise, occu, viewers);
    });

    connection.on("MIND2InfoReceive", function (epochs, runtime, momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise, occu) {

        if (room == 'room2')
            myinfo(epochs, runtime, momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise, occu, viewers);
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
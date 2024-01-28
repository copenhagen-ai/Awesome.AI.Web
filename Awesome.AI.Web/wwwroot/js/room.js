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



function myinfo(momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg) {

    var is_no = the_choise_isno == "True";

    var div1 = document.getElementById("momentumDiv");
    var div2 = document.getElementById("cyclesSpan");
    var div3 = document.getElementById("totalSpan");

    var div4 = document.getElementById("positionDiv");
    var div5 = document.getElementById("ratioYesSpan");
    var div6 = document.getElementById("ratioNoSpan");
    var div7 = document.getElementById("theChoiceDiv");

    var div8 = document.getElementById("biasDiv");
    var div9 = document.getElementById("limitDiv");
    var div10 = document.getElementById("limAvgDiv");
    var div11 = document.getElementById("painDiv");

    //document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    div1.textContent = `${momentum}`;
    div2.textContent = `${cycles[0]}`;
    div3.textContent = `${cycles[1]}`;

    div4.textContent = `${position}`;
    div5.textContent = `${ratio_yes}`;
    div6.textContent = `${ratio_no}`;
    div7.textContent = is_no ? `NO` : `YES`;
    if (is_no) {
        div7.classList.remove("i-color-red");
        div7.classList.add("i-color-green");
    }
    else {
        div7.classList.add("i-color-red");
        div7.classList.remove("i-color-green");
    }

    div8.textContent = `${bias}`;
    div9.textContent = `${limit}`;
    div10.textContent = `${limit_avg}`;
    div11.textContent = `${pain}`;
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
    connection.on("MIND1InfoReceive", function (momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg) {

        if (room == 'room1')
            myinfo(momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg, viewers);
    });

    connection.on("MIND2InfoReceive", function (momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg) {

        if (room == 'room2')
            myinfo(momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg, viewers);
    });

    connection.on("MIND1MessageReceive", function (message, dot1, dot2, subject) {
    
        if (room == 'room1')
            mymessage(message, dot1, dot2, subject);
    });

    connection.on("MIND2MessageReceive", function (message, dot1, dot2, subject) {
    
        if (room == 'room2')
            mymessage(message, dot1, dot2, subject);
    });

    connection.on("MIND1GraphReceive", function (_labs, _curr, _value, _curr_reset, _value_reset, _col) {

        if (room == 'room1')
            mygraph1(_labs, _curr, _value, _curr_reset, _value_reset, _col);


    });

    connection.on("MIND2GraphReceive", function (_labs, _curr, _value, _curr_reset, _value_reset, _col) {

        if (room == 'room2')
            mygraph1(_labs, _curr, _value, _curr_reset, _value_reset, _col);


    });

    connection.on("MIND3GraphReceive", function (_labs, _curr, _value, _curr_reset, _value_reset, _col) {

        if (room == 'room1')
            mygraph2(_labs, _curr, _value, _curr_reset, _value_reset, _col);


    });

    connection.on("MIND4GraphReceive", function (_labs, _curr, _value, _curr_reset, _value_reset, _col) {

        if (room == 'room2')
            mygraph2(_labs, _curr, _value, _curr_reset, _value_reset, _col);


    });

}

function mygraph1(_labs, _curr, _value, _curr_reset, _value_reset, _col) {

    //alert('graph1');

    var curr = `${_curr}`;
    var value = _value;
    var curr_reset = `${_curr_reset}`;
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
        if (`${_l}` == curr)
            myChart1.data.datasets[0].data[index] = value;
        if (`${_l}` == curr_reset)
            myChart1.data.datasets[0].data[index] = value_reset;
        index++;
    });

    myChart1.update();

    //alert('graph');
}

function mygraph2(_labs, _curr, _value, _curr_reset, _value_reset, _col) {

    //alert('graph1');

    var curr = `${_curr}`;
    var value = _value;
    var curr_reset = `${_curr_reset}`;
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
        if (`${_l}` == curr)
            myChart2.data.datasets[0].data[index] = value;
        if (`${_l}` == curr_reset)
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
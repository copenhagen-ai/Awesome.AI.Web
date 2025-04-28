"use strict";

var room = 'room1';
var connection = new signalR.HubConnectionBuilder().withUrl("/roomhub").build();
connection.start().then(function () {
    is_busy = false;
}).catch(function (err) {
    return console.error(err.toString());
});

const mylbls1 = ['label1', 'label2', 'label3', 'label4', 'label5', 'label6', 'label7', 'label8', 'label9', 'label10'];
const mylbls2 = ['label1', 'label2', 'label3', 'label4', 'label5', 'label6', 'label7', 'label8', 'label9', 'label10'];

const myChart1 = new Chart(document.getElementById('myChart1'), {
    type: "bar",
    data: {
        labels: mylbls1,
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
        labels: mylbls2,
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

const mylabels1 = ['l1', 'l2', 'l3', 'l4', 'l5', 'l6', 'l7', 'l8', 'l9', 'l10'];
const mylabels2 = ['l1', 'l2', 'l3', 'l4', 'l5', 'l6', 'l7', 'l8', 'l9', 'l10'];

const config1 = new Chart(document.getElementById('chartmood1'), {
    type: 'line',
    data: {
        labels: mylabels1,
        datasets: [
            {
                label: 'Momentum (normalized 10->90)',
                data: [50, 50, 50, 50, 50, 50, 50, 50, 50, 50],
                fill: false,
                borderColor: '#15803D',
                tension: 0.1
            },
            {
                label: 'Noise (normalized 10->90)',
                data: [40, 40, 40, 40, 40, 40, 40, 40, 40, 40],
                fill: false,
                borderColor: '#303030',
                tension: 0.1
            }]
    },
    options: {
        scales: {
            yAxes: [{
                position: 'right', // <-- this moves the y-axis to the right
                ticks: {
                    max: 100,
                    min: 0
                }
            }]
        }
    }
});

const config2 = new Chart(document.getElementById('chartmood2'), {
    type: 'line',
    data: {
        labels: mylabels2,
        datasets: [
            {
                label: 'Momentum (normalized 10->90)',
                data: [50, 50, 50, 50, 50, 50, 50, 50, 50, 50],
                fill: false,
                borderColor: '#15803D',
                tension: 0.1
            },
            {
                label: 'Noise (normalized 10->90)',
                data: [40, 40, 40, 40, 40, 40, 40, 40, 40, 40],
                fill: false,
                borderColor: '#303030',
                tension: 0.1
            }]
    },
    options: {
        scales: {
            yAxes: [{
                position: 'right', // <-- this moves the y-axis to the right
                ticks: {
                    max: 100,
                    min: 0
                }
            }]
        }        
    }
});



//document.getElementById("joinButtonID").addEventListener("click", function (event) {

//    //alert('server_running: ' + server_running);

//    if (is_busy)
//        return;

//    if (is_running)
//        return;

//    is_busy = true;
//    first_run = true;

//    popup();

//    connection.invoke("Start").catch(function (err) {
//        return console.error(err.toString());
//    });

//    setTimeout(mypopup, 10000);

//    event.preventDefault();

//    onConnect();
//});

$(document).ready(function () {

    //mytimer();
    setInterval(mytimer, 5000);

    $(".joinButton").click(function (event) {

        //alert('server_running: ' + server_running);

        if (is_busy)
            return;

        if (is_running) {
            popup_with_btn('Already running..');
            return;
        }

        is_busy = true;
        first_run = true;
        
        popup_no_btn('Starting app..');
        setTimeout(close_popup_no_btn, 10000);

        connection.invoke("Start").catch(function (err) {
            popup_with_btn('Sorry, something went wrong.');
            return console.error(err.toString());
        });

        event.preventDefault();

        if (first_load)
            setTimeout(onConnect, 2000);

        first_load = false;

        myChart1.data.labels = [];
        myChart1.data.datasets[0].label = 'Real-time Data';
        myChart1.data.datasets[0].data = [];
        myChart1.data.datasets[0].backgroundColor = [];

        myChart2.data.labels = [];
        myChart2.data.datasets[0].label = 'Real-time Data';
        myChart2.data.datasets[0].data = [];
        myChart2.data.datasets[0].backgroundColor = [];

        //connection.start();
        //setTimeout(mycrashed, 3000);
    });    
});

var server_running = false;

var is_busy = false;
var is_running = false;
var first_run = false;
var first_load = true;
var total_curr = 'xxxx';
var total_prev = 'xxxx';

function mytimer() {

    total_prev = total_curr;
    total_curr = $('#totalSpan').text();
    is_running = total_curr != total_prev;
}

function close_popup_no_btn() {

    $('#alertoverlay').hide();
    $('#alertbox').hide();
    
    is_busy = false;
    first_run = false;
}

//function mycrashed() {
    
//    if (is_running)
//        return;

//    if (server_running)
//        return;

//    connection = new signalR.HubConnectionBuilder().withUrl("/roomhub").build();

//    connection.start().then(function () {
//        connection.invoke("Start").catch(function (err) {
//            return console.error(err.toString());
//        });
//    });
//    //event.preventDefault();

//    onConnect();    
//}

function room1() {
    
    $("#overlay").fadeIn(300);
    
    myChart1.data.labels = mylbls1;
    myChart1.data.datasets[0].label = 'Real-time Data';
    myChart1.data.datasets[0].data = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1];
    myChart1.data.datasets[0].backgroundColor = [];

    myChart2.data.labels = mylbls2;
    myChart2.data.datasets[0].label = 'Real-time Data';
    myChart2.data.datasets[0].data = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1];
    myChart2.data.datasets[0].backgroundColor = [];

    room = 'room1';
    $('.r1').text('[Roberta]');
    $('.r2').text('Andrew');
    $('.mechSpan').text('ballonhill');
    $('.roomHeader').text('Roberta');
    $('#messageDiv').text('inner monologue.. (press view)');
    $('#dot1Span').text('xxxx');
    $('#dot2Span').text('xxxx');

    $(".mychartmood2").hide();
    $(".mychartmood1").show();

    $("#overlay").fadeOut(100);
}

function room2() {
    
    $("#overlay").fadeIn(300);

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

    $(".mychartmood1").hide();
    $(".mychartmood2").show();

    $("#overlay").fadeOut(100);    
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
    var div9 = document.getElementById("noiseYesSpan");
    var div10 = document.getElementById("noiseNoSpan");
    var div11 = document.getElementById("theDownSpan");
    var div12 = document.getElementById("painSpan");
    var div13 = document.getElementById("chatTitle");

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
    div9.textContent = `${ratio[2]}`;
    div10.textContent = `${ratio[3]}`;
    div11.textContent = `${going_down}`;
    div12.textContent = `${pain_out}`;

    if (going_down == 'NO') {
        div11.classList.remove("text-red-500");
        div11.classList.add("text-green-500");
    }
    else {
        div11.classList.add("text-red-500");
        div11.classList.remove("text-green-500");
    }

    if (parseFloat(pain) > 1.0) {
        div12.classList.add("text-red-500");
    }
    else {
        div12.classList.remove("text-red-500");
    }

    if (epochs_remaining <= 2) {
        div1.classList.add("text-red-500");
    }
    else {
        div1.classList.remove("text-red-500");
    }

    //if (chat_state == 'thinking') {
    //    div12.classList.add("i-color-red");
    //}
    //else {
    //    div12.classList.remove("i-color-red");
    //}
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
        div4.classList.remove("text-green-500");
    }
    else {
        div4.classList.add("text-green-500");
    }

    if (loc_state == 'making a decision') {
        div2a.classList.add("text-green-500");
        div3a.classList.add("text-green-500");
    }
    else {
        div2a.classList.remove("text-green-500");
        div3a.classList.remove("text-green-500");
    }

}

function mymood1(mood, moodOK, mom, noise) {

    ///alert(mood + ' ' + moodOK);

    $("#moodSpan").text(`${mood.replace("MOOD", "")}`);

    if (moodOK) {
        $("#moodSpan").addClass("text-green-500");
        $("#moodSpan").removeClass("text-red-500");
    }
    else {
        $("#moodSpan").addClass("text-red-500");
        $("#moodSpan").removeClass("text-green-500");
    }

    mymoodgraph1(mom, noise);
}

function mymood2(mood, moodOK, mom, noise) {

    ///alert(mood + ' ' + moodOK);

    $("#moodSpan").text(`${mood.replace("MOOD", "")}`);

    if (moodOK) {
        $("#moodSpan").addClass("text-green-500");
        $("#moodSpan").removeClass("text-red-500");
    }
    else {
        $("#moodSpan").addClass("text-red-500");
        $("#moodSpan").removeClass("text-green-500");
    }

    mymoodgraph2(mom, noise);
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


    connection.on("MIND1MoodReceive1", function (mood, moodOK, mom, noise) {

        if (room == 'room1')
            mymood1(mood, moodOK, mom, noise);
    });

    connection.on("MIND2MoodReceive1", function (mood, moodOK, mom, noise) {

        if (room == 'room2')
            mymood2(mood, moodOK, mom, noise);
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
            myChart1.data.datasets[0].backgroundColor.push('#15803D');
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
            myChart2.data.datasets[0].backgroundColor.push('#15803D');
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

var data11 = [];
var data12 = [];
function mymoodgraph1(mom, noise) {

    var labs = ['l1', 'l2', 'l3', 'l4', 'l5', 'l6', 'l7', 'l8', 'l9', 'l10'];

    if (first_run) {
        config1.data.datasets[0].data = [50, 50, 50, 50, 50, 50, 50, 50, 50, 50];
        config1.data.datasets[1].data = [40, 40, 40, 40, 40, 40, 40, 40, 40, 40];
    }

    data11 = [];
    data12 = [];
    var data_tmp1 = config1.data.datasets[0].data;
    var data_tmp2 = config1.data.datasets[1].data;

    var i = 1;
    for (; i < 10; i++) {
        data11.push(data_tmp1[i]);
        data12.push(data_tmp2[i]);
    }

    data11.push(mom);
    data12.push(noise);

    config1.data.labels = labs;
    config1.data.datasets[0].data = data11;
    config1.data.datasets[1].data = data12;

    config1.update();
    //alert('hep1');
}

var data21 = [];
var data22 = [];
function mymoodgraph2(mom, noise) {

    var labs = ['l1', 'l2', 'l3', 'l4', 'l5', 'l6', 'l7', 'l8', 'l9', 'l10'];

    if (first_run) {
        config2.data.datasets[0].data = [50, 50, 50, 50, 50, 50, 50, 50, 50, 50];
        config2.data.datasets[1].data = [40, 40, 40, 40, 40, 40, 40, 40, 40, 40];
    }

    data21 = [];
    data22 = [];
    var data_tmp1 = config2.data.datasets[0].data;
    var data_tmp2 = config2.data.datasets[1].data;

    var i = 1;
    for (; i < 10; i++) {
        data21.push(data_tmp1[i]);
        data22.push(data_tmp2[i]);
        //data2.push(getRandomInt(0, 100));
    }

    data21.push(mom);
    data22.push(noise);

    config2.data.labels = labs;
    config2.data.datasets[0].data = data21;
    config2.data.datasets[1].data = data22;

    config2.update();
    //alert('hep2: ' + mom);
}


////Disable the send button until connection is established.
//document.getElementById("joinButton").disabled = true;

//connection.start().then(function () {
//    document.getElementById("joinButton").disabled = false;
//}).catch(function (err) {
//    return console.error(err.toString());
//});

//document.getElementById("joinButton").addEventListener("click", function (event) {

//    connection.invoke("Start").catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});
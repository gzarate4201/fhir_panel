"use strict";

var connection = new signalR.HubConnectionBuilder()
        .withUrl("/note")
        .build();

Object.defineProperty(WebSocket, 'OPEN', { value: 1, });

connection.start().then(function () {
    console.log("Connected");
}).catch(function (err) {
    return console.error(err.toString());
});
        

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var mensaje = JSON.parse(message);

    console.log(mensaje);
    
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    // document.getElementById("mqtt_receive").html(msg);
    console.log('Mensaje recibido por Hub' + encodedMsg);
});





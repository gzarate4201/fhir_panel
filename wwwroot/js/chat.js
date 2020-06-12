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

    console.log("Mensaje recibido por el Hub");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    mensaje = JSON.parse(message);

    // Determina los valores para conexion de las otras tramas
    if(mensaje.msg=="mqtt bind ctrl success") {
        tag = mensaje.tag;
        $("#actual_device_tag").html(tag);
        device_id = mensaje.device_id;
        $("#actual_device_id").html(device_id);
        device_tkn = mensaje.datas.device_token;

        // Trae todos los parametros de configuracion del dispositivo enlazado
        getParameters();
        showActualParameters(mensaje);
    }
    console.log('Mensaje del Hub\n');
    console.log(mensaje);
    
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    // document.getElementById("mqtt_receive").html(msg);
    console.log('Mensaje recibido por Hub' + encodedMsg);
});

function showActualParameters(mensaje) {
    $("#actual_device_tag").html(mensaje.tag);
    $("#actual_device_time").html(mensaje.dev_cur_pts);
}





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
    if (mensaje.msg =="get param success") {
        tag = mensaje.tag;
        $("#actual_device_tag").html(tag);
        device_id = mensaje.device_id;
        $("#actual_device_id").html(device_id);
        //device_tkn = mensaje.datas.device_token;

        // Trae todos los parametros de configuracion del dispositivo enlazado
        //getParameters();
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
    $("#actual_device_name").html(mensaje.datas.basic_parameters.dev_name);
    console.log(mensaje.datas.basic_parameters.dev_name);
    $("#actual_device_pass").html(mensaje.datas.basic_parameters.dev_pwd);
    $("#actual_version").html(mensaje.datas.version_info.firmware_ver);
    $("#actual_firm_date").html(mensaje.datas.version_info.firmware_date);
    $("#actual_temp_en").html(mensaje.datas.fun_param.temp_dec_en);
    $("#actual_face_num").html(mensaje.datas.face_recognition_cfg.dec_face_num_cur);
    $("#actual_stranger_en").html(mensaje.datas.fun_param.stranger_pass_en);
    $("#actual_face_min").html(mensaje.datas.face_recognition_cfg.dec_face_num_min);
    $("#actual_make_en").html(mensaje.datas.fun_param.make_check_en);
    $("#actual_face_max").html(mensaje.datas.face_recognition_cfg.dec_face_num_max);
    $("#actual_alarm_temp").html(mensaje.datas.fun_param.alarm_temp);
    $("#actual_dec_interval").html(mensaje.datas.face_recognition_cfg.dec_interval_cur);
    $("#actual_temp_comp").html(mensaje.datas.fun_param.temp_comp);
    $("#actual_dec_min").html(mensaje.datas.face_recognition_cfg.dec_interval_min);
    $("#actual_record_time").html(mensaje.datas.fun_param.record_save_time);
    $("#actual_dec_max").html(mensaje.datas.face_recognition_cfg.dec_interval_max);
    $("#actual_save_record").html(mensaje.datas.fun_param.save_record);
    $("#actual_ip").html(mensaje.datas.network_config.ip_addr);
    $("#actual_mqtt").html(mensaje.datas.mqtt_protocol_set.enable);
    $("#actual_mask").html(mensaje.datas.network_config.net_mask);
    $("#actual_retain").html(mensaje.datas.mqtt_protocol_set.retain);
    $("#actual_gw").html(mensaje.datas.network_config.gateway);
    $("#actual_pqos").html(mensaje.datas.mqtt_protocol_set.pqos);
    $("#actual_ddns1").html(mensaje.datas.network_config.DDNS1);
    $("#actual_sqos").html(mensaje.datas.mqtt_protocol_set.sqos);
    $("#actual_ddns2").html(mensaje.datas.network_config.DDNS2);
    $("#actual_server").html(mensaje.datas.mqtt_protocol_set.server);
    $("#actual_dhcp").html(mensaje.datas.network_config.DHCP);
    $("#actual_username").html(mensaje.datas.mqtt_protocol_set.username);
    $("#actual_mqtt_password").html(mensaje.datas.mqtt_protocol_set.passwd);
    $("#actual_Topic2Subscribe").html(mensaje.datas.mqtt_protocol_set.topic2subscribe);
    $("#actual_Topic2Publish").html(mensaje.datas.mqtt_protocol_set.topic2publish);
    $("#actual_heartbeat").html(mensaje.datas.mqtt_protocol_set.heartbeat);

}





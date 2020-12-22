function bindMqtt() {
    var codigo = $('#bind_codigo').val();
    var descripcion = $('#bind_descripcion').val();
    console.log(codigo, descripcion);
    jsonMsg =
    {
        mqtt_cmd: 1,
        mqtt_operate_id: 0,
        device_id: $('#bind_codigo').val(),
        tag: dev_tag,
        bind_ctrl: 1
    };
    jsonMsg = JSON.stringify(jsonMsg);

    console.log(jsonMsg);
    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Empresa")',
        dataType: "json",
        data: {
            topic: "SubscribeTest",
            msg: jsonMsg,
        },
        async: false,
        success: function (data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function () {
            console.log("Error Ajax");
        }
    });
}



function unbindMqtt() {
    var codigo = $('#bind_codigod').val();
    var descripcion = $('#bind_descripcion').val();
    console.log(dev_id, dev_tag);

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Empresa")',
        dataType: "json",
        data: {
            topic: "SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 0,
                    codigo: codigo,
                    descripcion: descripcion,
                    bind_ctrl: 0
                }
            ),
        },
        async: false,
        success: function (data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function () {
            console.log("Error Ajax");
        }
    });
}


function deviceConfig() {
    var codigo = $('#bind_codigo').val();
    var descripcion = $('#bind_descripcion').val();
    var sync = $('#sync_server').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Empresa")',
        dataType: "json",
        data: {
            topic: "SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 1,
                    codigo: codigo,
                    descripcion: descripcion,
                    basic_config:
                    {
                        sync_server_pts_en: sync,
                        server_cur_pts: time
                    }
                }
            ),
        },
        async: false,
        success: function (data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function () {
            console.log("Error Ajax");
        }
    });
}



function networkConfig() {
    var codigo = $('#bind_codigo').val();
    var descripcion = $('#bind_descripcion').val();
    var ip = $('#ip').val();
    var mask = $('#mask').val();
    var gw = $('#gw').val();
    var dns1 = $('#dns1').val();
    var dns2 = $('#dns2').val();
    var dhcp = $('#dhcp').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Empresa")',
        dataType: "json",
        data: {
            topic: "SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 2,
                    codigo: codigo,
                    descripcion: descripcion,
                    network_config:
                    {
                        ip_addr: ip,
                        net_mask: mask,
                        gateway: gw,
                        DDNS1: dns1,
                        DDNS2: dns2,
                        DHCP: dhcp
                    }
                }
            ),
        },
        async: false,
        success: function (data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function () {
            console.log("Error Ajax");
        }
    });
}



function remoteConfig() {
    var codigo = $('#bind_codigo').val();
    var dev_descripcion = $('#bind_descripcion').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Empresa")',
        dataType: "json",
        data: {
            topic: "SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 4,
                    codigo: codigo,
                    descripcion: descripcion,
                }
            ),
        },
        async: false,
        success: function (data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function () {
            console.log("Error Ajax");
        }
    });
}






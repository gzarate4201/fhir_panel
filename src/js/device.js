function bindMqtt() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    console.log(dev_id, dev_tag);
    jsonMsg = 
        {
            mqtt_cmd: 1,
            mqtt_operate_id: 0,
            device_id:  $('#bind_device_id').val(),
            tag: dev_tag,
            bind_ctrl: 1
        };
    jsonMsg = JSON.stringify(jsonMsg);
    
    console.log(jsonMsg);
    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: jsonMsg,
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* unbinding example
    {
        "mqtt_cmd":1,
        "mqtt_operate_id":10,
        "device_token":"1514348271",
        "device_id":"7101389947744",
        "tag":"platfrom define",
        "bind_ctrl":0
    }
*/

function unbindMqtt() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    console.log(dev_id, dev_tag);

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 0,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    bind_ctrl: 0
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* device basic configuration example
 {
    "mqtt_cmd":1,
    "mqtt_operate_id":1,
    "device_token":"1057628122",
    "device_id":"7101389947744",
    "tag":"platfrom define",
    "basic_config":
    {
        "dev_name":"12315456445864",
        "dev_pwd":"YWRtaW4=",
        "sync_server_pts_en":true,
        "server_cur_pts":"2020/05/18 11:07"
    }
}

*/

function deviceConfig() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var name = $('#device_name').val();
    var pass = $('#device_pass').val();
    var time = $('#time_server').val();
    var sync = $('#sync_server').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 1,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    basic_config:
                    {
                        dev_name :name,
                        dev_pwd : pass,
                        sync_server_pts_en: sync,
                        server_cur_pts : time
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* network configuration example
 {
    "mqtt_cmd":1,
    "mqtt_operate_id":2,
    "device_token":"1057628122",
    "device_id":"7101389947744",
    "tag":"platfrom define",
    "network_cofnig":
    {
        "ip_addr":"172.18.195.67",
        "net_mask":"255.255.248.0",
        "gateway":"172.18.192.1",
        "DDNS1":"211.136.192.6",
        "DDNS2":"8.8.8.8",
        "DHCP":false
    }
}

*/

function networkConfig() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var ip = $('#ip').val();
    var mask = $('#mask').val();
    var gw = $('#gw').val();
    var dns1 = $('#dns1').val();
    var dns2 = $('#dns2').val();
    var dhcp = $('#dhcp').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 2,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    network_config:
                    {
                        ip_addr : ip,
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
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* Device remote parameter Configuration example
 {
    "mqtt_cmd":1,
    "mqtt_operate_id":4,
    "device_token":"1057628122",
    "device_id":"7101389947744",
    "tag":"platfrom define",
    "remote_config":
    {
        "volume":12,
        "screen_brightness":80,
        "light_supplementary":true
    }
}


*/

function remoteConfig() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var vol = $('#vol').val();
    var bright = $('#bright').val();
    var light = $('#light').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 4,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    remote_config:
                    {
                        volume: vol,
                        screen_brightness: bright,
                        light_supplementary: light
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* 	Device temperature control parameter configuration example
 {
    "mqtt_cmd":1,
    "mqtt_operate_id":6,
    "device_token":"1057628122",
    "device_id":"7101389947744",
    "tag":"platfrom define",
    "temperature_fun":
    {
        "temp_dec_en":false,
        "stranger_pass_en":false,
        "make_check_en":false,
        "alarm_temp":37.4,
        "temp_comp":1.1,
        "record_save_time":24,
        "save_record":true,
        "save_jpeg":true
    }
}
*/

function temperatureConfig() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var temp = $('#temp_dec_en').val();
    var stranger = $('#stranger_pass_en').val();
    var mask_check = $('#mask_check_enable').val();
    var video = $('#save_record').val();
    var photo = $('#save_jpeg').val();
    var alarm = $('#alarm_temp').val();
    var tmp_comp = $('#temp_comp').val();
    var save_time = $('#record_save_time').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 6,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    temperature_fun:
                    {
                        temp_dec_en: temp,
                        stranger_pass_en: stranger,
                        make_check_en: mask_check,
                        alarm_temp: alarm,
                        temp_comp: tmp_comp,
                        record_save_time: save_time,
                        save_record: video,
                        save_jpeg: photo
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* 	Add single or multiple user photo database information
 {
        "mqtt_cmd":1,
        "mqtt_operate_id":7,
        "device_token":"1514348271",
        "device_id":"7101389947744",
        "tag":"platfrom define",
        "piclib_manage":0,
        "param":
        {
            "lib_name":"Smart Park",
            "lib_id":"8",
            "server_ip":"172.18.195.61",
            "server_port":80,
            "pictures":
            [{
                "active_time":"2020/01/1 00:00:01",
                "user_id":"11",
                "user_name":"Zhang San",
                "end_time":"2020/12/30 23:59:59",
                "p_id":"null",
                "picture":"/192952.JPG"
            },
            {
                "active_time":"2020/01/1 00:00:01",
                "user_id":"22",
                "user_name":"LI SI",
                "end_time":"2020/12/30 23:59:59",
                "p_id":"null",
                "picture":"/linxiaofei.jpg"
            }]
        }
    }

*/

function addsUser() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var batch_name = $('#batch_name').val();
    var batch_id = $('#batch_id').val();
    var address = $('#server_ip').val();
    var port = $('#server_port').val();
    var in_time = $('#active_time').val();
    var out_time = $('#end_time').val();
    var id_user = $('#user_id').val();
    var name_user = $('#user_name').val();
    var url = $('#picture').val();

    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 7,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    piclib_manage: 0,
                    param:
                    {
                        lib_name: batch_name,
                        lib_id: batch_id,
                        server_ip: address,
                        server_port: port,
                        pictures:
                        {
                            active_time: in_time,
                            user_id: id_user,
                            user_name: name_user,
                            end_time: out_time,
                            p_id: null,
                            picture: url
                        }
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* delete individual or multiple user face database information
{
        "mqtt_cmd":1,
        "mqtt_operate_id":7,
        "device_token":"1057628122",
        "device_id":"7101389947744",
        "tag":"platfrom define",
        "piclib_manage":3,
        "param":{
            "users":[
                {
                    "user_id":"11"
                },
                {
                    "user_id":"22"
                }
            ]
        }
    }
*/

function deleteUser() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var user = $('#user_delete').val();
    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 7,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    piclib_manage:3,
                    param:
                    {
                        users: {
                            user_id: user
                        }
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* Delete the photo database information of the corresponding batch
{
        "mqtt_cmd":1,
        "mqtt_operate_id":7,
        "device_token":"1057628122",
        "device_id":"7101389947744",
        "tag":"platfrom define",
        "piclib_manage":2,
        "param":{
            "lib":[
                {
                    "lib_id":"8"
                },
                {
                    "lib_id":"9"
                }
            ]
        }
    }

*/

function deleteBatch() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();
    var batch = $('#batch_delete').val();
    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 7,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    piclib_manage:2,
                    param:
                    {
                        lib: {
                            lib_id: batch
                        }
                    }
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
}

/* Delete all photo library information
{
        "mqtt_cmd":1,
        "mqtt_operate_id":7,
        "device_token":"1057628122",
        "device_id":"7101389947744",
        "tag":"platfrom define",
        "piclib_manage":1
    }


*/

function deleteAll() {
    var dev_id = $('#bind_device_id').val();
    var dev_tag = $('#bind_tag').val();        
    $.ajax({
        type: "GET",
        url: '@Url.Action("publishMQTT", "Device")',
        dataType: "json",
        data:{
            topic:"SubscribeTest",
            msg: JSON.stringify(
                {
                    mqtt_cmd: 1,
                    mqtt_operate_id: 7,
                    device_token: device_tkn,
                    device_id: dev_id,
                    tag: dev_tag,
                    piclib_manage:1                        
                }
            ),
        },
        async: false,
        success: function(data) {
            if (data == null || data.data == null) {
                console.log("Error")
            } else {
                console.log("Ok")
            }
        },
        error: function() {
            console.log("Error Ajax");
        }
    });
};
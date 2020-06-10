using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AspStudio.Models;

namespace AspStudio.Controllers
{
    public class MqttCon {
        public string topic {get; set;}
        public string msg {get; set;}
    }

    

    public class DeviceController : Controller
    {

        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        private readonly ILogger<DeviceController> _logger;


        public DeviceController(ILogger<DeviceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public  async void publishMQTT(MqttCon mqtt) {
            Console.WriteLine(mqtt.topic);
            Console.WriteLine(mqtt.msg);

            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder()
                .WithClientId("Client123456789")
                .WithTcpServer("iot02.qaingenieros.com")
                .WithCleanSession()
                .Build();

                
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(mqtt.topic)
                .WithPayload(mqtt.msg)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            // Console.WriteLine(msg.ConvertPayloadToString());
            await mqttClient.ConnectAsync(options);
            await mqttClient.PublishAsync(msg);
            await mqttClient.DisconnectAsync();
        }

        

    }
}
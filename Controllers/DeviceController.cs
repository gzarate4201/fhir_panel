using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mqtt;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace AspStudio.Controllers
{
    public class DeviceController : Controller
    {


        private readonly ILogger<DeviceController> _logger;

        public DeviceController(ILogger<DeviceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public async void connectMQTT(object sender, EventArgs e) {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,	
                AllowWildcardsInTopicFilters = true 
            };
            var client = await MqttClient.CreateAsync("iot02.qaingenieros.com", configuration);
            var sessionState = await client.ConnectAsync (new MqttClientCredentials(clientId: "foo"));

        }

        public async void subscribeAll() {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,	
                AllowWildcardsInTopicFilters = true 
            };
            var client = await MqttClient.CreateAsync("iot02.qaingenieros.com", configuration);
            await client.SubscribeAsync("SubscribeTest", MqttQualityOfService.AtMostOnce);
            await client.DisconnectAsync();
        }

        public async void publish(String message, String topic) {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,	
                AllowWildcardsInTopicFilters = true 
            };
            var client = await MqttClient.CreateAsync("iot02.qaingenieros.com", configuration);
            var message1 = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(message)); 
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            await client.DisconnectAsync();

        }

        public async void publishTest() {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,	
                AllowWildcardsInTopicFilters = true 
            };
            var client = await MqttClient.CreateAsync("iot02.qaingenieros.com", configuration);
            var sessionState = await client.ConnectAsync (new MqttClientCredentials(clientId: "foo"));
            var message1 = new MqttApplicationMessage("PublishTest", Encoding.UTF8.GetBytes("Hola Mundo")); 
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            await client.DisconnectAsync();

        }
        
        public async void publishMQTT(String topic, String Msg) {
            var configuration = new MqttConfiguration {
                BufferSize = 128 * 1024,
                Port = 1883,
                KeepAliveSecs = 10,
                WaitTimeoutSecs = 2,
                MaximumQualityOfService = MqttQualityOfService.AtMostOnce,	
                AllowWildcardsInTopicFilters = true 
            };
            var client = await MqttClient.CreateAsync("iot02.qaingenieros.com", configuration);
            var sessionState = await client.ConnectAsync (new MqttClientCredentials(clientId: "foo"));
            var message1 = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(Msg)); 
            await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
            await client.DisconnectAsync();

        }
    }
}
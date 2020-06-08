using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mqtt;
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


        [HttpGet]
        [HttpPost]
        public async void publishMQTT(MqttCon mqtt) {
            if (string.IsNullOrEmpty(mqtt.topic) ||
                string.IsNullOrEmpty(mqtt.msg))
            {
                Console.WriteLine("Debe contener Topic y Message");
            } else {
                try{
                    Console.WriteLine(mqtt.topic);
                    Console.WriteLine(mqtt.msg);
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
                    var message1 = new MqttApplicationMessage(mqtt.topic, Encoding.UTF8.GetBytes(mqtt.msg)); 

                    await client.PublishAsync(message1, MqttQualityOfService.AtMostOnce); //QoS0
                    await client.DisconnectAsync();
                }
                catch (TimeoutException timeEx)
                {
                    Console.WriteLine("Time out failed....");
                    Console.WriteLine(timeEx.ToString());
                }
                catch (MqttConnectionException connectionEx)
                {
                    Console.WriteLine("MQTT connection failed....");
                    Console.WriteLine(connectionEx.ToString());
                }
                catch (MqttClientException clientEx)
                {
                    Console.WriteLine("MQTT client failed....");
                    Console.WriteLine(clientEx.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Standard Exception failed....");
                    Console.WriteLine(ex.ToString());
                }
                
            }
      

        }

    }
}
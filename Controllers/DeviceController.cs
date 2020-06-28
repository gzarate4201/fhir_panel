using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;


// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;




// Modelo que contiene las variables de un mensaje MQTT
// Para ser usadas dentro de un llamado HttpPost o HttpGet
namespace AspStudio.Controllers
{
    public class MqttCon {
        public string topic {get; set;}
        public string msg {get; set;}
    }

    public class DeviceData {
        public string dev_id {get; set;}
        public string dev_tag {get; set;}
    }

    

    public class DeviceController : Controller
    {

        // Inyecta la instancia de MQTTnet (mqttClient) que fue creada como
        // servicio inyectable en StartUp.cs
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        private readonly ILogger<DeviceController> _logger;
        
        // Inyeccion clase para manejo de la conexion a BD

        private readonly ApplicationDbContext dbContext;


        public DeviceController(ILogger<DeviceController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HttpGet]
        public async Task<JsonResult> publishMQTT(MqttCon mqtt) {

            // Inicializar respuesta 
            string result = string.Empty;

            // Debug
            Console.WriteLine(mqtt.topic);
            Console.WriteLine(mqtt.msg);

            // Parametros para la configuracion del cliente MQTT.
            var options = new MqttClientOptionsBuilder()
                .WithClientId("Client123456789")
                .WithTcpServer("iot02.qaingenieros.com")
                .WithCleanSession()
                .Build();

            // Consruye el mensaje MQTT
            var msg = new MqttApplicationMessageBuilder()
                .WithTopic(mqtt.topic)
                .WithPayload(mqtt.msg)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            // Console.WriteLine(msg.ConvertPayloadToString());
            // Establece conexion con el Broker
            await mqttClient.ConnectAsync(options);
            // Envia mensaje MQTT
            mqttClient.PublishAsync(msg).Wait();

            // Cierra la conexion
            await mqttClient.DisconnectAsync();

            // Retorna Json indicando que fue exitoso
            return Json(new {success=true});

        }


        [HttpPost]
        [HttpGet]
        public  Object addDevice(DeviceData deviceData) {
            var device = new Device() {
                DevId = "7101396770022",
                DevTag = "dev01",
                DevTkn = "12345678",
                Bound = false
            };

            
            try {
                dbContext.Devices.Add(device);
                dbContext.SaveChanges();
                // Retorna Json indicando que fue exitoso
                return new {success=true};
            } catch (Exception e) {
                System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                // Retorna Json indicando que fue exitoso
                return new {success=false};
            }
            

        }

        

    }
}
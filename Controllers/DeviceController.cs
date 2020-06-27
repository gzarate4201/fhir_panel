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
        private readonly IServiceScopeFactory _scopeFactory;


        public DeviceController(ILogger<DeviceController> logger)
        {
            _logger = logger;
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
                DevId = deviceData.dev_id,
                DevTag = deviceData.dev_tag,
            };

            using (var scope = _scopeFactory.CreateScope()) {

                // el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
                // scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
                // de lo contrario da un error 
                // Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
                // e instanciar un scope
                // revisar en el constructor la instanciacion de _scopeFactory
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
}
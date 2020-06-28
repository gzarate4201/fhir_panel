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
            var dispositivos = dbContext.Devices;
            System.Console.WriteLine(dispositivos.ToArray());

            List<dynamic> devices = new List<dynamic>();
            try{
                foreach(var dispositivo in dispositivos)
                {
                    devices.Add(
                        new
                        {
                            id = dispositivo.Id,
                            dev_id = dispositivo.DevId
                        }
                    );
                }
            } catch(System.Exception e) {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }
            

            // ViewBag.Dispositivos = devices;
            // System.Console.WriteLine(ViewBag.Dispositivos);
            return View();
        }

        [HttpPost]
        [HttpGet]
        public async Task<JsonResult> publishMQTT(MqttCon mqtt) {

            // Inicializar respuesta 
            string result = string.Empty;

            // Generar un client_id aleatorio para evitar conflictos en mqtt
            var client_id = Guid.NewGuid().ToString();


            // Debug
            Console.WriteLine(mqtt.topic);
            Console.WriteLine(mqtt.msg);

            // Parametros para la configuracion del cliente MQTT.
            var options = new MqttClientOptionsBuilder()
                .WithClientId("client_id")
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


        /// <summary>
        /// con esta funcion se crea un registro en la base de datos de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceData"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public  Object addDevice(DeviceData deviceData) {
            System.Console.WriteLine(deviceData);

            var device = new Device() {
                DevId = deviceData.dev_id,
                DevTag = deviceData.dev_tag,
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
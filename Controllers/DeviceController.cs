using System;
using System.Text;
using System.Collections.Generic;
using System.Dynamic;
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
using Mqtt.Client.AspNetCore.Settings;

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
            

            List<dynamic> Devices = new List<dynamic>();
            dynamic device;
            
            try{
                foreach(var dispositivo in dispositivos)
                {
                    device = new  ExpandoObject();
                    device.id = dispositivo.Id;
                    device.dev_id = dispositivo.DevId;
                    device.tag = dispositivo.DevTag;
                    Devices.Add(device);   
                }
            } catch(System.Exception e) {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }
            
            System.Console.WriteLine(Devices);
            ViewBag.Devices = Devices;
            // System.Console.WriteLine(ViewBag.Dispositivos);
            return View();
        }

        [HttpPost]
        [HttpGet]
        public Object publishMQTT(MqttCon mqtt) {

            // Inicializar respuesta 
            string result = string.Empty;

            // Generar un client_id aleatorio para evitar conflictos en mqtt
            var client_id = Guid.NewGuid().ToString();


            // Debug
            Console.WriteLine(mqtt.topic);
            Console.WriteLine(mqtt.msg);

            var clientSettinigs = AppSettingsProvider.ClientSettings;
            var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;

            // Parametros para la configuracion del cliente MQTT.
            var options = new MqttClientOptionsBuilder()
                .WithCredentials(clientSettinigs.UserName, clientSettinigs.Password)
                .WithClientId(client_id)
                .WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port)
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
            
            // Envia mensaje MQTT

            try {
                // Establece conexion con el Broker
                mqttClient.ConnectAsync(options).Wait();
                // Envia el mensaje
                mqttClient.PublishAsync(msg).Wait();
                //Desconecta el cliente
                mqttClient.DisconnectAsync().Wait();
            } catch(Exception e) {
                System.Console.WriteLine("Error enviando por MQTT : " + e.Message + e.StackTrace);
            }
            

            // Cierra la conexion
            //

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
                var result = dbContext.Devices.FirstOrDefault(p => p.DevId == device.DevId);
                if (result != null)
                {
                    // Retorna Json indicando que ya existe
                    return new {success=false, message="Dispositivo ya existe en la base de datos"};
                } else {
                    // Retorna Json indicando que fue exitoso
                    dbContext.Devices.Add(device);
                    dbContext.SaveChanges();
                    return new {success=true};
                }
                
                
                
            } catch (Exception e) {
                System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                // Retorna Json indicando que fue exitoso
                return new {success=false, message = "Error guardando en la base de datos"};
            }

        }

        [HttpGet]
        public  Object getDeviceToken(string device_id) {
            try {
                var result = dbContext.Devices.FirstOrDefault(p => p.DevId == device_id);
                if (result != null)
                {
                    // Retorna Json indicando que ya existe
                    return new {success=false, token=result.DevTkn, tag=result.DevTag};
                } else {
                    return new {success=false, message = "Dispositivo no encontrado"};
                }
                
            } catch (Exception e) {
                System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                // Retorna Json indicando que fue exitoso
                return new {success=false, message = "Error consutando la base de datos"};
            }
        }


        

    }
}
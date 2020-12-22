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

namespace AspStudio.Controllers
{



    public class EmpresaController : Controller
    {

        // Inyecta la instancia de MQTTnet (mqttClient) que fue creada como
        // servicio inyectable en StartUp.cs
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        private readonly ILogger<EmpresaController> _logger;

        // Inyeccion clase para manejo de la conexion a BD

        private readonly ApplicationDbContext dbContext;

        public EmpresaController(ILogger<EmpresaController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        // GET: EmpresaController
        public IActionResult Index()
        {

            var empresasdbc = dbContext.Empresas;


            List<dynamic> Empresas = new List<dynamic>();
            dynamic empresa;

            try
            {
                foreach (var empdbc in empresasdbc)
                {
                    empresa = new ExpandoObject();
                    empresa.codigo = empdbc.codigo;
                    empresa.descripcion = empdbc.descripcion;
                    Empresas.Add(empresa);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(Empresas);
            ViewBag.Empresas = Empresas;
            // System.Console.WriteLine(ViewBag.Dispositivos);
            
            return View();
        }

        [HttpPost]
        [HttpGet]
        public Object publishMQTT(MqttCon mqtt)
        {

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

            try
            {
                // Establece conexion con el Broker
                mqttClient.ConnectAsync(options).Wait();
                // Envia el mensaje
                mqttClient.PublishAsync(msg).Wait();
                //Desconecta el cliente
                mqttClient.DisconnectAsync().Wait();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error enviando por MQTT : " + e.Message + e.StackTrace);
            }


            // Cierra la conexion
            //

            // Retorna Json indicando que fue exitoso
            return Json(new { success = true });

        }


        /// <summary>
        /// con esta funcion se crea un registro en la base de datos de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceData"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public Object addEmpresa(Empresa empresaData)
        {
            System.Console.WriteLine(empresaData);

            var empresa = new Empresa()
            {
                codigo = empresaData.codigo,
                descripcion = empresaData.descripcion
            };
            try
            {
                var result = dbContext.Empresas.FirstOrDefault(p => p.codigo == empresa.codigo);
                if (result != null)
                {
                    // Retorna Json indicando que ya existe
                    return new { success = false, message = "Empresa ya existe en la base de datos" };
                }
                else
                {
                    // Retorna Json indicando que fue exitoso
                    dbContext.Empresas.Add(empresa);
                    dbContext.SaveChanges();
                    return new { success = true };
                }



            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                // Retorna Json indicando que fue exitoso
                return new { success = false, message = "Error guardando en la base de datos" };
            }

        }


        [HttpGet]
        public Object getDeviceToken(string codigo)
        {
            try
            {
                var result = dbContext.Empresas.FirstOrDefault(p => p.codigo == codigo);
                if (result != null)
                {
                    // Retorna Json indicando que ya existe
                    return new { success = false, token = result.codigo, tag = result.descripcion };
                }
                else
                {
                    return new { success = false, message = "Empresa no encontrada" };
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                // Retorna Json indicando que fue exitoso
                return new { success = false, message = "Error consutando la base de datos" };
            }
        }


    }
}

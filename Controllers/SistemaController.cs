using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// Hub Chat
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;

// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using Mqtt.Client.AspNetCore.Settings;


// Logging
using Microsoft.Extensions.Logging;



// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;

namespace AspStudio.Controllers
{
    
    public class SistemaController : Controller
    {
        
        private readonly ILogger<DeviceController> _logger;
            
        // Inyeccion clase para manejo de la conexion a BD

        private readonly ApplicationDbContext dbContext;

        public SistemaController(ILogger<DeviceController> logger, ApplicationDbContext _dbContext)
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
            return View();
        }
    }
}
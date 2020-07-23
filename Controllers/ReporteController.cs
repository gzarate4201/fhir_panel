using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;

namespace AspStudio.Controllers
{
    
     public class getFilter {
        public string start_date {get; set;}
        public string end_date {get; set;}
        public string name {get; set;}
        public string company {get; set;}
        public string document {get; set;}
        public string temperature {get; set;}
        public string tmin {get; set;}
        public string tmax {get; set;}
        public string smin {get; set;}
        public string smax {get; set;}
        public string device_id {get; set;}
        public string similar {get; set;}
        public Boolean hasMatch {get; set;}
        public string mask {get; set;}
        public string hasId {get; set;}
        public string hasPhoto {get; set;}
        public string groupBy {get; set;}
        public string ciudad {get; set;}
        public string sitio {get; set;}

    }

    public class ReporteController : Controller
    {
        // Inyecta la instancia de MQTTnet (mqttClient) que fue creada como
        // servicio inyectable en StartUp.cs
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        private readonly ILogger<ReporteController> _logger;

        // Inyeccion clase para manejo de la conexion a BD
        private readonly ApplicationDbContext dbContext;

        public ReporteController(ILogger<ReporteController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            var dispositivos = dbContext.Devices;


            List<dynamic> Devices = new List<dynamic>();
            dynamic device;

            try
            {
                foreach (var dispositivo in dispositivos)
                {
                    device = new ExpandoObject();
                    device.id = dispositivo.Id;
                    device.dev_id = dispositivo.DevId;
                    device.tag = dispositivo.DevTag;
                    device.token = dispositivo.DevTkn;
                    Devices.Add(device);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(Devices);
            ViewBag.Devices = Devices;
            // System.Console.WriteLine(ViewBag.Dispositivos);
            return View();
        }


        public JsonResult getReconocimientos(getFilter filter) {

            var result = from o in dbContext.Reconocimientos
                select o;

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);

            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            //Console.WriteLine("Start Time: {0}", DateTime.Parse(filter.start_date).ToString());
            if (filter.start_date != null) 
            result = result.Where(c => c.DateTime >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.DateTime <= DateTime.Parse(filter.end_date));

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }

        public JsonResult getRecoDia(getFilter filter) {

            var result = from o in dbContext.RecoDia
                select o;

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);
            
            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            //Console.WriteLine("Start Time: {0}", DateTime.Parse(filter.start_date).ToString());
            if (filter.start_date != null) 
            result = result.Where(c => c.Fecha >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.Fecha <= DateTime.Parse(filter.end_date));

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }

        public JsonResult getRepoEnroll(getFilter filter) {

            var result = from o in dbContext.RepoEnrolamientos
                select o;

            if (filter.name != null) 
            result = result.Where(c => c.Name.Contains(filter.name));

            if (filter.company != null) 
            result = result.Where(c => c.Empresa.Contains(filter.company));

            //Console.WriteLine("Start Time: {0}", DateTime.Parse(filter.start_date).ToString());
            if (filter.start_date != null) 
            result = result.Where(c => c.Fecha >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.Fecha <= DateTime.Parse(filter.end_date));

            if (filter.hasPhoto != null) {
                var hasPhoto = (filter.hasPhoto == "true") ? true : false;
                result = result.Where(c => c.hasPhoto == hasPhoto);
            }

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }

        public JsonResult getRepoEnrollDev(getFilter filter) {

            var result = from o in dbContext.RepoEnrollDevices
                select o;

            if (filter.name != null) 
            result = result.Where(c => c.Name.Contains(filter.name));

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);

            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            if (filter.hasPhoto != null) {
                var hasPhoto = (filter.hasPhoto == "true") ? true : false;
                result = result.Where(c => c.hasPhoto == hasPhoto);
            }

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }

        public JsonResult getSopoRecoDia(getFilter filter) {

            var result = from o in dbContext.SopoRecoDias
                select o;

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);

            if (filter.document != null) 
            result = result.Where(c => c.DocId.Contains(filter.document));

            if (filter.company != null) 
            result = result.Where(c => c.Empresa.Contains(filter.company));

            if (filter.name != null) 
            result = result.Where(c => c.Name.Contains(filter.name));
            
            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            //Console.WriteLine("Start Time: {0}", DateTime.Parse(filter.start_date).ToString());
            if (filter.start_date != null) 
            result = result.Where(c => c.Fecha >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.Fecha <= DateTime.Parse(filter.end_date));

            if (filter.tmin != null) 
            result = result.Where(c => c.Temperature >= Double.Parse(filter.tmin));
                            

            if (filter.tmax != null) 
            result = result.Where(c => c.Temperature <= Double.Parse(filter.tmax));


            if (filter.smin != null) 
            result = result.Where(c => c.Similar >= Double.Parse(filter.smin));
                            

            if (filter.smax != null) 
            result = result.Where(c => c.Similar <= Double.Parse(filter.smax));

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }


        public JsonResult getEventos(getFilter filter) {

            //var result = dbContext.Persons
            //    .Where(t => t.DevId == filter.device_id);

            

            var result = from o in dbContext.Persons
                select o;

            if (filter.name != null) 
            result = result.Where(c => c.Name.Contains(filter.name));

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);

            if (filter.mask != null) 
            result = result.Where(c => c.Mask == Int16.Parse(filter.mask));

            if (filter.document != null) 
            result = result.Where(c => c.UserId == Int16.Parse(filter.document));

            if (filter.tmin != null) 
            result = result.Where(c => c.Temperature >= Double.Parse(filter.tmin));
                            

            if (filter.tmax != null) 
            result = result.Where(c => c.Temperature <= Double.Parse(filter.tmax));

            
            

            //Console.WriteLine("Start Time: {0}", DateTime.Parse(filter.start_date).ToString());
            if (filter.start_date != null) 
            result = result.Where(c => c.RegisterTime >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.RegisterTime <= DateTime.Parse(filter.end_date));
           
            
            // if (filter.groupBy != null) {
            //     switch (filter.groupBy)
            //     {
            //         case "name":
            //             result = result.GroupBy(n => new { n.Name });
            //             break;
            //         case "device":
            //             result = result.GroupBy(n => new { n.DevId });
            //             break;
            //         default:
            //     }
            // }

            
            
            // IQueryable<IGrouping<int, Person>> groups = result.GroupBy(x => x.UserId);

            if (filter.hasId != null) {
                result = result.Where(c => c.UserId > 0);
            }

            var count = result.Count();

            return new JsonResult ( new { Count = count, Data = result} );
          
        }

    }
}


// && ((filter.name == "") || (item.Name.Contains(filter.name)))
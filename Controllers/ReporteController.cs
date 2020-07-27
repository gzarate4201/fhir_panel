//using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Net.Http;
using System.Web;

using System.Net.Mime;
// using System.Reflection.PortableExecutable;
// using System.Net;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


// Email 
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

// JSON
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Regular Expresion
using System.Text.RegularExpressions;

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

// C# PDF Generator
using PdfSharpCore.Drawing;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharpCore.Pdf;


using IronPdf;
using HandlebarsDotNet;


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
        public string eXcompany {get; set;}
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
        public string persons {get; set;}
        public string hasPhoto {get; set;}
        public string groupBy {get; set;}
        public string ciudad {get; set;}
        public string sitio {get; set;}

    }

    
    public class pdfData {

        public string pdfName {get; set;}
        public string data {get; set;}
    }


    public class MailData
    {
        public string to { get; set; }
        public string attachment { get; set; }
    }


    public class responseJson
    {
        public string count { get; set; }
        public string data { get; set; }
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

        [HttpPost]
        [HttpGet]
        public JsonResult getRecoDia(getFilter filter) {

            var result = from o in dbContext.RecoDia
                select o;

            if (filter.device_id != null)
            result = result.Where(c => c.DevId == filter.device_id);
            
            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            // Console.WriteLine("Start Time: {0} End Time : {1}", DateTime.Parse(filter.start_date).ToString(), DateTime.Parse(filter.end_date).ToString());

            if (filter.start_date != null) 
            result = result.Where(c => c.time >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.time <= DateTime.Parse(filter.end_date));

            var count = result.Count();


            return new JsonResult ( new { Count = count, Data = result} );

        }

        [HttpPost]
        [HttpGet]
        public JsonResult getSopoRecoPersona(getFilter filter) {

            var result = from o in dbContext.SopoRecoPersonas
                select o;


            
            if (filter.ciudad != null) 
            result = result.Where(c => c.Ciudad.Contains(filter.ciudad));

            if (filter.name != null) 
            result = result.Where(c => c.Name.Contains(filter.name));

            if (filter.company != null) 
            result = result.Where(c => c.Empresa.Contains(filter.company));

            // Console.WriteLine("Start Time: {0} End Time : {1}", DateTime.Parse(filter.start_date).ToString(), DateTime.Parse(filter.end_date).ToString());

            if (filter.start_date != null) 
            result = result.Where(c => c.Time >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.Time <= DateTime.Parse(filter.end_date));

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
            result = result.Where(c => c.Time >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.Time <= DateTime.Parse(filter.end_date));

            if (filter.hasPhoto != null) {
                
                var hasFoto = (filter.hasPhoto == "true") ? true : false;
                Console.WriteLine("El filtro es :" + filter.hasPhoto);
                if (filter.hasPhoto == "true") {
                    result = result.Where(c => c.hasPhoto == true);
                } else {
                    result = result.Where(c => c.hasPhoto == false);
                }
                
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
                Console.WriteLine("Filtro enrolamiento:" + filter.hasPhoto);
                var hasPhoto = (filter.hasPhoto == "1") ? true : false;
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
            result = result.Where(c => c.DateTime >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.DateTime <= DateTime.Parse(filter.end_date));

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

        public JsonResult getSopoEvRecoDia(getFilter filter) {

            var result = from o in dbContext.SopoEvRecoDias
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
            result = result.Where(c => c.DateTime >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.DateTime <= DateTime.Parse(filter.end_date));

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


        public JsonResult SendEmail (EmailFormModel model) {

            var message = new MimeMessage ();
			message.From.Add (new MailboxAddress ("Alejandro", "alejandromejia@qaingenieros.com"));
			message.To.Add (new MailboxAddress (model.ToName, model.ToEmail));
			message.Subject = model.Subject;

			// message.Body = new TextPart ("plain") {
			// 	Text = model.Message
			// };

            //Fetch the attachments from db
            //considering one or more attachments
            var builder = new BodyBuilder { TextBody = model.Message };
            builder.Attachments.Add(@"wwwroot/Reports/reporte.pdf");
            message.Body = builder.ToMessageBody();
            
            using (var client = new SmtpClient ()){
                client.Connect ("mail.qaingenieros.com", 587, false);

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate ("alejandromejia@qaingenieros.com", "Al3j0@17");

				client.Send (message);
				client.Disconnect (true);
            }

            return new JsonResult ( new { Status = "Success"} );
        }


    

 
        /// <summary>
        /// Recibe un PDF desde la vista de reportes
        /// Lo guarda en el directorio wwwrott/Reports
        /// Lo envia por email
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
		public ActionResult Export(MailData data)
		{
            // Parse JSON
            // MailData data = JsonConvert.DeserializeObject<MailData>(content);

			//create pdf
            Console.WriteLine(data);
			var pdfBinary = Convert.FromBase64String(data.attachment);
			// var dir = Server.MapPath("~/DataDump");
            var folderPath = "wwwroot/Reports/";
            var pdfPath = folderPath + "generated.pdf";


			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			var fileName =  folderPath + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".pdf";

			// write content to the pdf
			using (var stream = System.IO.File.Create(fileName))
            {
                try {
                    stream.Write(pdfBinary, 0, pdfBinary.Length);
                } catch(System.Exception e){
                    System.Console.WriteLine("Error saving " + fileName + " in filesystem" + e.Message + e.StackTrace );
                }
            }

			//Send mail
			var status = SendMail(fileName, data.to);
    		//Delete file from file system
			//System.IO.File.Delete(fileName);

			//Return result to client
			return Json(status ? new { result = "success" } : new { result = "failed" });
		}

        private static bool SendMail(string filePath, string recipient)
		{
            var message = new MimeMessage ();
			message.From.Add (new MailboxAddress ("Alejandro", "alejandromejia@qaingenieros.com"));
			message.To.Add (new MailboxAddress ("Alejandro Mejia",recipient));
			message.Subject = "Informe diario";

            Console.WriteLine("Archivo: " + filePath);
            //Fetch the attachments from db
            //considering one or more attachments
            var builder = new BodyBuilder { TextBody = "PFA"};
            builder.Attachments.Add(filePath);
            message.Body = builder.ToMessageBody();
            
            using (var client = new SmtpClient ()){
                client.Connect ("mail.qaingenieros.com", 587, false);

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate ("alejandromejia@qaingenieros.com", "Al3j0@17");

                try
				{
					client.Send(message);
					return true;
				}
				catch (Exception)
				{
					return false;
				}
            }

		}

        public JsonResult dailyPdf() {

            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.MarginTop = 20;  //millimeters
            Renderer.PrintOptions.MarginBottom = 40;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;

            Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            {
                CenterText = "{pdf-title}",
                DrawDividerLine = true,
                FontSize = 16
            };
            Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            {
                LeftText = "{date} {time}",
                RightText = "Pagina {page} de {total-pages}",
                DrawDividerLine = true,
                FontSize = 14
            };

            var html = System.IO.File.ReadAllText(Path.Combine("wwwroot/Assets", "Informe.html"));
            //Console.WriteLine(html);

            
            var filter = new getFilter();
            // var today = DateTime.Now.ToString("yyyy-MM-dd 12:00");
            
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);
            Console.WriteLine("Hoy:" + today.ToString("yyyy-MM-dd 12:00"));
            Console.WriteLine("Ayer:" + yesterday.ToString("yyyy-MM-dd 12:00"));

            filter.start_date = yesterday.ToString("yyyy-MM-dd 12:00");
            filter.end_date = today.ToString("yyyy-MM-dd 12:00");

            var result = from o in dbContext.RecoDia
                select o;

            var enrolados = from e in dbContext.RepoEnrolamientos
                select e;

            var eventosPersona = from e in dbContext.SopoRecoPersonas
                select e;
            
            var eventosRecon = from e in dbContext.SopoEvRecoDias
                select e;

            var alarmas = eventosRecon.Where(c => c.Temperature >= 37.3);

            var totalPersonas = eventosPersona.Count();
            var totalEventos = eventosRecon.Count();
            var totalAlarmas = alarmas.Count();

            if (filter.start_date != null) 
            result = result.Where(c => c.time >= DateTime.Parse(filter.start_date));

            if (filter.end_date != null) 
            result = result.Where(c => c.time <= DateTime.Parse(filter.end_date));

            result = result.OrderBy(c => c.Ciudad).ThenBy(c =>c.Sitio);

            var count = result.Count();

            var totalEnrolados = enrolados.Count();

            var enroladosOk = enrolados.Where(c => c.hasPhoto == true);

            var okEnrolados = enroladosOk.Count();

            var enroladosNew = enrolados.Where(c => c.Time >= DateTime.Parse(filter.start_date));
            enroladosNew = enroladosNew.Where(c => c.Time <= DateTime.Parse(filter.end_date));

            var newEnrolados = enroladosNew.Count();

            string htmlTable = "";

            int tEventos = 0;
            int tPersonas = 0;
            int tAlarmas = 0;

            foreach (var item in result)
            {
                Console.WriteLine("{0} {1} {2} {3}\n", item.Fecha, item.time, 
                    item.Sitio, item.Recos, item.Personas);
                htmlTable += "<tr><td>" + item.time + "</td><td>" + item.Ciudad + "</td>" + "<td>" + item.Sitio + "</td>" + "<td class='numero'>" + item.Recos + "</td>" + "<td class='numero'>" + item.Personas + "</td>" + "<td class='numero'>" + item.Alertas + "</td></tr>";
                tEventos+= item.Recos;
                tPersonas+= item.Personas;
                tAlarmas+= item.Alertas;
            }

            htmlTable += "<tr><td> Totales </td><td></td><td></td><td class='numero'>" + tEventos + "</td>" + "<td class='numero'>" + tPersonas + " ** </td>" + "<td class='numero'>" + tAlarmas + "</td></tr>";
            Console.WriteLine(htmlTable);
            

            // Cargar los datos
            html = html.Replace("{{start_date}}", filter.start_date.ToString());
            html = html.Replace("{{end_date}}"  , filter.end_date.ToString());
            html = html.Replace("{{totalDatos}}"       , totalEnrolados.ToString());
            html = html.Replace("{{totalEnrolados}}"   , okEnrolados.ToString());
            html = html.Replace("{{nuevosEnrolados}}"  , newEnrolados.ToString());
            html = html.Replace("{{tablaEventos}}"     , htmlTable);
            html = html.Replace("{{totalEventos}}"     , totalEventos.ToString());
            html = html.Replace("{{totalPersonas}}"    , totalPersonas.ToString());
            html = html.Replace("{{totalAlertas}}"     , totalAlarmas.ToString());
            html = html.Replace("{{totalEventosP}}"     , tEventos.ToString());
            html = html.Replace("{{totalPersonasP}}"    , tPersonas.ToString());
            html = html.Replace("{{totalAlertasP}}"     , tAlarmas.ToString());


            var PDF = Renderer.RenderHtmlAsPdf(html);
            var OutputPath = "wwwroot/Reports/ReporteDiario.pdf";
            PDF.SaveAs(OutputPath);

            SendMail(OutputPath,"alejomejia1@gmail.com");
            SendMail(OutputPath,"mauriciogaviria@qaingenieros.com");
            SendMail(OutputPath,"santiagouruena@qaingenieros.com");
            // This neat trick opens our PDF file so we can see the result in our default PDF viewer
            // System.Diagnostics.Process.Start(OutputPath);
            return new JsonResult ( new { Status = "Success"} );
        }
	
    
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Threading;

using System.Net;
using System.Web;

using System.Data;
using System.Text;
using System.Text.RegularExpressions;


// using LinqToExcel;
// using ImportExceData.Models;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.Util.Collections;
using NPOI.Util;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Mqtt.Client.AspNetCore.Settings;

// Hub Chat
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;


// using System.Net.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

// Json
using System.Text.Json;
using System.Text.Json.Serialization;

// Database connection
using AspStudio.Models;
using AspStudio.Data;

namespace AspStudio.Controllers
{
    
    public class DeleteId {
        public string document {get; set;}
    }
    public class EmployeeController : Controller
    {
        // Inyecta la instancia de MQTTnet (mqttClient) que fue creada como
        // servicio inyectable en StartUp.cs
        static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment _environment;
        // public IHostingEnvironment _hostingEnvironment { get; set; }
        public IWebHostEnvironment _hostingEnvironment { get; set; }

        public EmployeeController(ILogger<AccountController> logger, ApplicationDbContext _dbContext, IWebHostEnvironment env, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            dbContext = _dbContext;
            _environment = env;
            _hostingEnvironment = hostingEnvironment;

        }





        // public IActionResult Index(string sortOrder, string searchString)
        // {
        //     ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "Id" : "";
        //     ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstName" : "";
            
        //     var empleados = from s in dbContext.Empleados
        //                     select s;
        //     if (!String.IsNullOrEmpty(searchString))
        //     {
        //         empleados = empleados.Where(s => s.FirstName.Contains(searchString)
        //                             || s.LastName.Contains(searchString));
        //     }
        //     switch (sortOrder)
        //     {
        //         case "lastname_desc":
        //             empleados = empleados.OrderByDescending(s => s.LastName);
        //             break;
        //         default:
        //             empleados = empleados.OrderBy(s => s.Id);
        //             break;
        //     }
        //     return View(empleados.ToList());

        // }
        [HttpGet]
        public ViewResult Index()
        {
            var serverHostSettings = AppSettingsProvider.ServerHostSettings;
            // var serverIp = serverHostSettings.Host;
            // var serverPort = serverHostSettings.Port;
            var serverIp = "192.168.0.5";
            var serverPort = 5050;
            var imageServerIp = "192.168.0.5";
            // var imageServerIp = AppSettingsProvider.ImageServerIp;
            // var serverPort = AppSettingsProvider.ServerPort;
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
                    Devices.Add(device);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(Devices);
            ViewBag.Devices = Devices;
            ViewBag.ServerIp = serverIp;
            ViewBag.ImageServerIp = imageServerIp;
            ViewBag.ServerPort = serverPort;
            // System.Console.WriteLine(ViewBag.Dispositivos);
            // Result needs to be IQueryable in database scenarios, to make use of database side paging.
            return View(dbContext.Set<Employee>());
        }



        [HttpPost]
        [HttpGet]
        public string getEmployees () {
            var empleados = dbContext.Empleados;
            string JsonData = JsonSerializer.Serialize(empleados);
            return JsonData;
        }

        [HttpPost]
        [HttpGet]
        public string getEmployeeByIdLenel (int idLenel) {
            var empleado = dbContext.Empleados.Where(c=>c.IdLenel.Equals(idLenel));
            string JsonData = JsonSerializer.Serialize(empleado);
            return JsonData;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "Uploads/");
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return RedirectToAction("Index");
        }


        /// <summary>  
        /// This function is used to download excel format.  
        /// </summary>  
        /// <param name="Path"></param>  
        /// <returns>file</returns>  
        public FileResult DownloadExcel()  
        {  
            string path = "/Doc/Users.xlsx";  
            return File(path, "application/vnd.ms-excel", "Users.xlsx");  
        }  
  
        public ActionResult ImportToTable()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string[] keys = new   string[] {};
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table class='table table-bordered'><tr>");

                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                        keys[j] = cell.ToString();
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return this.Content(sb.ToString());
        }


        [HttpPost]
        public  ActionResult Import() {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string[] keys = new   string[] {};
            

            DataTable Tabla = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;

                        Tabla = new DataTable(sheet.SheetName);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        for(int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);
                            // IRow row2 = null;
                            DateTime localDate = DateTime.Now; 

                            if(row != null && rowIndex > 0) {
                                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                                var employeeDb = dbContext.Empleados.FirstOrDefault(p => p.Documento.Contains(row.Cells[6].ToString()));
                                var employee = new Employee() {
                                    IdLenel = row.Cells[0].ToString(),
                                    LastName = row.Cells[1].ToString(),
                                    FirstName = row.Cells[2].ToString(),
                                    SSNO = row.Cells[3].ToString(),
                                    IdStatus = row.Cells[4].ToString(),
                                    Status = row.Cells[5].ToString(),
                                    Documento = row.Cells[6].ToString(),
                                    Empresa = row.Cells[7].ToString(),
                                    Nit = row.Cells[8].ToString(),
                                    StartTime = DateTime.Parse(row.Cells[9].ToString()),
                                    EndTime = DateTime.Parse(row.Cells[10].ToString()),
                                    imageUrl = row.Cells[11].StringCellValue.ToString(),
                                    Ciudad = row.Cells[12].ToString(),
                                    Created = localDate,
                                    Updated = null
                                };

                                if (employeeDb != null)
                                {
                                    employee.Id = employeeDb.Id;
                                    employee.Created = employeeDb.Created;
                                    employee.Updated = localDate;
                                    dbContext.Empleados.Update(employee);
                                    dbContext.SaveChanges();
                                } else {
                                    
                                    dbContext.Empleados.Add(employee);
                                    dbContext.SaveChanges();
                                }
                                

                            }
                            
                            
                        }
                    }
                }
                else
                {
                    throw new Exception("ERROR 404: El archivo especificado NO existe.");
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            // return new {success=true, message = "Base de Datos Actualizada"};
            //return View();
            return Content("<p> Se importo el archivo excel con exito");


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
        
        [HttpPost]
        [HttpGet]
        public  Object DeleteUser(DeleteId deleteId)
        {
            var document = deleteId.document;

            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var employeeDb = dbContext.Empleados.FirstOrDefault(p => p.Documento.Contains(document));
            if (employeeDb != null)
            {
                dbContext.Empleados.Remove(employeeDb);
                dbContext.SaveChanges();
                return new {success=true, message="Registro borrado de la base de datos"};
            }

            return new {success=false, message="Registro no existente"};
        } 

        // public ActionResult SendToDevice()
        // {
        //     var empleados = dbContext.Empleados;
            
        //     return JsonData;
        // }




    }
}
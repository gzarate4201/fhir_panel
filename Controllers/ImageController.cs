using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using studio.Models;

// Database connection
using AspStudio.Models;
using AspStudio.Data;
using System.Dynamic;

namespace studio.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        private readonly ApplicationDbContext dbContext;

        public ImageController(ILogger<ImageController> logger, ApplicationDbContext _dbContext)
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
                    Devices.Add(device);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error generando lista" + e.Message + e.StackTrace);
            }

            System.Console.WriteLine(Devices);
            ViewBag.Devices = Devices;
            
            try{
                var dirs = Directory.GetFiles("wwwroot/Registers/", "*.jpg", SearchOption.AllDirectories).Select(f=> new FileInfo(f)).OrderByDescending(f=> f.CreationTime);
                // var sortedDir = dirs.OrderBy(item => int.Parse(item));


                ViewBag.Archivos = dirs;
            } catch (Exception e) {
                Console.WriteLine("The process failed: {0}", e.ToString());
                
                ViewBag.Archivos = null;

            }
            return View();
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

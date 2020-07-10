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

namespace studio.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            
            try{
                var dirs = Directory.EnumerateFiles("wwwroot/Registers/", "7101076854037*.jpg", SearchOption.AllDirectories);
    
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

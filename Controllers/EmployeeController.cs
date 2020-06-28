using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;


// Json
using System.Text.Json;
using System.Text.Json.Serialization;

// Database connection
using AspStudio.Models;
using AspStudio.Data;

namespace AspStudio.Controllers
{
    
    
    public class EmployeeController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(ILogger<AccountController> logger, ApplicationDbContext _dbContext, IWebHostEnvironment env)
        {
            _logger = logger;
            dbContext = _dbContext;
            _environment = env;
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
    }
}
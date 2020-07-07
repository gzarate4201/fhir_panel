using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                                    imageUrl = row.Cells[8].ToString(),
                                    StartTime = DateTime.Parse(row.Cells[9].ToString()),
                                    EndTime = DateTime.Parse(row.Cells[10].ToString())
                                };

                                if (employeeDb != null)
                                {
                                    employee.Id = employeeDb.Id;
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


        public ActionResult Download()
        {
            string Files = "wwwroot/UploadExcel/CoreProgramm_ExcelImport.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "employee.xlsx");
        }

        public async Task<IActionResult> Export()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Employees.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("employee");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("EmployeeId");
                row.CreateCell(1).SetCellValue("EmployeeName");
                row.CreateCell(2).SetCellValue("Age");
                row.CreateCell(3).SetCellValue("Sex");
                row.CreateCell(4).SetCellValue("Designation");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue(1);
                row.CreateCell(1).SetCellValue("Jack Supreu");
                row.CreateCell(2).SetCellValue(45);
                row.CreateCell(3).SetCellValue("Male");
                row.CreateCell(4).SetCellValue("Solution Architect");

                row = excelSheet.CreateRow(2);
                row.CreateCell(0).SetCellValue(2);
                row.CreateCell(1).SetCellValue("Steve khan");
                row.CreateCell(2).SetCellValue(33);
                row.CreateCell(3).SetCellValue("Male");
                row.CreateCell(4).SetCellValue("Software Engineer");

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue(3);
                row.CreateCell(1).SetCellValue("Romi gill");
                row.CreateCell(2).SetCellValue(25);
                row.CreateCell(3).SetCellValue("FeMale");
                row.CreateCell(4).SetCellValue("Junior Consultant");

                row = excelSheet.CreateRow(4);
                row.CreateCell(0).SetCellValue(4);
                row.CreateCell(1).SetCellValue("Hider Ali");
                row.CreateCell(2).SetCellValue(34);
                row.CreateCell(3).SetCellValue("Male");
                row.CreateCell(4).SetCellValue("Accountant");

                row = excelSheet.CreateRow(5);
                row.CreateCell(0).SetCellValue(5);
                row.CreateCell(1).SetCellValue("Mathew");
                row.CreateCell(2).SetCellValue(48);
                row.CreateCell(3).SetCellValue("Male");
                row.CreateCell(4).SetCellValue("Human Resource");

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Controllers
{
    public class EnrolController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }



    }
}

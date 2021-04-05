using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steps_Count_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SharedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

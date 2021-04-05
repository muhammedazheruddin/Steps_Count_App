using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Steps_Count_App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

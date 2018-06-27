using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        // GET
        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
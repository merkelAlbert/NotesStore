using Microsoft.AspNetCore.Mvc;

namespace Notes.Controllers
{
    public class NotesController : Controller
    {
        // GET
        [Route("notes")]
        public IActionResult Index()
        {
            return Json("notes");
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace MiSitioWeb.Controllers
{
    public class HtmlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using FluentCssMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FluentCssMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        // Rendered over fetch/AJAX from the About page. It registers classes that
        // the page's stylesheet link did not know about, which is the case the
        // tag helper script and middleware are there to demonstrate.
        public IActionResult LoadPartial()
        {
            return PartialView("_Partial");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

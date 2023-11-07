namespace FastFood.Web.Controllers
{
    using System.Diagnostics;
    using IdentityServer3.Core.ViewModels;
    using Microsoft.AspNetCore.Mvc;



    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // If we need any data, we should collect it before rendering
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

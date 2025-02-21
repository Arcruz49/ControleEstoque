using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class AnalyticsController : Controller
    {
        public IActionResult analytcsView()
        {
            return View();
        }
    }
}

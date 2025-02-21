using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

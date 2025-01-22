using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class ConfiguracaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

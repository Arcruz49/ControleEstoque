using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class MateriaisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

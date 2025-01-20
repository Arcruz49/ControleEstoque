using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleEstoque.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult GetDados(DateTime data, int tipoFiltro = 0)
        {

            //switch (tipoFiltro)
            //{
            //    case 1
            //}


            var resultado = new
            {
                quantidadeVendas = 100,  // Exemplo
                valorGanho = 25000.00,  // Exemplo
                valorGasto = 10000.00,  // Exemplo
                lucroTotal = 15000.00,  // Exemplo
                graficoLabels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }, // Exemplos
                graficoValores = new[] { 500, 700, 900, 1200, 1100 } // Exemplos
            };

            return Json(resultado);
        }

    }
}

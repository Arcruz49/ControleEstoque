using ControleEstoque.Contexts;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleEstoque.Controllers
{
    public class HomeController : Controller
    {
        private readonly ControleEstoqueContext db;

        public HomeController(ControleEstoqueContext _db)
        {
            db = _db;
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
            var quantidadeVendas = 0;
            var valorGanho = 0.0;
            var valorGasto = 0.0;
            var lucroTotal = 0.0;
            int? produtosVendidos = 0;

            var graficoLabels = new List<string>();  
            var graficoValores = new List<double>();
            var produtosTiposLabels = new List<string>(); 
            var produtosTiposValores = new List<int>(); 

            switch (tipoFiltro)
            {
                case 1: // Filtro por dia
                    quantidadeVendas = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Date == data.Date)
                        .Count();

                    valorGanho = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Date == data.Date)
                        .Sum(a => (double?)a.valorVenda ?? 0);

                    valorGasto = db.cadProdutos
                        .Where(p => p.dtCriacao.HasValue && p.dtCriacao.Value.Date == data.Date)
                        .Sum(p => (double?)p.valorCompra * p.quantidade) ?? 0;

                    lucroTotal = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Date == data.Date)
                        .Sum(a => (double?)a.valorLucro ?? 0);

                    produtosVendidos = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Date == data.Date)
                        .Sum(x => (int?)x.vp.quantidade);

                    graficoLabels = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Date == data.Date)
                        .AsEnumerable()
                        .Select((v, index) => $"Venda {index + 1}")
                        .ToList();

                    graficoValores = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Date == data.Date)
                        .AsEnumerable()
                        .Select(v => (double?)v.valorVenda ?? 0)
                        .ToList();

                    produtosTiposLabels = db.cadVenda_produtos
                     .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                     .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Date == data.Date)
                     .GroupBy(x => x.vp.cdProduto)
                     .Select(g => db.cadProdutos
                                 .Where(p => p.cdProduto == g.Key)
                                 .Select(p => p.nmProduto)
                                 .FirstOrDefault() ?? "")
                     .ToList();

                    produtosTiposValores = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Date == data.Date)
                        .GroupBy(x => x.vp.cdProduto)
                        .Select(g => g.Sum(x => x.vp.quantidade ?? 0)) 
                        .ToList();

                    break;

                case 2: // Filtro por mês
                    quantidadeVendas = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year && a.dtVenda.Value.Month == data.Month)
                        .Count();

                    valorGanho = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year && a.dtVenda.Value.Month == data.Month)
                        .Sum(a => (double?)a.valorVenda ?? 0);

                    valorGasto = db.cadProdutos
                        .Where(p => p.dtCriacao.HasValue && p.dtCriacao.Value.Year == data.Year && p.dtCriacao.Value.Month == data.Month)
                        .Sum(p => (double?)p.valorCompra * p.quantidade) ?? 0;

                    lucroTotal = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year && a.dtVenda.Value.Month == data.Month)
                        .Sum(a => (double?)a.valorLucro ?? 0);

                    produtosVendidos = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year && x.v.dtVenda.Value.Month == data.Month)
                        .Sum(x => (int?)x.vp.quantidade);

                    graficoLabels = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Year == data.Year && v.dtVenda.Value.Month == data.Month)
                        .AsEnumerable()  
                        .GroupBy(v => v.dtVenda.Value.DayOfWeek)
                        .Select(g => $"Semana {g.Key}")
                        .ToList();

                    graficoValores = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Year == data.Year && v.dtVenda.Value.Month == data.Month)
                        .AsEnumerable()
                        .GroupBy(v => v.dtVenda.Value.DayOfWeek)
                        .Select(g => (double?)g.Sum(v => v.valorVenda) ?? 0)
                        .ToList();

                    produtosTiposLabels = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year && x.v.dtVenda.Value.Month == data.Month)
                        .GroupBy(x => x.vp.cdProduto)
                        .Select(g => db.cadProdutos
                                    .Where(p => p.cdProduto == g.Key)
                                    .Select(p => p.nmProduto)
                                    .FirstOrDefault() ?? "") 
                        .ToList();


                    produtosTiposValores = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year && x.v.dtVenda.Value.Month == data.Month)
                        .GroupBy(x => x.vp.cdProduto)
                        .Select(g => g.Sum(x => x.vp.quantidade ?? 0))
                        .ToList();

                    break;

                case 3: // Filtro por ano
                    quantidadeVendas = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year)
                        .Count();

                    valorGanho = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year)
                        .Sum(a => (double?)a.valorVenda ?? 0);

                    valorGasto = db.cadProdutos
                        .Where(p => p.dtCriacao.HasValue && p.dtCriacao.Value.Year == data.Year)
                        .Sum(p => (double?)p.valorCompra * p.quantidade) ?? 0;

                    lucroTotal = db.cadVenda
                        .Where(a => a.dtVenda.HasValue && a.dtVenda.Value.Year == data.Year)
                        .Sum(a => (double?)a.valorLucro ?? 0);

                    produtosVendidos = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year)
                        .Sum(x => (int?)x.vp.quantidade);

                    graficoLabels = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Year == data.Year)
                        .AsEnumerable()
                        .GroupBy(v => v.dtVenda.Value.Month)
                        .Select(g => g.Key.ToString())
                        .ToList();

                    graficoValores = db.cadVenda
                        .Where(v => v.dtVenda.HasValue && v.dtVenda.Value.Year == data.Year)
                        .AsEnumerable()
                        .GroupBy(v => v.dtVenda.Value.Month)
                        .Select(g => (double?)g.Sum(v => v.valorVenda) ?? 0)
                        .ToList();

                    produtosTiposLabels = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year && x.v.dtVenda.Value.Month == data.Month)
                        .GroupBy(x => x.vp.cdProduto)
                        .Select(g => db.cadProdutos
                                    .Where(p => p.cdProduto == g.Key)
                                    .Select(p => p.nmProduto)
                                    .FirstOrDefault() ?? "") 
                        .ToList();

                    produtosTiposValores = db.cadVenda_produtos
                        .Join(db.cadVenda, vp => vp.cdVenda, v => v.cdVenda, (vp, v) => new { vp, v })
                        .Where(x => x.v.dtVenda.HasValue && x.v.dtVenda.Value.Year == data.Year)
                        .GroupBy(x => x.vp.cdProduto)
                        .Select(g => g.Sum(x => x.vp.quantidade ?? 0))
                        .ToList();

                    break;
            }

            var resultado = new
            {
                quantidadeVendas,
                valorGanho,
                valorGasto,
                lucroTotal,
                produtosVendidos,
                graficoLabels,
                graficoValores,
                produtosTiposLabels,
                produtosTiposValores
            };

            return Json(resultado);
        }



    }
}

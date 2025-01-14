using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class ClienteController : Controller
    {
        private readonly controleEstoqueDBContext db;


        public ClienteController(controleEstoqueDBContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult List()
        {
            var draw = Request.Query["draw"].FirstOrDefault();
            var start = int.Parse(Request.Query["start"].FirstOrDefault() ?? "0");
            var length = int.Parse(Request.Query["length"].FirstOrDefault() ?? "10");
            var searchValue = Request.Query["search[value]"].FirstOrDefault();

            var totalRecords = db.cadCliente.Count();

            var query = db.cadCliente.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.nmCliente.Contains(searchValue));
            }

            var filteredRecords = query.Count();


            var fornecedores = query
                .OrderBy(f => f.cdCliente)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.cdCliente,
                    nome = f.nmCliente,
                    celular = f.numeroCelular,
                    endereco = string.Format($@"Cidade: {f.cidade}; Bairro: {f.bairro}; Rua: {f.rua}; Número: {f.numero}; Complemento: {f.complemento};"),
                    compras = f.cadVenda.Count()
                })
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = fornecedores
            });
        }
    }
}

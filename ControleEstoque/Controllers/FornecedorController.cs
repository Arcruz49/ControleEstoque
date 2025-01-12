using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly ControleEstoqueDbContext db;

        public FornecedorController(ControleEstoqueDbContext _db)
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

            var totalRecords = db.CadFornecedor.Count();

            var query = db.CadFornecedor.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.NmFornecedor.Contains(searchValue));
            }

            var filteredRecords = query.Count();

            
            var fornecedores = query
                .OrderBy(f => f.CdFornecedor) 
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.CdFornecedor,
                    nome = f.NmFornecedor,
                    dataCriacao = f.DtCriacao.HasValue ? f.DtCriacao.Value.ToString("dd/MM/yyyy") : null,
                    status = f.Ativo.HasValue ? (f.Ativo.Value ? "Ativo" : "Inativo") : "Indefinido"
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

        [HttpPost]
        public JsonResult Create(CadFornecedor fornecedor)
        {
            try
            {
                if(string.IsNullOrEmpty(fornecedor.NmFornecedor) || string.IsNullOrWhiteSpace(fornecedor.NmFornecedor)) return Json(new { success = false, message = "Nome Inválido" });

                fornecedor.DtCriacao = DateTime.Now;
                db.CadFornecedor.Add(fornecedor);
                db.SaveChanges();

                return Json(new { success = true, message = "Fornecedor adicionado com sucesso" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}

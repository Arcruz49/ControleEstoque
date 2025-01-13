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

                return Json(new { success = true, message = "Fornecedor adicionado com sucesso." });
            }
            catch(Exception ex) 
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Edit(int cd = 0)
        {
            try
            {

                if(cd == 0) return Json(new { success = false, message = "Fornedecor não identificado." });

                var fornecedor = db.CadFornecedor.Where(a => a.CdFornecedor == cd).FirstOrDefault();

                if(fornecedor == null) return Json(new { success = false, message = "Fornedecor não identificado." });

                return Json(new { success = true, nmFornecedor = fornecedor.NmFornecedor, cdFornecedor = fornecedor.CdFornecedor, status = fornecedor.Ativo });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(CadFornecedor fornecedor)
        {
            try
            {
                if (fornecedor.CdFornecedor == 0)
                    return Json(new { success = false, message = "Fornecedor não identificado." });

                var fornecedorExistente = db.CadFornecedor.FirstOrDefault(a => a.CdFornecedor == fornecedor.CdFornecedor);

                if (fornecedorExistente == null)
                    return Json(new { success = false, message = "Fornecedor não encontrado." });

                fornecedorExistente.NmFornecedor = fornecedor.NmFornecedor;
                fornecedorExistente.Ativo = fornecedor.Ativo;
                

                db.SaveChanges();

                return Json(new { success = true, message = "Fornecedor alterado."});

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        [HttpGet]
        public JsonResult GetFornecedoresAtivos()
        {
            try
            {
                var query = db.CadFornecedor
                              .Where(a => a.Ativo == true)
                              .Select(a => new { a.CdFornecedor, a.NmFornecedor }).ToList();

                return Json(new { success = true, message = query });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

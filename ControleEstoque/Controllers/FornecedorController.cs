using ControleEstoque.Contexts;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly ControleEstoqueContext db;

        public FornecedorController(ControleEstoqueContext _db)
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

            var totalRecords = db.cadFornecedors.Count();

            var query = db.cadFornecedors.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.nmFornecedor.Contains(searchValue));
            }

            var filteredRecords = query.Count();

            
            var fornecedores = query
                .OrderBy(f => f.cdFornecedor) 
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.cdFornecedor,
                    nome = f.nmFornecedor,
                    dataCriacao = f.dtCriacao.HasValue ? f.dtCriacao.Value.ToString("dd/MM/yyyy") : null,
                    status = f.ativo.HasValue ? (f.ativo.Value ? "Ativo" : "Inativo") : "Indefinido"
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
        public JsonResult Create(cadFornecedor fornecedor)
        {
            try
            {
                if(string.IsNullOrEmpty(fornecedor.nmFornecedor) || string.IsNullOrWhiteSpace(fornecedor.nmFornecedor)) return Json(new { success = false, message = "Nome Inválido" });

                fornecedor.dtCriacao = DateTime.Now;
                db.cadFornecedors.Add(fornecedor);
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

                var fornecedor = db.cadFornecedors.Where(a => a.cdFornecedor == cd).FirstOrDefault();

                if(fornecedor == null) return Json(new { success = false, message = "Fornedecor não identificado." });

                return Json(new { success = true, nmFornecedor = fornecedor.nmFornecedor, cdFornecedor = fornecedor.cdFornecedor, status = fornecedor.ativo });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(cadFornecedor fornecedor)
        {
            try
            {
                if (fornecedor.cdFornecedor == 0)
                    return Json(new { success = false, message = "Fornecedor não identificado." });

                var fornecedorExistente = db.cadFornecedors.FirstOrDefault(a => a.cdFornecedor == fornecedor.cdFornecedor);

                if (fornecedorExistente == null)
                    return Json(new { success = false, message = "Fornecedor não encontrado." });

                fornecedorExistente.nmFornecedor = fornecedor.nmFornecedor;
                fornecedorExistente.ativo = fornecedor.ativo;
                

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
                var query = db.cadFornecedors
                              .Where(a => a.ativo == true)
                              .Select(a => new { a.cdFornecedor, a.nmFornecedor }).ToList();

                return Json(new { success = true, message = query });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

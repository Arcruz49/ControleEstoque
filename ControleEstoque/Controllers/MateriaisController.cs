using ControleEstoque.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class MateriaisController : Controller
    {
        private readonly ControleEstoqueContext db;

        public MateriaisController(ControleEstoqueContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetTiposMateriais()
        {
            var tpMateriais = db.cadTipoMaterias.ToList();

            return Json(tpMateriais);
        }

        [HttpPost]
        public JsonResult SetTiposMateriais()
        {
            try
            {

                //implementar o insert na tabela cadMateriais

                return Json(new { success = true, message = "Tipos de Materiais salvos com sucesso" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

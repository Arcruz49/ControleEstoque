using ControleEstoque.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ControleEstoqueContext db;

        public PrincipalController(ControleEstoqueContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public JsonResult GetNomeSistema()
        {
            try
            {
                var nome = db.cadConfiguracaos.Select(a => a.nomeMenu).FirstOrDefault() ?? "Menu";
                return Json(new { success = true, message = nome });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetCorSistema()
        {
            try
            {
                var cor = db.cadConfiguracaos.Select(a => a.corFundoSistema).FirstOrDefault() ?? "#f8f4f4";
                return Json(new { success = true, message = cor });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}

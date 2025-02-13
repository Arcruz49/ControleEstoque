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
        public JsonResult getNomeSistema()
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
    }
}

using ControleEstoque.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var corFundo = db.cadConfiguracaos.Select(a => a.corFundoSistema).FirstOrDefault() ?? "#f8f4f4";
                var corPrincipal = db.cadConfiguracaos.Select(a => a.corSistema).FirstOrDefault() ?? "#fff";
                var corFonte = db.cadConfiguracaos.Select(a => a.corFonteTexto).FirstOrDefault() ?? "#000000";
                return Json(new { success = true, corFundo = corFundo, corPrincipal = corPrincipal, corFonte = corFonte });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetUtilizaMateriais()
        {
            var utiliza = db.cadConfiguracaos
                                  .Select(c => c.utilizaMateriais)
                                  .FirstOrDefault();

            return Json(new { success = (utiliza == true)});
        }
    }
}

using ControleEstoque.Contexts;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ControleEstoque.Controllers
{
    public class ConfiguracaoController : Controller
    {

        private readonly ControleEstoqueContext db;


        public ConfiguracaoController(ControleEstoqueContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var configuracao = db.cadConfiguracaos.FirstOrDefault(); 
            return View(configuracao);
        }

        [HttpPost]
        public JsonResult Edit(cadConfiguracao configuracao)
        {
            try
            {

                var configExistente = db.cadConfiguracaos.Where(a => a.cdConfiguracao == configuracao.cdConfiguracao).FirstOrDefault();

                if(configExistente == null) return Json(new { success = false, message = "Erro ao buscar as configurações." });

                if (configuracao.imgSistema == null) configuracao.imgSistema = configExistente.imgSistema;

                db.Entry(configExistente).CurrentValues.SetValues(configuracao);
                db.SaveChanges();

                if (configuracao.imgSistema != null)
                {
                    var imageBytes = configuracao.imgSistema;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "favicon.ico");
                    System.IO.File.WriteAllBytes(path, imageBytes);
                }

                return Json(new { success = true, message = "Configurações atualizadas"});

            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }
        }
    }
}

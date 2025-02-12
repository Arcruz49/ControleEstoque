using ControleEstoque.Contexts;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;

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
        public JsonResult Edit(cadConfiguracao configuracao, string imagemBase64)
        {
            try
            {

                var configExistente = db.cadConfiguracaos.Where(a => a.cdConfiguracao == configuracao.cdConfiguracao).FirstOrDefault();

                if(configExistente == null) return Json(new { success = false, message = "Erro ao buscar as configurações." });


                db.Entry(configExistente).CurrentValues.SetValues(configuracao);
                db.SaveChanges();


                return Json(new { success = true, message = "Configurações atualizadas"});

            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }
        }
    }
}

using ControleEstoque.Contexts;
using ControleEstoque.Models;
using ControleEstoque.Models.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ControleEstoque.Controllers
{
    public class FerramentaConsultaController : Controller
    {

        private readonly ControleEstoqueContext db;

        public FerramentaConsultaController(ControleEstoqueContext _db)
        {
            db = _db;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ExecutarBusca([FromBody] ResourceExecucaoConsultaModel model)
        {
            try
            {
                model.query = ValidaQuery(model);
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = model.query;
                    command.CommandType = System.Data.CommandType.Text;
                     
                    db.Database.OpenConnection(); 

                    using (var reader = command.ExecuteReader())
                    {
                        var resultado = new List<Dictionary<string, dynamic>>();

                        while (reader.Read())
                        {
                            var linha = new Dictionary<string, dynamic>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                linha[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            resultado.Add(linha);
                        }

                        return Json(new { success = true, message = "", dados = resultado });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public string ValidaQuery(ResourceExecucaoConsultaModel model)
        {
            if (model.query.ToUpper().Contains("UPDATE") || model.query.ToUpper().Contains("CREATE") || model.query.ToUpper().Contains("DROP")
                || model.query.ToUpper().Contains("TRUNCATE") || model.query.ToUpper().Contains("DELETE") || model.query.ToUpper().Contains("ALTER")) return "";


            foreach(var paramentro in model.parametros)
            {
                if(paramentro.tipo.Equals("varchar"))
                    model.query = model.query.Replace("{" + paramentro.nome + "}", "'"+paramentro.valor+"'");
                else if(paramentro.tipo.Equals("int"))
                    model.query = model.query.Replace("{" + paramentro.nome + "}", "" + paramentro.valor + "");

            }


            return model.query;
        }

    }
}

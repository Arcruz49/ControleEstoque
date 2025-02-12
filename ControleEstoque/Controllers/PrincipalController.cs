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
    }
}

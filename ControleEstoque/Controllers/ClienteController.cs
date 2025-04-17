using ControleEstoque.Contexts;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ControleEstoqueContext db;


        public ClienteController(ControleEstoqueContext _db)
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

            var totalRecords = db.cadClientes.Count();

            var query = db.cadClientes.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.nmCliente.Contains(searchValue));
            }

            var filteredRecords = query.Count();


            var clientes = query
                .OrderBy(f => f.cdCliente)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.cdCliente,
                    nome = f.nmCliente,
                    cpf = f.cpf,
                    celular = f.numeroCelular,
                    endereco = string.Format($@"Cidade: {f.cidade}; Bairro: {f.bairro}; Rua: {f.rua}; Número: {f.numero}; Complemento: {f.complemento};"),
                    compras = f.cadVenda.Count()
                })
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = clientes
            });
        }


        [HttpPost]
        public JsonResult Create(cadCliente cliente)
        {
            try
            {
                if (string.IsNullOrEmpty(cliente.nmCliente) || string.IsNullOrWhiteSpace(cliente.nmCliente)) return Json(new { success = false, message = "Nome inválido" });
                if(cliente.cpf.Length != 14) return Json(new { success = false, message = "CPF inválido" });
                db.cadClientes.Add(cliente);
                db.SaveChanges();

                return Json(new { success = true, message = "Cliente adicionado com sucesso." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Edit(int cdCliente = 0)
        {
            try
            {
                if(cdCliente == 0) return Json(new { success = false, message = "Cliente não encontrado"});

                var cliente = db.cadClientes.Where(a => a.cdCliente == cdCliente).FirstOrDefault();
                
                if(cliente == null) return Json(new { success = false, message = "Cliente não encontrado" });

                return Json(new { success = true, message = cliente});
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(cadCliente cliente)
        {
            try
            {
                if (cliente == null) return Json(new { success = false, message = "Cliente não identificado" });
                if (cliente.cdCliente == 0) return Json(new { success = false, message = "Cliente não identificado" });
                if (string.IsNullOrWhiteSpace(cliente.nmCliente)) return Json(new { success = false, message = "Nome inválido" });

                var clienteAntigo = db.cadClientes.Find(cliente.cdCliente);
                if (clienteAntigo == null) return Json(new { success = false, message = "Cliente não identificado" });

                clienteAntigo.nmCliente = cliente.nmCliente;
                clienteAntigo.numeroCelular = cliente.numeroCelular;
                clienteAntigo.cidade = cliente.cidade;
                clienteAntigo.bairro = cliente.bairro;
                clienteAntigo.rua = cliente.rua;
                clienteAntigo.numero = cliente.numero;
                clienteAntigo.complemento = cliente.complemento;

                db.Entry(clienteAntigo).State = EntityState.Modified; 
                db.SaveChanges();

                return Json(new { success = true, message = "Cliente editado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetClientes(string search)
        {
            try
            {
                var clientes = db.cadClientes
                    .Where(c => string.IsNullOrEmpty(search) || c.nmCliente.Contains(search)) 
                    .Select(c => new { id = c.cdCliente, text = c.nmCliente, cpf = c.cpf }) 
                    .OrderBy(c => c.text)
                    .ToList();

                return Json(new
                {
                    success = true,
                    results = clientes
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}

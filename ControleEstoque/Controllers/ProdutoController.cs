using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ControleEstoqueDbContext db;


        public ProdutoController(ControleEstoqueDbContext _db)
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

            var totalRecords = db.CadFornecedor.Count();

            var query = db.CadProduto.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.NmProduto.Contains(searchValue));
            }

            var filteredRecords = query.Count();


            var produtos = query
                .OrderBy(f => f.CdProduto)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.CdProduto,
                    nome = f.NmProduto,
                    descricao = f.DsProduto,
                    valorCompra = f.ValorCompra,
                    valorVenda = f.ValorVenda,
                    dataCriacao = f.DtCriacao.HasValue ? f.DtCriacao.Value.ToString("dd/MM/yyyy") : null,
                    quantidade = f.Quantidade,
                    tamanho = f.Tamanho,
                    fornecedor = f.CdFornecedorNavigation.NmFornecedor,
                })
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = produtos
            });
        }


        [HttpPost]
        public JsonResult Create(CadProduto produto)
        {
            try
            {
                #region validações
                
                if (string.IsNullOrEmpty(produto.NmProduto) || string.IsNullOrWhiteSpace(produto.NmProduto)) return Json(new { success = false, message = "Nome Inválido" });
                if (string.IsNullOrEmpty(produto.Tamanho) || string.IsNullOrWhiteSpace(produto.Tamanho)) return Json(new { success = false, message = "Tamanho Inválido" });
                if (produto.ValorCompra == null) return Json(new { success = false, message = "Valor de compra Inválido" });
                if (produto.ValorVenda == null) return Json(new { success = false, message = "Valor de venda Inválido" });
                if (produto.Quantidade == null) return Json(new { success = false, message = "Quantidade Inválida" });
                if (produto.CdFornecedor == 0) return Json(new { success = false, message = "Fornecedor Inválido" });

                #endregion
                produto.DtCriacao = DateTime.Now;
                db.CadProduto.Add(produto);
                db.SaveChanges();

                return Json(new { success = true, message = "Produto adicionado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Edit(int cd = 0)
        {
            try
            {
                if (cd == 0) return Json(new { success = false, message = "Produto não identificado." });

                var produto = db.CadProduto.Where(a => a.CdProduto == cd).FirstOrDefault();

                if (produto == null) return Json(new { success = false, message = "Produto não identificado." });

                return Json(new
                {
                    success = true,
                    cdProduto = produto.CdProduto,
                    nmProduto = produto.NmProduto,
                    dsProduto = produto.DsProduto,
                    valorCompra = produto.ValorCompra,
                    valorVenda = produto.ValorVenda,
                    quantidade = produto.Quantidade,
                    tamanho = produto.Tamanho,
                    fornecedor = produto.CdFornecedor
                });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(CadProduto produto)
        {
            try
            {
                if (produto.CdProduto == 0)
                    return Json(new { success = false, message = "Produto não identificado." });

                var produtoExistente = db.CadProduto.FirstOrDefault(a => a.CdProduto == produto.CdProduto);

                if (produtoExistente == null)
                    return Json(new { success = false, message = "Produto não encontrado." });

                if (string.IsNullOrEmpty(produto.NmProduto) || string.IsNullOrWhiteSpace(produto.NmProduto)) return Json(new { success = false, message = "Nome Inválido" });
                if (string.IsNullOrEmpty(produto.Tamanho) || string.IsNullOrWhiteSpace(produto.Tamanho)) return Json(new { success = false, message = "Tamanho Inválido" });
                if (produto.ValorCompra == null) return Json(new { success = false, message = "Valor de compra Inválido" });
                if (produto.ValorVenda == null) return Json(new { success = false, message = "Valor de venda Inválido" });
                if (produto.Quantidade == null) return Json(new { success = false, message = "Quantidade Inválida" });
                if (produto.CdFornecedor == 0) return Json(new { success = false, message = "Fornecedor Inválido" });

                produtoExistente.NmProduto = produto.NmProduto;
                produtoExistente.DsProduto = produto.DsProduto;
                produtoExistente.ValorCompra = produto.ValorCompra;
                produtoExistente.ValorVenda = produto.ValorVenda;
                produtoExistente.Quantidade = produto.Quantidade;
                produtoExistente.Tamanho = produto.Tamanho;
                produtoExistente.CdFornecedor = produto.CdFornecedor;


                db.SaveChanges();

                return Json(new { success = true, message = "Produto alterado." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

    }
}

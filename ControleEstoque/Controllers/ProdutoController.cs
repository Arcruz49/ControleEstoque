using ControleEstoque.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly controleEstoqueDBContext db;


        public ProdutoController(controleEstoqueDBContext _db)
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

            var totalRecords = db.cadProduto.Count();

            var query = db.cadProduto.AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.nmProduto.Contains(searchValue));
            }

            var filteredRecords = query.Count();


            var produtos = query
                .OrderBy(f => f.cdProduto)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.cdProduto,
                    nome = f.nmProduto,
                    descricao = f.dsProduto,
                    valorCompra = f.valorCompra,
                    valorVenda = f.valorVenda,
                    dataCriacao = f.dtCriacao.HasValue ? f.dtCriacao.Value.ToString("dd/MM/yyyy") : null,
                    quantidade = f.quantidade,
                    tamanho = f.tamanho,
                    fornecedor = f.cdFornecedorNavigation.nmFornecedor,
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
        public JsonResult Create(cadProduto produto)
        {
            try
            {
                #region validações
                
                if (string.IsNullOrEmpty(produto.nmProduto) || string.IsNullOrWhiteSpace(produto.nmProduto)) return Json(new { success = false, message = "Nome Inválido" });
                if (string.IsNullOrEmpty(produto.tamanho) || string.IsNullOrWhiteSpace(produto.tamanho)) return Json(new { success = false, message = "tamanho Inválido" });
                if (produto.valorCompra == null) return Json(new { success = false, message = "Valor de compra Inválido" });
                if (produto.valorVenda == null) return Json(new { success = false, message = "Valor de venda Inválido" });
                if (produto.quantidade == null) return Json(new { success = false, message = "quantidade Inválida" });
                if (produto.cdFornecedor == 0) return Json(new { success = false, message = "Fornecedor Inválido" });

                #endregion
                produto.dtCriacao = DateTime.Now;
                db.cadProduto.Add(produto);
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

                var produto = db.cadProduto.Where(a => a.cdProduto == cd).FirstOrDefault();

                if (produto == null) return Json(new { success = false, message = "Produto não identificado." });

                return Json(new
                {
                    success = true,
                    cdProduto = produto.cdProduto,
                    nmProduto = produto.nmProduto,
                    dsProduto = produto.dsProduto,
                    valorCompra = produto.valorCompra,
                    valorVenda = produto.valorVenda,
                    quantidade = produto.quantidade,
                    tamanho = produto.tamanho,
                    fornecedor = produto.cdFornecedor
                });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(cadProduto produto)
        {
            try
            {
                if (produto.cdProduto == 0)
                    return Json(new { success = false, message = "Produto não identificado." });

                var produtoExistente = db.cadProduto.FirstOrDefault(a => a.cdProduto == produto.cdProduto);

                if (produtoExistente == null)
                    return Json(new { success = false, message = "Produto não encontrado." });

                if (string.IsNullOrEmpty(produto.nmProduto) || string.IsNullOrWhiteSpace(produto.nmProduto)) return Json(new { success = false, message = "Nome Inválido" });
                if (string.IsNullOrEmpty(produto.tamanho) || string.IsNullOrWhiteSpace(produto.tamanho)) return Json(new { success = false, message = "tamanho Inválido" });
                if (produto.valorCompra == null) return Json(new { success = false, message = "Valor de compra Inválido" });
                if (produto.valorVenda == null) return Json(new { success = false, message = "Valor de venda Inválido" });
                if (produto.quantidade == null) return Json(new { success = false, message = "quantidade Inválida" });
                if (produto.cdFornecedor == 0) return Json(new { success = false, message = "Fornecedor Inválido" });

                produtoExistente.nmProduto = produto.nmProduto;
                produtoExistente.dsProduto = produto.dsProduto;
                produtoExistente.valorCompra = produto.valorCompra;
                produtoExistente.valorVenda = produto.valorVenda;
                produtoExistente.quantidade = produto.quantidade;
                produtoExistente.tamanho = produto.tamanho;
                produtoExistente.cdFornecedor = produto.cdFornecedor;


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

using ControleEstoque.Contexts;
using ControleEstoque.Models;
using ControleEstoque.Models.Resource;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ControleEstoque.Controllers
{
    public class VendaController : Controller
    {
        private readonly ControleEstoqueContext db;

        public VendaController(ControleEstoqueContext _db)
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

            var totalRecords = db.cadVenda.Count();

            var query = db.cadVenda.AsQueryable();
            query = query.OrderByDescending(a => a.dtVenda);


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(f => f.cdClienteNavigation.nmCliente.Contains(searchValue));
            }

            var filteredRecords = query.Count();


            var fornecedores = query
                .OrderBy(f => f.cdCliente)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    id = f.cdVenda,
                    nomeCliente = f.cdClienteNavigation.nmCliente,
                    valorVenda = f.valorVenda,
                    valorLucro = f.valorLucro,
                    dtVenda = f.dtVenda.HasValue ? f.dtVenda.Value.ToString("dd/MM/yyyy") : null,
                    qtdProdutos = f.cadVenda_produtos.Sum(vp => vp.quantidade)
                })
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = fornecedores
            });
        }

        [HttpPost]
        public JsonResult Create(string dados)
        {
            try
            {
                var resourceVenda = JsonConvert.DeserializeObject<ResourceVenda>(dados);

                #region validações básicas
                
                if (resourceVenda == null) return Json(new { success = false, message = "Erro interno." });
                if(resourceVenda.cdCliente == 0) return Json(new { success = false, message = "Cliente não encontrado" });
                if(resourceVenda.dataVenda == default) return Json(new { success = false, message = "Data inválida" });
                if(resourceVenda.produtos == null || !resourceVenda.produtos.Any()) return Json(new { success = false, message = "Pordutos devem ser selecionados" });

                #endregion

                #region valida a Quantidade de produtos

                foreach (var item in resourceVenda.produtos)
                {
                    var produto = (from a in db.cadProdutos
                                   where a.cdProduto == item.cdProduto
                                   select a).FirstOrDefault();
                    if (produto == null) return Json(new { success = false, message = string.Format("Produto não encontrado: " + item.cdProduto) });

                    if (produto.quantidade < item.quantidade || produto.quantidade == 0) return Json(new { success = false, message = string.Format($@"Quantidade de {produto.nmProduto} indisponível no estoque. Total disponível: {produto.quantidade}") });

                }

                #endregion

                var valorTotal = resourceVenda.produtos.Sum(p => p.preco * p.quantidade);



                var valorGasto = resourceVenda.produtos.Sum(p =>
                {
                    var produto = db.cadProdutos.First(a => a.cdProduto == p.cdProduto);
                    return produto.valorCompra * p.quantidade;
                });
                
                cadVendum cadVenda = new cadVendum
                {
                    cdCliente = resourceVenda.cdCliente,
                    valorVenda = valorTotal,
                    dtVenda = resourceVenda.dataVenda,
                    valorLucro = valorTotal - valorGasto
                };

                db.cadVenda.Add(cadVenda);
                db.SaveChanges();


                foreach (var item in resourceVenda.produtos)
                {
                    cadVenda_produto vendaProduto = new cadVenda_produto
                    {
                        cdVenda = cadVenda.cdVenda, 
                        cdProduto = item.cdProduto,
                        quantidade = item.quantidade
                    };

                    // Atualiza o estoque do produto
                    var produto = db.cadProdutos.First(a => a.cdProduto == item.cdProduto);
                    produto.quantidade -= item.quantidade;

                    db.cadVenda_produtos.Add(vendaProduto);
                }

                db.SaveChanges();


                return Json(new { success = true, message = "Venda registrada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Edit(int cdVenda)
        {
            try
            {
                if (cdVenda <= 0)
                {
                    return Json(new { success = false, message = "Venda não encontrada." });
                }

                // Busca a venda no banco
                var venda = db.cadVenda
                    .Where(v => v.cdVenda == cdVenda)
                    .Select(v => new
                    {
                        v.cdVenda,
                        v.cdClienteNavigation.nmCliente,
                        v.valorVenda,
                        v.valorLucro,
                        v.dtVenda,
                        produtos = v.cadVenda_produtos.Select(p => new
                        {
                            p.cdProduto,
                            p.quantidade,
                            p.cdProdutoNavigation.nmProduto,
                            p.cdProdutoNavigation.tamanho,
                            preco = db.cadProdutos.FirstOrDefault(prod => prod.cdProduto == p.cdProduto).valorVenda
                        }).ToList()
                    })
                    .FirstOrDefault();

                if (venda == null)
                {
                    return Json(new { success = false, message = "Venda não encontrada." });
                }

                return Json(new { success = true, data = venda });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



    }
}

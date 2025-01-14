using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadProduto
{
    public int cdProduto { get; set; }

    public string? nmProduto { get; set; }

    public string? dsProduto { get; set; }

    public decimal? valorCompra { get; set; }

    public decimal? valorVenda { get; set; }

    public DateTime? dtCriacao { get; set; }

    public int? quantidade { get; set; }

    public string? tamanho { get; set; }

    public int cdFornecedor { get; set; }

    public virtual ICollection<cadVenda_produto> cadVenda_produtos { get; set; } = new List<cadVenda_produto>();

    public virtual cadFornecedor cdFornecedorNavigation { get; set; } = null!;
}

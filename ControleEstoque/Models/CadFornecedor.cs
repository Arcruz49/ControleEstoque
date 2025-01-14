using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadFornecedor
{
    public int cdFornecedor { get; set; }

    public string? nmFornecedor { get; set; }

    public DateTime? dtCriacao { get; set; }

    public bool? ativo { get; set; }

    public virtual ICollection<cadProduto> cadProdutos { get; set; } = new List<cadProduto>();
}

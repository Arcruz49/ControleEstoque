using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class CadFornecedor
{
    public int CdFornecedor { get; set; }

    public string? NmFornecedor { get; set; }

    public DateTime? DtCriacao { get; set; }

    public bool? Ativo { get; set; }

    public virtual ICollection<CadProduto> CadProdutos { get; set; } = new List<CadProduto>();
}

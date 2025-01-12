using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class CadVenda
{
    public int CdVenda { get; set; }

    public string? NmComprador { get; set; }

    public DateTime? DtVenda { get; set; }

    public decimal? ValorVenda { get; set; }

    public decimal? ValorLucro { get; set; }

    public virtual ICollection<CadVendaProduto> CadVendaProdutos { get; set; } = new List<CadVendaProduto>();
}

using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class CadProduto
{
    public int CdProduto { get; set; }

    public string? NmProduto { get; set; }

    public string? DsProduto { get; set; }

    public decimal? ValorCompra { get; set; }

    public decimal? ValorVenda { get; set; }

    public DateTime? DtCriacao { get; set; }

    public int? Quantidade { get; set; }

    public string? Tamanho { get; set; }

    public int CdFornecedor { get; set; }

    public virtual ICollection<CadVendaProduto> CadVendaProdutos { get; set; } = new List<CadVendaProduto>();

    public virtual CadFornecedor CdFornecedorNavigation { get; set; } = null!;
}

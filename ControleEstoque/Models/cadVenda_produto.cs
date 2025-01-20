using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadVenda_produto
{
    public int cdVendaProduto { get; set; }

    public int cdVenda { get; set; }

    public int cdProduto { get; set; }

    public int? quantidade { get; set; }

    public virtual cadProduto cdProdutoNavigation { get; set; } = null!;

    public virtual cadVendum cdVendaNavigation { get; set; } = null!;
}

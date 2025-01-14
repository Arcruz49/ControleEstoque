﻿using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class CadVendaProduto
{
    public int CdVendaProduto { get; set; }

    public int CdVenda { get; set; }

    public int cdProduto { get; set; }

    public virtual cadProduto cdProdutoNavigation { get; set; } = null!;

    public virtual CadVenda CdVendaNavigation { get; set; } = null!;
}

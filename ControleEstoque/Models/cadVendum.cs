using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadVendum
{
    public int cdVenda { get; set; }

    public int? cdCliente { get; set; }

    public DateTime? dtVenda { get; set; }

    public decimal? valorVenda { get; set; }

    public decimal? valorLucro { get; set; }

    public virtual ICollection<cadVenda_produto> cadVenda_produtos { get; set; } = new List<cadVenda_produto>();

    public virtual cadCliente? cdClienteNavigation { get; set; }
}

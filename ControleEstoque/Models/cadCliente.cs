using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadCliente
{
    public int cdCliente { get; set; }

    public string? nmCliente { get; set; }

    public string? numeroCelular { get; set; }

    public string? cidade { get; set; }

    public string? bairro { get; set; }

    public string? rua { get; set; }

    public string? numero { get; set; }

    public string? complemento { get; set; }

    public virtual ICollection<cadVendum> cadVenda { get; set; } = new List<cadVendum>();
}

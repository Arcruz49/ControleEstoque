using System;
using System.Collections.Generic;

namespace ControleEstoque.Models;

public partial class cadConfiguracao
{
    public int cdConfiguracao { get; set; }

    public string? nomeMenu { get; set; }

    public byte[]? imgSistema { get; set; }

    public string? corFundoSistema { get; set; }

    public string? corSistema { get; set; }

    public string? corFonteTexto { get; set; }
}

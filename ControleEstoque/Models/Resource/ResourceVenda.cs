namespace ControleEstoque.Models.Resource
{
    public class ResourceVenda
    {
        public int cdCliente { get; set; } // Nome deve bater com o JSON
        public DateTime dataVenda { get; set; }
        public List<ResourceProduto> produtos { get; set; } // Nome consistente
    }
}

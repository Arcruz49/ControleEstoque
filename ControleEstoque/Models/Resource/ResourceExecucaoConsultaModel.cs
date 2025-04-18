namespace ControleEstoque.Models.Resource
{
    public class ResourceExecucaoConsultaModel
    {
        public string query { get; set; }
        public List<ResourceParametro> parametros { get; set; }
    }
}

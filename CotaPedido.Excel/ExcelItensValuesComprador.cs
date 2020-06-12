namespace CotaPedido.Excel
{
    public class ExcelItensValuesComprador
    {
        public string Produto { get; set; }

        public int? CategoriaId { get; set; }
        public string Categoria { get; set; }

        public int? SubCategoriaId { get; set; }
        public string SubCategoria { get; set; }

        public int? UnidadeId { get; set; }
        public string Unidade { get; set; }

        public int Quantidade { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(Produto) &&
                    CategoriaId.HasValue &&
                    !string.IsNullOrEmpty(Categoria) &&
                    SubCategoriaId.HasValue &&
                    !string.IsNullOrEmpty(SubCategoria) &&
                    UnidadeId.HasValue &&
                    !string.IsNullOrEmpty(Unidade) &&
                    Quantidade > 0;
            }
        }
    }
}

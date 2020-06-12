namespace CotaPedido.Excel
{
    public class ExcelItensValues
    {
        public int Id { get; set; }
        public int IdItem { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal
        {
            get
            {
                return Quantidade * ValorUnitario;
            }
        }
    }
}

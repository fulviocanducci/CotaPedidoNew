using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
namespace CotaPedido.Excel
{
    public class ImportFromExcel
    {        
        public IEnumerable<ExcelItensValues> Render(Stream stream)
        {
            IExcelDataReader reader;
            try
            {
                reader = ExcelReaderFactory.CreateReader(stream);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }                       
            if (reader.Read())
            {
                if (reader.GetString(0) == "Id" &&
                            reader.GetString(1) == "Item" &&
                            reader.GetString(5) == "Quantidade" &&
                            reader.GetString(6) == "Valor Unitário")
                {
                    while (reader.Read())
                    {
                        yield return new ExcelItensValues
                        {
                            Id = (int)reader.GetDouble(0),
                            IdItem = (int)reader.GetDouble(1),
                            Quantidade = (int)reader.GetDouble(5),
                            ValorUnitario = (decimal)reader.GetDouble(6),
                        };
                    }
                }
            }
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                reader = null;                    
            }
        }

        public IEnumerable<ExcelItensValues> Render(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                return Render(stream);   
            }
        }
    }
}

using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;

namespace CotaPedido.Excel
{   
    public class ImportFromExcelToComprador
    {
        public IEnumerable<ExcelItensValuesComprador> Render(Stream stream)
        {
            IExcelDataReader reader;
            try
            {
                reader = ExcelReaderFactory.CreateReader(stream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (reader.Read())
            {
                if (reader.GetString(0) == "Produto" &&
                            reader.GetString(1) == "Categoria" &&
                            reader.GetString(2) == "SubCategoria" &&
                            reader.GetString(3) == "Unidade" &&
                            reader.GetString(4) == "Quantidade")
                {
                    while (reader.Read())
                    {
                        yield return new ExcelItensValuesComprador
                        {
                            Produto = reader.GetString(0),
                            Categoria = reader.GetString(1),
                            SubCategoria = reader.GetString(2),
                            Unidade = reader.GetString(3),
                            Quantidade = (int)reader.GetDouble(4)
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

        public IEnumerable<ExcelItensValuesComprador> Render(string fileName)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                return Render(stream);
            }
        }
    }
}

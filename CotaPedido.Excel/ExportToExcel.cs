using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CotaPedido.Excel
{  

    public class ExportToExcel
    {
        private IWorkbook Workbook { get; }
        private ISheet Sheet { get; }
        private IRow Row { get; set; }
        private ICell Cell { get; set; }
        private int[] Widths { get; set; }
        private CellType[] CellTypes { get; set; }
        private string[] CellTypesParse { get; set; }

        public ExportToExcel(string name)
        {
            Workbook = new XSSFWorkbook();
            Sheet = Workbook.CreateSheet(name);
        }

        public ExportToExcel SetCellTypesParse(params string[] cellTypesParse)
        {
            CellTypesParse = cellTypesParse;
            return this;
        }

        private void AutoSizeColumn(int index)
        {
            //Sheet.AutoSizeColumn(index);            
        }

        public ExportToExcel Header(params string[] headers)
        {
            var headersIndex = headers
                .Select((x, y) => new
                {
                    Name = x,
                    Index = y
                })
            .ToList();

            Row = Sheet.CreateRow(0);
            Sheet.HorizontallyCenter = true;
            Sheet.VerticallyCenter = true;            
            ICellStyle style = Workbook.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.Black.Index;            
            //style.FillBackgroundColor = HSSFColor.Grey25Percent.Index;
            //style.FillPattern = FillPattern.Diamonds;

            foreach (var header in headersIndex)
            {
                Cell = Row.CreateCell(header.Index);
                Cell.SetCellType(CellType.String);
                Cell.SetCellValue(header.Name);
                Cell.CellStyle = style;
                Cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                Cell.CellStyle.Alignment = HorizontalAlignment.Center;                
                Cell.CellStyle.BorderBottom = BorderStyle.Thin;
                Cell.CellStyle.BorderLeft = BorderStyle.Thin;
                Cell.CellStyle.BorderRight = BorderStyle.Thin;
                Cell.CellStyle.BorderTop = BorderStyle.Thin;
                Cell.CellStyle.WrapText = false;
                AutoSizeColumn(header.Index);
            }
            
            return this;
        }

        public MemoryStream Render(IEnumerable items, params string[] fields)
        {
            CreateRows(items, fields);
            if (Widths != null)
            {
                foreach (var w in Widths.Select((x, y) => new { x, y }).ToList())
                {
                    Sheet.SetColumnWidth(w.y, w.x);                    
                }
            }
            MemoryStream result = new MemoryStream();
            Workbook.Write(result);
            return result;
        }

        private ExportToExcel CreateRows(IEnumerable items, params string[] fields)
        {
            IDataFormat format = Workbook.CreateDataFormat();

            var fieldsIndex = fields
                .Select((x, y) => new
                {
                    Name = x,
                    Index = y
                })
            .ToList();
                                   
            IEnumerator values = items.GetEnumerator();
            while (values.MoveNext())
            {
                Type typeFields = values.Current.GetType();                  
                Row = Sheet.CreateRow(Sheet.PhysicalNumberOfRows);

                foreach (PropertyInfo item in typeFields.GetProperties())
                {
                    if (fields.Where(x => x == item.Name).Any())
                    {
                        var index = fieldsIndex.Where(x => x.Name == item.Name)
                            .Select(a => a.Index)
                            .FirstOrDefault();
                        var cell = Row.CreateCell(index);
                        if (CellTypes != null && CellTypes.Count() == fieldsIndex.Count())
                        {
                            cell.SetCellType(CellTypes[index]);
                        }
                        if (CellTypesParse != null && CellTypesParse.Count() == fieldsIndex.Count())
                        {                            
                            switch (CellTypesParse[index])
                            {
                                case "i": //int
                                    {
                                        cell.SetCellValue(int.Parse(item.GetValue(values.Current).ToString()));
                                        break;
                                    }
                                case "d": //double
                                    {
                                        cell.SetCellValue(double.Parse(item.GetValue(values.Current).ToString()));                                        
                                        cell.CellStyle.DataFormat = 4;                                        
                                        //cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#.##0,00");                                        
                                        //cell.CellStyle.DataFormat = format.GetFormat("#.##0,00");
                                        break;
                                    }
                                case "s": //string
                                case "":
                                    {
                                        cell.SetCellValue(item.GetValue(values.Current).ToString());
                                        break;
                                    }
                            }                            
                        }
                        else
                        {
                            cell.SetCellValue(item.GetValue(values.Current).ToString());                            
                        }
                        cell.CellStyle.WrapText = false;
                    }
                }                
            }
            return this;
        }

        public ExportToExcel SetWidths(params int[] widths)
        {
            Widths = widths;
            return this;
        }

        public ExportToExcel SetTypes(params CellType[] types)
        {
            CellTypes = types;
            return this;
        }
    }
}

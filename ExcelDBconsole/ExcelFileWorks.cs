using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ExcelDBconsole
{
    public static class ExcelFileWorks
    {
        public static List<List<object>> GetDataFromExcelFile(string path)
        {
            var outcome_list = new List<List<object>>();
            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheets.First();
                var range = ws.RangeUsed();
                if (range != null)
                {
                    foreach (var cell in range.Cells())
                    {
                        var value = cell.Value;
                        if (!cell.Value.IsBlank)
                        {
                            outcome_list.Add( new List<object>{ cell.Address.ColumnLetter, cell.Address.RowNumber, value });
                        }
                    }
                }
                else { throw new ArgumentException($"there's no woekshits in {path}"); }
            }
            return outcome_list;
        }
        public static void SaveDataToExcel(string path, List<List<object>> data, string worksheet_name) {
            using (var worlbook = new XLWorkbook()) {
                var worksheet = worlbook.Worksheets.Add(worksheet_name);
                foreach (var cell in data) {
                    string? columm = Convert.ToString(cell[0]);
                    string? row_num = Convert.ToString(cell[1]);
                    string? cell_data = Convert.ToString(cell[2]);
                    if (columm != null && cell_data != null && cell_data!=null) {
                        worksheet.Cell(columm + row_num).Value = cell_data;
                    }
                }
                worlbook.SaveAs(path);
            }
        }
    }
}

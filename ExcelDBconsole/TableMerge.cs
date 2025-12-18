using System;
using System.Collections.Generic;
using System.Linq;
namespace ExcelBDConsol
{
    public static class TableMerge
    {
        //главная функция объединения двух таблиц на основе общих данных
        public static List<List<object>> MergeTables(List<List<object>> table1, List<List<object>> table2)
        {
            //проверка на общие элементы
            var flagCommon = HatChecker(table1, table2);
            if (flagCommon.Count == 0)
            {
                throw new ArgumentException("There is no shared data");
            }
            //проверка исключений
            var exceptionResult = ExceptionProcess(table1, table2);
            if (exceptionResult != null)
            {
                return exceptionResult;
            }
            var all_headers = SelectHeaders(table1, table2); //сначала нужно получать словарь всех заголовков
            var mergedTable = CreateNewTable(table1, table2, all_headers); //потом уже создавать новую таблицу
            return mergedTable;
        }

        //функция для выделения шапки
        private static List<Tuple<string, string>> HatHunter(List<List<object>> table)
        {
            var hat = new List<Tuple<string, string>>();

            foreach (var box in table)
            {
                if (box.Count > 1 && box[1] is int && (int)box[1] == 1)
                {
                    string currId = box[0]?.ToString() ?? "A";
                    for (int i = 2; i < box.Count; i++)
                    {
                        var headerValue = box[i]?.ToString();
                        if (!string.IsNullOrEmpty(headerValue))
                        {
                            hat.Add(new Tuple<string, string>(currId, headerValue));
                            currId = IdCont(currId);
                        }
                    }
                }
            }
            return hat;
        }

        //функция для сравнения шапок
        private static List<Tuple<string, string>> HatChecker(List<List<object>> table1, List<List<object>> table2)
        {
            var hat1 = HatHunter(table1);
            var hat2 = HatHunter(table2);
            var matches = new List<Tuple<string, string>>();
            foreach (var ha in hat1)
            {
                string a = ha.Item2;
                foreach (var hb in hat2)
                {
                    string b = hb.Item2;
                    if (a == b)
                    {
                        matches.Add(new Tuple<string, string>(ha.Item1, hb.Item1));
                    }
                }
            }
            return matches;
        }

        //ВЫДЕЛЕНИЕ ЗАГОЛОВКОВ
        //функция для выделения всех заголовков в шапке в один словарь
        private static Dictionary<string, HeaderInfo> SelectHeaders(List<List<object>> table1, List<List<object>> table2)
        {
            var headersDict = new Dictionary<string, HeaderInfo>();
            ProcessTableHeaders(table1, headersDict, "Table1");
            ProcessTableHeaders(table2, headersDict, "Table2");
            return headersDict;
        }
        //функция для обработки таблицы
        private static void ProcessTableHeaders(List<List<object>> table, Dictionary<string, HeaderInfo> headersDict, string tableName)
        {
            var tableHeaders = HatHunter(table);
            foreach (var header in tableHeaders)
            {
                string headerName = header.Item2;
                string headerId = header.Item1;
                if (!headersDict.ContainsKey(headerName))
                {
                    headersDict[headerName] = new HeaderInfo();
                }
                if (tableName == "Table1")
                {
                    headersDict[headerName].Table1Id = headerId;
                }
                else
                {
                    headersDict[headerName].Table2Id = headerId;
                }
            }
        }
        //класс для хранения заголовков и значений
        private class HeaderInfo
        {
            public string Table1Id { get; set; }
            public string Table2Id { get; set; }
        }

        //СОЗДАНИЕ ОБЪЕДИНЁННОЙ ТАБЛИЦЫ
        //функция создания новой мёрджнутой таблицы 
        private static List<List<object>> CreateNewTable(List<List<object>> table1, List<List<object>> table2, Dictionary<string, HeaderInfo> allUniqHeaders)
        {
            var newTable = new List<List<object>>();
            //заполнение массивов уникальных заголовков
            var uniqueHeaders1 = new List<string>();
            var uniqueHeaders2 = new List<string>();
            var commonHeaders = new List<string>();
            foreach (var kvp in allUniqHeaders)
            {
                string header = kvp.Key;
                var info = kvp.Value;
                if (!string.IsNullOrEmpty(info.Table1Id) && !string.IsNullOrEmpty(info.Table2Id))
                {
                    commonHeaders.Add(header);
                }
                else if (!string.IsNullOrEmpty(info.Table1Id) && string.IsNullOrEmpty(info.Table2Id))
                {
                    uniqueHeaders1.Add(header);
                }
                else if (string.IsNullOrEmpty(info.Table1Id) && !string.IsNullOrEmpty(info.Table2Id))
                {
                    uniqueHeaders2.Add(header);
                }
            }
            //заполнение массивов уникальных заголовков
            var allHeaders = new List<string>();
            allHeaders.AddRange(commonHeaders);
            allHeaders.AddRange(uniqueHeaders1);
            allHeaders.AddRange(uniqueHeaders2);
            //составление шапки таблицы
            string currId = "A";
            foreach (var header in allHeaders)
            {
                newTable.Add(new List<object> { currId, 1, header });
                currId = IdCont(currId);
            }
            //заполнение словарей заголовков с их ID
            var table1IdHeader = BuildHeaderIdDictionary(table1);
            var table2IdHeader = BuildHeaderIdDictionary(table2);
            //максимальные номера строк
            int maxRowNumTable1 = GetMaxRowNumber(table1);
            int maxRowNumTable2 = GetMaxRowNumber(table2);
            //добавление строк из первой таблицы
            for (int rowNum = 2; rowNum <= maxRowNumTable1; rowNum++)
            {
                currId = "A";
                var allCells = GetRowCells(table1, rowNum);
                foreach (var header in allHeaders)
                {
                    string cellValue = "";
                    string columnId = table1IdHeader.ContainsKey(header) ? table1IdHeader[header] : null;

                    if (columnId != null && allCells.ContainsKey(columnId))
                    {
                        cellValue = allCells[columnId]?.ToString() ?? "";
                    }
                    newTable.Add(new List<object> { currId, rowNum, cellValue });
                    currId = IdCont(currId);
                }
            }
            //добавление строк из второй таблицы
            int nextRowNum = maxRowNumTable1 + 1;
            for (int rowNum = 2; rowNum <= maxRowNumTable2; rowNum++)
            {
                currId = "A";
                var allCells = GetRowCells(table2, rowNum);
                foreach (var header in allHeaders)
                {
                    string cellValue = "";
                    string columnId = table2IdHeader.ContainsKey(header) ? table2IdHeader[header] : null;
                    if (columnId != null && allCells.ContainsKey(columnId))
                    {
                        cellValue = allCells[columnId]?.ToString() ?? "";
                    }

                    newTable.Add(new List<object> { currId, nextRowNum, cellValue });
                    currId = IdCont(currId);
                }
                nextRowNum++;
            }
            return newTable;
        }
        //функция для построения словарей заголовков
        private static Dictionary<string, string> BuildHeaderIdDictionary(List<List<object>> table)
        {
            var dict = new Dictionary<string, string>();
            foreach (var cell in table)
            {
                if (cell.Count > 1 && cell[1] is int && (int)cell[1] == 1)
                {
                    string columnId = cell[0]?.ToString();

                    for (int i = 2; i < cell.Count; i++)
                    {
                        string headerName = cell[i]?.ToString();
                        if (!string.IsNullOrEmpty(headerName))
                        {
                            dict[headerName] = columnId;
                            columnId = IdCont(columnId);
                        }
                    }
                }
            }
            return dict;
        }
        //функция для выделения максимального номера строки
        private static int GetMaxRowNumber(List<List<object>> table)
        {
            int max = 0;
            foreach (var cell in table)
            {
                if (cell.Count > 1 && cell[1] is int)
                {
                    int rowNum = (int)cell[1];
                    if (rowNum > max)
                    {
                        max = rowNum;
                    }
                }
            }
            return max;
        }
        //функция для сбора всех ячеек строки в словарь
        private static Dictionary<string, object> GetRowCells(List<List<object>> table, int rowNum)
        {
            var cells = new Dictionary<string, object>();

            foreach (var cell in table)
            {
                if (cell.Count > 1 && cell[1] is int && (int)cell[1] == rowNum) //нахождение всех ячеек определённой строки
                {
                    string columnId = cell[0]?.ToString();
                    object value = cell.Count > 2 ? cell[2] : "";
                    cells[columnId] = value;
                }
            }
            return cells;
        }

        //функция для продолжения нумерации ячеек 
        private static string IdCont(string cellId)
        {
            //если ячейка пуста — начинаем с первой ячейки A
            if (string.IsNullOrEmpty(cellId))
            {
                return "A";
            }
            /*для определения правильного двузначного и больше идентификатора будет строиться 
            следующая логика: текущий буквенный идентификатор сначала превратится в число, 
            потом это число увеличится и вернётся новый правильный буквенный идентификатор*/
            //превращение буквенного id в число
            int NumberFromId(string id)
            {
                int number = 0;
                foreach (char letter in id.ToUpper())
                {
                    number = number * 26 + (letter - 'A' + 1); //вычисление номера буквы по латинскому алфавиту
                }
                return number;
            }
            //превращение числового id обратно в буквенный
            string IdFromNumber(int number)
            {
                string id = "";
                while (number > 0)
                {
                    int remainder = (number - 1) % 26;
                    number = (number - 1) / 26;
                    id = (char)('A' + remainder) + id;
                }
                return id;
            }
            int currentNumber = NumberFromId(cellId);
            int nextNumber = currentNumber + 1; //номер следующей ячейки от заданной
            return IdFromNumber(nextNumber); //id этой следующей
        }

        //функция для обработки исключений
        private static List<List<object>> ExceptionProcess(List<List<object>> table1, List<List<object>> table2)
        {
            if (table1 == null && table2 == null)
            {
                throw new ArgumentException("Both tables are None");
            }
            if (table1 != null && table2 == null)
            {
                Console.WriteLine("Table #2 is None. Return table #1");
                return table1;
            }
            if (table1 == null && table2 != null)
            {
                Console.WriteLine("Table #1 is None. Return table #2");
                return table2;
            }
            return null; //нет исключительной ситуации
        }
    }
}
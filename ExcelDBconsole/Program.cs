using ExcelBDConsol;
using System;
using System.Collections.Generic;

namespace ExcelDBconsole
{
    internal class Program
    {

        private static List<List<object>> table1 = null;
        private static List<List<object>> table2 = null;
        private static List<List<object>> mergedTable = null;
        private static string table1Path = null;
        private static string table2Path = null;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Excel BD Converter (Консольная версия) ===");
            Console.WriteLine("Команды:");
            Console.WriteLine("  SetFILE1 <путь>  - загрузить первую таблицу");
            Console.WriteLine("  SetFILE2 <путь>  - загрузить вторую таблицу");
            Console.WriteLine("  MERGE!           - объединить загруженные таблицы");
            Console.WriteLine("  SAVE! <путь>     - сохранить результат");
            Console.WriteLine("=============================================\n");

            // Бесконечный цикл обработки команд
            while (true)
            {
                try
                {
                    //Console.Write("> ");
                    string input = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    ProcessCommand(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }


        /// Обработка введенной команды

        private static void ProcessCommand(string command)
        {
            string[] parts = command.Split('$', 2, StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToUpper();
            string arg = parts.Length > 1 ? parts[1].Trim('"') : null;

            switch (cmd)
            {
                case "SETFILE1":
                    LoadTable1(arg);
                    break;

                case "SETFILE2":
                    LoadTable2(arg);
                    break;

                case "MERGE!":
                    MergeTables();
                    break;

                case "SAVE!":
                    SaveTable(arg);
                    break;

                default:
                    Console.WriteLine($"Неизвестная команда: {cmd}");
                    throw new Exception("FUCK YOU");
                    break;
            }
        }


        /// Загрузка первой таблицы

        private static void LoadTable1(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Ошибка: укажите путь к файлу");
                return;
            }

            try
            {
                table1 = ExcelFileWorks.GetDataFromExcelFile(filePath);
                table1Path = filePath;
                Console.WriteLine($"✓ Таблица 1 загружена: {System.IO.Path.GetFileName(filePath)}");
                Console.WriteLine($"  Строк: {GetRowCount(table1)}, Колонок: {GetColumnCount(table1)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки файла 1: {ex.Message}");
                table1 = null;
                table1Path = null;
            }
        }


        /// Загрузка второй таблицы

        private static void LoadTable2(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Ошибка: укажите путь к файлу");
                return;
            }

            try
            {
                table2 = ExcelFileWorks.GetDataFromExcelFile(filePath);
                table2Path = filePath;
                Console.WriteLine($"✓ Таблица 2 загружена: {System.IO.Path.GetFileName(filePath)}");
                Console.WriteLine($"  Строк: {GetRowCount(table2)}, Колонок: {GetColumnCount(table2)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки файла 2: {ex.Message}");
                table2 = null;
                table2Path = null;
            }
        }

        /// Объединение таблиц

        private static void MergeTables()
        {
            if (table1 == null)
            {
                Console.WriteLine("Ошибка: таблица 1 не загружена. Используйте SetFILE1");
                return;
            }

            if (table2 == null)
            {
                Console.WriteLine("Ошибка: таблица 2 не загружена. Используйте SetFILE2");
                return;
            }

            try
            {
                Console.WriteLine("Объединение таблиц...");
                mergedTable = TableMerge.MergeTables(table1, table2);

                Console.WriteLine("MERGE!$Merge ended");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка объединения: {ex.Message}");
                mergedTable = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                mergedTable = null;
            }
        }


        /// Сохранение результата

        private static void SaveTable(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Ошибка: укажите путь для сохранения");
                return;
            }

            if (mergedTable == null)
            {
                Console.WriteLine("Ошибка: нет объединенной таблицы. Выполните MERGE! сначала");
                return;
            }

            try
            {
                // Используем имя файла без расширения как название листа
                string sheetName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrEmpty(sheetName))
                    sheetName = "Sheet1";

                ExcelFileWorks.SaveDataToExcel(filePath, mergedTable, sheetName);
                Console.WriteLine($"SAVE!$Merge ended");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }


        /// Получить количество строк в таблице

        private static int GetRowCount(List<List<object>> table)
        {
            if (table == null || table.Count == 0)
                return 0;

            int maxRow = 0;
            foreach (var cell in table)
            {
                if (cell.Count > 1 && cell[1] is int rowNum)
                {
                    maxRow = Math.Max(maxRow, rowNum);
                }
            }
            return maxRow;
        }


        /// Получить количество колонок в таблице

        private static int GetColumnCount(List<List<object>> table)
        {
            if (table == null || table.Count == 0)
                return 0;

            int maxCol = 0;
            foreach (var cell in table)
            {
                if (cell.Count > 1 && cell[1] is int && (int)cell[1] == 1)
                {
                    // Это шапка, считаем колонки
                    maxCol = Math.Max(maxCol, cell.Count - 2);
                }
            }
            return maxCol;
        }
    }
}
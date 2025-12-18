using ExcelDBconsole;
using System;
using System.Collections.Generic;

namespace ExcelBDConsol
{
	public static class Tests
	{
		public static void RunTest()
		{
			var table1 = new List<List<object>>
			{
				new List<object> { "A", 1, "Name" },
				new List<object> { "B", 1, "Age" },
				new List<object> { "C", 1, "Work" },
				new List<object> { "A", 2, "Andrey" },
				new List<object> { "B", 2, 19 },
				new List<object> { "C", 2, "manager" },
				new List<object> { "A", 3, "Petya" },
				new List<object> { "B", 3, 18 },
				new List<object> { "C", 3, "worker" },
				new List<object> { "A", 4, "Sasha" },
				new List<object> { "B", 4, 19 },
				new List<object> { "C", 4, "director" }
			};
			var table2 = new List<List<object>>
			{
				new List<object> { "A", 1, "Name" },
				new List<object> { "B", 1, "Color" },
				new List<object> { "C", 1, "Age" },
				new List<object> { "A", 2, "Gosha" },
				new List<object> { "B", 2, "Red" },
				new List<object> { "C", 2, 9 },
				new List<object> { "A", 3, "Vova" },
				new List<object> { "B", 3, "Black" },
				new List<object> { "C", 3, 50 }
			};
			Console.WriteLine("Таблица 1:");
			PrintTable(table1);
			Console.WriteLine("\nТаблица 2:");
			PrintTable(table2);
			try
			{
				var newTable = TableMerge.MergeTables(table1, table2);
				if (newTable != null && newTable.Count > 0)
				{
					Console.WriteLine("\nОбъединенная таблица:");
					PrintTable(newTable);
				}
				else
				{
					Console.WriteLine("Таблицы не могут быть объединены");
				}
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
			}

			List<List<object>> lists1 = ExcelFileWorks.GetDataFromExcelFile("Sample1.xlsx");
			List<List<object>> lists2 = ExcelFileWorks.GetDataFromExcelFile("Sample2.xlsx");
			PrintTable(lists1);
			PrintTable(lists2);
			List<List<object>> output_lust = TableMerge.MergeTables(lists1, lists2);
			ExcelFileWorks.SaveDataToExcel("sampleoutput.xlsx", output_lust, "книга 1");
		}
		private static void PrintTable(List<List<object>> table)
		{
			if (table == null)
			{
				Console.WriteLine("Таблица пуста");
				return;
			}
			foreach (var row in table)
			{
				Console.Write("  [");
				for (int i = 0; i < row.Count; i++)
				{
					var value = row[i];

					if (value is string)
						Console.Write($"\"{value}\"");
					else
						Console.Write(value);
					if (i < row.Count - 1)
						Console.Write(", ");
				}
				Console.WriteLine("]");
			}
		}
		
	}
}
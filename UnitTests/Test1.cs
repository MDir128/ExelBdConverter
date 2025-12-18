using ExcelBDConsol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
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
            var NewTable = new List<List<object>>
            {
                new List<object> { "A", 1, "Name" },
                new List<object> { "B", 1, "Age" },
                new List<object> { "C", 1, "Work" },
                new List<object> { "D", 1, "Color" },
                new List<object> { "A", 2, "Andrey" },
                new List<object> { "B", 2, 19 },
                new List<object> { "C", 2, "manager" },
                new List<object> { "D", 2, "" },
                new List<object> { "A", 3, "Petya" },
                new List<object> { "B", 3, 18 },
                new List<object> { "C", 3, "Worker" },
                new List<object> { "D", 3, "" },
                new List<object> { "A", 4, "Sasha" },
                new List<object> { "B", 4, 19 },
                new List<object> { "C", 4, "director" },
                new List<object> { "D", 4, "" },
                new List<object> { "A", 5, "Gosha" },
                new List<object> { "B", 5, 9 },
                new List<object> { "C", 5, "" },
                new List<object> { "D", 5, "Red" },
                new List<object> { "A", 6, "Vova" },
                new List<object> { "B", 6, 50 },
                new List<object> { "C", 6, "" },
                new List<object> { "D", 6, "Black" },
            };
            Assert.AreEqual(NewTable, TableMerge.MergeTables(table1, table2));
        }
        
    }
}

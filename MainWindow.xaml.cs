using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;

namespace ExelBdConverter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public void OpenTableClick(Object sender, RoutedEventArgs args)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Таблицы (*.xls;*.xlsx)|*.xls;*.xlsx";
        bool? gotcha = openFileDialog.ShowDialog();
        string path = openFileDialog.FileName;
        if (gotcha == true)
        {
            try
            {
                OpenTableProcess(path);
            }
            catch { }
        }
    }
    public void OpenTableProcess(string table_path)
    {
        Process excelpro = new Process();
        excelpro.EnableRaisingEvents = true;
        excelpro.StartInfo = new ProcessStartInfo()
        {
            FileName = "excel.exe",
            Arguments = $"\"{table_path}\"",
            UseShellExecute = true
        };
        excelpro.Start();
        // процесс настроен так, что необходимо при работе с ним в первую
        // очередь запускать метод ThrowStartDataInProcc(),
        // в котором он запомнит адресс файла с которым работает
        ProccPy tableview = new ProccPy("pytableplot.py");
        tableview.ThrowStartDataInProcc(table_path);
        testProcess(tableview);
        excelpro.Exited += (s, e) => {
            tableview.Close();
            MessageBox.Show("Вышел");
        };
    }
    private void testProcess(ProccPy procc)
    {
        string response = procc.ChecProcc();
        Console.WriteLine(response);
    }

    class ProccPy
    {
        private StreamWriter stdin;
        private StreamReader stdout;
        Process pysubproc;
        public void Close()
        {
            stdin.Close();
            stdout.Close();
        }
        public ProccPy(string path)
        {
            pysubproc = new Process();
            pysubproc.StartInfo = new ProcessStartInfo()
            {
                FileName = "python.exe",
                Arguments = $"PythonSubProg\"{path}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            pysubproc.Start();
            stdin = pysubproc.StandardInput;
            stdout = pysubproc.StandardOutput;

        }
        public void ThrowStartDataInProcc(string data)
        {
            stdin.WriteLine(data);
            stdin.Flush();
        }
        public string ChecProcc()
        {
            stdin.WriteLine("CHECK!");
            stdin.Flush();
            string? output = stdout.ReadLine();
            if (output != null)
            {
                return output;
            }
            else
            {
                return "Oh, no!";
            }
        }
    }
}

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ExelBdConverter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ProccPy tableview;
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
                Console.WriteLine("start table");
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
        tableview = new ProccPy(@"PythonSubProg\pytableplot.py");
        tableview.ThrowStartDataInProcc("FILE=" + table_path);
        MessageBox.Show(testProcess(tableview));
        excelpro.Exited += (s, e) =>
        {
            tableview.Close();
            MessageBox.Show("Вышел");
        };
    }
    private string testProcess(ProccPy procc)
    {
        string response = procc.ChecProcc();
        return response;

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
                FileName = @"python-3.13.7-embed-amd64\python.exe",
                Arguments = $"\"{path}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
            };
            pysubproc.Start();
            pysubproc.BeginErrorReadLine();
            Debug.WriteLine("processstarted");
            stdin = pysubproc.StandardInput;
            stdout = pysubproc.StandardOutput;
            pysubproc.ErrorDataReceived += (s, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    Debug.WriteLine($"Python ERR: {e.Data}");
            };
            pysubproc.Exited += (s, e) =>
            {
                Debug.WriteLine("Nah, I'm done here, good luck");
            };
            
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
            
            try
            {
                string output = stdout.ReadLine();
                Debug.WriteLine("output" + output);
                return output;
            }
            catch
            {
                return "Oh, no!";
            }
        }
    }
}

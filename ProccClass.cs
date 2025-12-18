using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ExcelBdConverter
{
    // Процесс потока Питона - при создании Запускает в качестве нового процесса файл питона, казанный в конструкторе
    // для взаимодействия с процессом использовать функцию ThrowaCommandDataResp -
    // в качестве аргуемнта - команда с данными,
    // а в качестве возвращаемого значения - вывод процесса

    // процесс настроен так, что необходимо при работе с ним в первую
    // очередь запускать метод ThrowStartDataInProcc(),
    // в котором он запомнит адресс файла с которым работает
    class ProccPy
    {
        public EventHandler<string> GotAnswer;

        private StreamWriter stdin;
        private StreamReader stdout;
        Process pysubproc;

        // инициализация процесса
        public ProccPy(string path, string start_data)
        {
            //конфигурация процесса
            pysubproc = new Process();
            ProcessStartInfo StartInfo = new ProcessStartInfo()
            {
                //FileName = @"python-3.13.7-embed-amd64\python.exe",
                //Arguments = $"\"{path}\"",
                FileName = path,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = ""
                //WorkingDirectory = @"PythonSubProg"
            };

            pysubproc.StartInfo = StartInfo;
            pysubproc.Start(); //запуск процесса
            pysubproc.BeginErrorReadLine(); //покдключение потока ошибок
            Debug.WriteLine("processstarted");
            stdin = pysubproc.StandardInput; //переопределение потока ввода
            stdout = pysubproc.StandardOutput; // переопределение потока вывода
            Task.Run(async () =>
            {
                while (!pysubproc.HasExited)
                {
                    var line = await stdout.ReadLineAsync();
                    if (line != null)
                        GotAnswer?.Invoke(this, line);
                }
            });
            pysubproc.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    Debug.WriteLine($"Python ERR: {e.Data}");
            }; // eventhandler для вывода ошибок
            pysubproc.Exited += (s, e) =>
            {
                Debug.WriteLine("Nah, I'm done here, good luck");
            }; //сообщение о закрытии процесса питона
            this.ThrowStartDataInProcc(start_data);
        }
        //Деструктор
        ~ProccPy()
        {
            this.Close();
        }
        // метод закрытия потоков ввода-вывода
        public void Close()
        {
            if (stdin != null)
            {
                stdin.Close();
            }
            if (stdout != null)
            {
                stdout.Close();
            }
            if (pysubproc != null)
            {
                pysubproc.Close();
            }
        }
        // часть класса для обмена данных с процессом

        // этот метод не ожидает данных в ответ
        public void ThrowStartDataInProcc(string data)
        {
            stdin.WriteLine(data);
            stdin.Flush();
        }

        //этот метод подразумевает, что данные в ответ будут
        //Подразумевается, что ответ отловят с другой стороны через EventHandler GotAnswer, где в качестве аргумента он и получит ответ потока
        public Task ThrowaCommandDataResp(string command, string data)
        {
            stdin.WriteLine(data != null ? $"{command}:{data}" : command);
            stdin.Flush();
            return Task.CompletedTask;
        }
        public void ChecProcc()
        {
            ThrowaCommandDataResp("Debug$CHECK!", null);
        }
        private async Task GetAnswer()
        {
            //try
            //{
                //while (true)
                //{
                    string? line = await stdout.ReadLineAsync();
                    //if (line != null)
                    //{
                    //    break;
                    //}
                    GotAnswer.Invoke(this, line);
                //}
            //}
            //catch { GotAnswer.Invoke(this, "failed"); }
        }
    }
}

﻿using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace ExelBdConverter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<KeyValuePair<string, string>> fileHistory = new List<KeyValuePair<string, string>>(); //Словарь для хранения истории. Ключ - имя файла, значение - путь
    private const int MaxHistorySize = 10; // Ограничение для списка открытых файлов
    ProccPy tableview; // Объект процесса питон
    public MainWindow()
    {
        InitializeComponent();
    }

    //Buttons

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
                AddFileToHistory(path); // Добавляем файл в историю

                Console.WriteLine("start table");
                OpenTableProcess(path);
            }
            catch { }
        }
    }

    // DeepFunctions

    // Для получения информации из потока используется подобная функция

    // Описание самой функции, которая всякий раз будет активироваться когда процесс выдает значение

    //private void testProcess(ProccPy procc)
    //{
    //    procc.ThrowaCommandDataResp("Commad/Flag$"+args/data) // на 
    //    EventHandler<string> eventhandlex = null;                 // задание пустой лямбда-функции
    //    eventhandlex = (s, e) => {                                // для отлавливания значения, рекомендуется использовать лямбда функцию такого вида
    //        response = e;                                         // В качестве значения работы процесса исользуется в данном случае аргумент e
    //        string[] gotresp = response.Split('$');
    //        if (gotresp.Length == 2)
    //        {
    //            if (gotresp[0] == "Debug")                        // Функция всегда возвращает флаг, который был подан в начале аргумента команды с разделителем '$'
    //            {
    //                Debug.WriteLine(response);
    //                procc.GotAnswer -= eventhandlex;              // В обязательном порядке, требуется добавить в конец лямбда функции, её отписку от события
    //            }
    //        }
    //    };
    //    Debug.WriteLine("StartedTestResp");
    //    procc.GotAnswer += eventhandlex;
    //}

    // Функция для добавления файла в историю
    private void AddFileToHistory(string filePath)
    {
        // Получение названия файла
        string fileName = Path.GetFileName(filePath);

        // Удаляем дубликаты
        fileHistory.RemoveAll(x => x.Key == fileName);

        // Добавление в начало списка
        fileHistory.Insert(0, new KeyValuePair<string, string>(fileName, filePath));

        // Ограничиваем размер истории
        if (fileHistory.Count > MaxHistorySize)
        {
            fileHistory.RemoveAt(MaxHistorySize);
        }

        //Обновление ListBox
        UpdateHistoryListBox();
    }
    
    

    // Функция для обновления ListBox
    private void UpdateHistoryListBox()
    {
        // Необходимо очистить ListBox
        HistoryListBox.Items.Clear();

        // Добавить все файлы из истории
        foreach (var item in fileHistory)  // Перебор KeyValuePair
        {
            HistoryListBox.Items.Add(item.Key);  // Берем только ключ (имя файла)
        }
    }

    // Функция для кнопки "Очистить историю"
    private void ClearHistoryClick(object sender, RoutedEventArgs e)
    {
        fileHistory.Clear(); // Очищаем список
        HistoryListBox.Items.Clear(); // Очищаем ListBox
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
        tableview = new ProccPy(@"pytableplot.py", "SetFILE$" + table_path);
        testProcess(tableview);
        excelpro.Exited += (s, e) =>
        {
            tableview.Close();
            Debug.WriteLine("closed");
        };
    }

    private void HistoryListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (HistoryListBox.SelectedIndex >= 0)
        {
            var selectedItem = fileHistory[HistoryListBox.SelectedIndex];
            OpenTableProcess(selectedItem.Value);  // Используем Value (полный путь)
        }
    }

    private void testProcess(ProccPy procc)
    {
        procc.ChecProcc();
        EventHandler<string> eventhandlex = null;
        eventhandlex = (s, e) => {
            string response = e;
            if (response != null)
            {
                string[] gotresp = response.Split('$');
                if (gotresp.Length == 2)
                {
                    if (gotresp[0] == "Debug")
                    {
                        Debug.WriteLine(gotresp[1]);
                        procc.GotAnswer -= eventhandlex;
                    }
                }
            }
        };
        Debug.WriteLine("StartedTestResp");
        procc.GotAnswer += eventhandlex;
    }    
}

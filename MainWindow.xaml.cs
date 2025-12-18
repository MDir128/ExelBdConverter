using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace ExcelBdConverter;

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

    public void MergeTablesClick(Object sender, RoutedEventArgs args)
    {
        // Шаг 1: Выбираем две таблицы для объединения
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Таблицы (*.xls;*.xlsx)|*.xls;*.xlsx";
        openFileDialog.Multiselect = true;
        openFileDialog.Title = "Выберите две таблицы для объединения";

        bool? gotcha = openFileDialog.ShowDialog();

        if (gotcha == true && openFileDialog.FileNames.Length == 2)
        {
            string[] selectedFiles = openFileDialog.FileNames;
            string firstTablePath = selectedFiles[0];
            string secondTablePath = selectedFiles[1];

            try
            {
                // Шаг 2: Выбираем куда сохранить результат
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить объединенную таблицу";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.FileName = "Объединенная_таблица.xlsx"; // имя по умолчанию

                if (saveFileDialog.ShowDialog() == true)
                {
                    string savePath = saveFileDialog.FileName;

                    // Добавляем файлы в историю
                    AddFileToHistory(firstTablePath);
                    AddFileToHistory(secondTablePath);

                    // Шаг 3: Объединяем и сохраняем
                    MergeAndSaveTables(firstTablePath, secondTablePath, savePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при объединении таблиц: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else if (gotcha == true && openFileDialog.FileNames.Length != 2)
        {
            MessageBox.Show("НАДО ВЫБИРАТЬ ДВА ФАЙЛА!!11!!11",
                           "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    public async void MergeAndSaveTables(string firstTablePath, string secondTablePath, string savePath)
    {
        try
        {
            // Создаем процесс для работы с таблицами
            tableview = new ProccPy(@"ExcelDBconsole\ExcelDBconsole.exe", "SetFILE$" + firstTablePath);

            // Ждем немного инициализации
            await Task.Delay(100);

            // Шаг 1: Объединяем таблицы
            bool mergeSuccess = await SendMergeCommand(secondTablePath);

            if (mergeSuccess)
            {
                // Шаг 2: Сохраняем результат
                bool saveSuccess = await SendSaveCommand(savePath);

                if (saveSuccess)
                {
                    MessageBox.Show($"Таблица успешно сохранена в:\n{savePath}", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Добавляем результат в историю
                    AddFileToHistory(savePath);
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении файла", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ошибка при объединении таблиц", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Закрываем процесс
            tableview.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                          MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task<bool> SendMergeCommand(string secondTablePath)
    {
        var tcs = new TaskCompletionSource<bool>();

        EventHandler<string> handler = null;
        handler = (s, e) => {
            if (!string.IsNullOrEmpty(e))
            {
                string[] gotresp = e.Split('$');
                if (gotresp.Length == 2)
                {
                    if (gotresp[0] == "MERGE!")
                    {
                        tableview.GotAnswer -= handler;  // Отписываемся сразу

                        if (gotresp[1].Contains("Merge ended"))
                        {
                            Debug.WriteLine("Объединение успешно");
                            tcs.SetResult(true);
                        }
                        else if (gotresp[1].Contains("merge is not allowed"))
                        {
                            Debug.WriteLine($"Ошибка объединения: {gotresp[1]}");
                            tcs.SetResult(false);
                        }
                    }
                }
            }
        };

        tableview.GotAnswer += handler;

        // Отправляем команду
        await tableview.ThrowaCommandDataResp($"MERGE!${secondTablePath}", null);

        // Ждем ответа с таймаутом
        return await tcs.Task.WaitAsync(TimeSpan.FromSeconds(30));
    }

    private async Task<bool> SendSaveCommand(string savePath)
    {
        var tcs = new TaskCompletionSource<bool>();

        EventHandler<string> handler = null;
        handler = (s, e) => {
            if (!string.IsNullOrEmpty(e))
            {
                string[] gotresp = e.Split('$');
                if (gotresp.Length == 2)
                {
                    if (gotresp[0] == "SAVE!")
                    {
                        tableview.GotAnswer -= handler;  // Отписываемся сразу

                        Debug.WriteLine($"Ответ на сохранение: {gotresp[1]}");
                        tcs.SetResult(true);
                    }
                }
            }
        };

        tableview.GotAnswer += handler;

        // Отправляем команду
        await tableview.ThrowaCommandDataResp($"SAVE!${savePath}", null);

        // Ждем ответа с таймаутом
        return await tcs.Task.WaitAsync(TimeSpan.FromSeconds(30));
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
        tableview = new ProccPy(@"ExcelDBconsole\ExcelDBconsole.exe", "SetFILE$" + table_path);
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
                    else
                    {
                        Debug.WriteLine("Not mine: " + response);
                    }
                }
                else
                {
                    Debug.WriteLine("unexpected resp: " + response);
                }
            }
        };
        Debug.WriteLine("StartedTestResp");
        procc.GotAnswer += eventhandlex;
    }    
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab02.Parsing;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;

namespace Lab02
{
    public partial class MainPage : ContentPage
    {
        private readonly DormDataService _dataService;
        private string _currentFilePath;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new DormDataService();

            TypePicker.SelectedIndex = 0;
        }

        private async void LoadButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                UpdateStrategy();

                var xmlFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".xml" } },
                    });

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Оберіть XML файл студентів",
                    FileTypes = xmlFileType
                });

                if (result != null)
                {
                    _currentFilePath = result.FullPath;
                    var students = _dataService.LoadData(_currentFilePath);
                    DisplayResults(students);
                    await DisplayAlertAsync("Успіх", $"Завантажено {students.Count} студентів.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", ex.Message, "OK");
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                await DisplayAlert("Помилка", "Спочатку завантажте файл XML", "OK");
                return;
            }

            try
            {
                string xslPath = Path.ChangeExtension(_currentFilePath, ".xsl");

                if (!File.Exists(xslPath))
                {
                    await DisplayAlert("Помилка", $"Не знайдено файл XSL!\nОчікувався тут:\n{xslPath}", "OK");
                    return;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string defaultFileName = Path.GetFileNameWithoutExtension(_currentFilePath) + ".html";
                string defaultSavePath = Path.Combine(desktopPath, defaultFileName);

                string outputPath = await DisplayPromptAsync(
                    title: "Збереження файлу",
                    message: "Введіть повний шлях та назву файлу:",
                    initialValue: defaultSavePath,
                    accept: "Зберегти",
                    cancel: "Скасувати",
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(outputPath))
                    return;

                _dataService.SaveToHtml(_currentFilePath, xslPath, outputPath);

                ResultLabel.Text = $"Файл збережено: {outputPath}";
                await DisplayAlert("Успіх", $"HTML файл успішно створено за адресою:\n{outputPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка при експорті", ex.Message, "OK");
            }
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            _dataService.ClearData();

            ResultLabel.Text = "Результати очищено.";
            Last_Name.Text = string.Empty;
            First_Name.Text = string.Empty;
            Patronymic.Text = string.Empty;
            Facuty.Text = string.Empty;
            Course.Text = string.Empty;
            City.Text = string.Empty;
            HasScholarship.IsChecked = false;
            _currentFilePath = null;
        }

        private void ApplyFilters()
        {
            try
            {
                var filtered = _dataService.Filter(
                    Last_Name.Text,
                    Facuty.Text,
                    Course.Text,
                    City.Text,
                    HasScholarship.IsChecked
                );
                DisplayResults(filtered);
            }
            catch
            {
            }
        }

        private void UpdateStrategy()
        {
            IParsingStrategy strategy = null;

            switch (TypePicker.SelectedIndex)
            {
                case 1:
                    strategy = new SAXParsingStrategy();
                    break;
                case 2:
                    strategy = new DOMParsingStrategy();
                    break;
                case 3:
                    strategy = new LINQParsingStrategy();
                    break;
                default:
                    throw new Exception("Будь ласка, оберіть метод аналізу (SAX, DOM або LINQ).");
            }
            _dataService.SetStrategy(strategy);
        }

        private void DisplayResults(List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                ResultLabel.Text = "Студентів не знайдено.";
                return;
            }

            ResultLabel.Text = string.Join("\n\n", students.Select(s => s.ToString()));
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentFilePath))
                ApplyFilters();
        }

        protected override bool OnBackButtonPressed()
        {
            Dispatcher.Dispatch(async () =>
            {
                bool exit = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
                if (exit)
                {
                    Application.Current.Quit();
                }
            });
            return true; 
        }
    }
}


using Lab02.Parsing;

namespace Lab02
{
    public partial class MainPage : ContentPage
    {
        private readonly DormDataService _dataService;
        private string? _currentXmlPath;
        private string? _currentXslPath;

        public MainPage()
        {
            InitializeComponent();
            _dataService = new DormDataService();

            TypePicker.SelectedIndex = 0;
            StatusPicker.SelectedIndex = 0;
        }

    private async Task<string?> PickFileAsync(string title, string extension)
        {
            try
            {
                var fileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { extension } },
                    });

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = title,
                    FileTypes = fileType
                });

                return result?.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", ex.Message, "OK");
                return null;
            }
        }

        private async void LoadButtonXML_Clicked(object sender, EventArgs e)
        {
            try
            {
                UpdateStrategy();

                var xmlPath = await PickFileAsync("Оберіть XML файл студентів", ".xml");
                if (!string.IsNullOrEmpty(xmlPath))
                {
                    _currentXmlPath = xmlPath;
                    var students = _dataService.LoadData(_currentXmlPath);
                    DisplayResults(students);
                    await DisplayAlertAsync("Успіх", $"Завантажено {students.Count} студентів.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", ex.Message, "OK");
            }

            
        }

        private async void LoadButtonXSL_Clicked(object sender, EventArgs e)
        {
            try
            {
                UpdateStrategy();
                var xslPath = await PickFileAsync("Оберіть XSL файл для трансформації", ".xsl");
                if (!string.IsNullOrEmpty(xslPath))
                {
                    _currentXslPath = xslPath;
                    await DisplayAlertAsync("Успіх", $"Файл XSL обрано: {xslPath}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка", ex.Message, "OK");
            }
            
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(_currentXmlPath))
            {
                await DisplayAlertAsync("Помилка", "Спочатку завантажте файл XML", "OK");
                return;
            }
            if (string.IsNullOrEmpty(_currentXslPath))
            {
                await DisplayAlertAsync("Помилка", "Спочатку завантажте файл XSL", "OK");
                return;
            }

            try
            {
                string outputPath = await DisplayPromptAsync(
                    title: "Збереження файлу",
                    message: "Введіть повний шлях та назву файлу:",
                    accept: "Зберегти",
                    cancel: "Скасувати",
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(outputPath)) return;

                HtmlSaver.Transform(_currentXmlPath, _currentXslPath, outputPath);

                ResultLabel.Text = $"Файл збережено: {outputPath}";
                await DisplayAlertAsync("Успіх", $"HTML файл успішно створено за адресою:\n{outputPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Помилка при експорті", ex.Message, "OK");
            }
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            _dataService.ClearData();

            ResultLabel.Text = "Результати очищено.";
            LastNameEntry.Text = string.Empty;
            FirstNameEntry.Text = string.Empty;
            PatronymicEntry.Text = string.Empty;
            FacultyEntry.Text = string.Empty;
            SpecialtyEntry.Text = string.Empty;
            ChairEntry.Text = string.Empty;
            CourseEntry.Text = string.Empty;
            AddressEntry.Text = string.Empty;
            RoomEntry.Text = string.Empty;
            HasScholarshipCheckBox.IsChecked = false;
            StatusPicker.SelectedIndex = 0;
            _currentXmlPath = null;
        }

        private void ApplyFilters()
        {
            try
            {
                
                Student filter = new Student
                {
                    LastName = LastNameEntry.Text,
                    FirstName = FirstNameEntry.Text,
                    Patronymic = PatronymicEntry.Text,
                    Faculty = FacultyEntry.Text,
                    Specialty = SpecialtyEntry.Text,
                    Chair = ChairEntry.Text,
                    Address = AddressEntry.Text,
                    Room = RoomEntry.Text,
                    Status = returnStatus(),
                    Course = int.TryParse(CourseEntry.Text, out int course) ? course : 0,
                    HasScholarship = HasScholarshipCheckBox.IsChecked
                };
                var filtered = _dataService.Filter(filter);
                DisplayResults(filtered);
            }
            catch  
            {
            }
        }

        private string returnStatus()
        {
            switch (StatusPicker.SelectedIndex)
            {
                case 1: return "Проживає";
                case 2: return "Очікує";
                case 3: return "Виселено";
                default: return string.Empty;
            }
        }
        private void UpdateStrategy()
        {
            IParsingStrategy? strategy = null;

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
            if (!string.IsNullOrEmpty(_currentXmlPath))
                ApplyFilters();
        }
    }
}


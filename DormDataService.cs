using Lab02.Parsing;

namespace Lab02
{
    internal class DormDataService
    {
        private IParsingStrategy? _parsingStrategy;
        private List<Student> _allStudents;

        public DormDataService()
        {
            _allStudents = new List<Student>();
        }

        public void SetStrategy(IParsingStrategy strategy)
        {
            _parsingStrategy = strategy;
        }

        public List<Student> LoadData(string filePath)
        {
            if (_parsingStrategy == null)
                throw new Exception("Стратегія аналізу не обрана.");

            _allStudents = _parsingStrategy.Parse(filePath);
            return _allStudents;
        }

        public List<Student> Filter(Student filter)
        {

            var query = _allStudents.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filter.LastName))
                query = query.Where(s => !string.IsNullOrEmpty(s.LastName)
                                         && s.LastName.Contains(filter.LastName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                query = query.Where(s => !string.IsNullOrEmpty(s.FirstName)
                                         && s.FirstName.Contains(filter.FirstName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Patronymic))
                query = query.Where(s => !string.IsNullOrEmpty(s.Patronymic)
                                         && s.Patronymic.Contains(filter.Patronymic, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Faculty))
                query = query.Where(s => !string.IsNullOrEmpty(s.Faculty)
                                         && s.Faculty.Contains(filter.Faculty, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Specialty))
                query = query.Where(s => !string.IsNullOrEmpty(s.Specialty)
                                         && s.Specialty.Contains(filter.Specialty, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Chair))
                query = query.Where(s => !string.IsNullOrEmpty(s.Chair)
                                         && s.Chair.Contains(filter.Chair, StringComparison.OrdinalIgnoreCase));

            query = query.Where(s => s.Course == filter.Course);

            if (!string.IsNullOrWhiteSpace(filter.Address))
                query = query.Where(s => !string.IsNullOrEmpty(s.Address)
                                         && s.Address.Contains(filter.Address, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Room))
                query = query.Where(s => !string.IsNullOrEmpty(s.Room)
                                         && s.Room.Contains(filter.Room, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(s => !string.IsNullOrEmpty(s.Status)
                                         && s.Status.Equals(filter.Status, StringComparison.OrdinalIgnoreCase));

            if (filter.FromDate != null)
                query = query.Where(s => s.FromDate != null && s.FromDate >= filter.FromDate);

            if (filter.ToDate != null)
                query = query.Where(s => s.ToDate != null && s.ToDate <= filter.ToDate);

            if (filter.HasScholarship)
                query = query.Where(s => s.HasScholarship);

            return query.ToList();
        }


        public void ClearData()
        {
            _allStudents.Clear();
        }

        public void SaveToHtml(string xmlPath, string xslPath, string outputPath)
        {
            HtmlSaver.Transform(xmlPath, xslPath, outputPath);
        }
    }
}


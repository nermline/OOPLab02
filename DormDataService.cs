using Lab02.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab02
{
    internal class DormDataService
    {
        private IParsingStrategy _parsingStrategy;
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
                throw new System.Exception("Стратегія аналізу не обрана.");

            _allStudents = _parsingStrategy.Parse(filePath);
            return _allStudents;
        }

        public List<Student> Filter(string lastName, string faculty, string course, string city, bool onlyScholarship)
        {
            var query = _allStudents.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(s => !string.IsNullOrEmpty(s.LastName) && s.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(faculty))
                query = query.Where(s => !string.IsNullOrEmpty(s.Faculty) && s.Faculty.Contains(faculty, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(course))
                query = query.Where(s => s.Course == course);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(s => !string.IsNullOrEmpty(s.City) && s.City.Contains(city, StringComparison.OrdinalIgnoreCase));

            if (onlyScholarship)
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


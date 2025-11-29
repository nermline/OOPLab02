using System.Xml.Linq;

namespace Lab02.Parsing
{
    internal class LINQParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            var doc = XDocument.Load(filePath);

            foreach (var elem in doc.Descendants("Student"))
            {
                var student = new Student();

                student.Status = elem.Attribute("status")?.Value ?? string.Empty;
                student.HasScholarship = elem.Attribute("scholarship")?.Value == "true";
                student.Room = elem.Attribute("room")?.Value;

                var nameElem = elem.Element("Name");
                student.LastName = nameElem?.Element("Last")?.Value ?? string.Empty;
                student.FirstName = nameElem?.Element("First")?.Value ?? string.Empty;
                student.Patronymic = nameElem?.Element("Patronymic")?.Value ?? string.Empty;

                var facultyElem = elem.Element("Faculty");
                student.Faculty = facultyElem?.Element("Name")?.Value ?? string.Empty;
                student.Specialty = facultyElem?.Element("Specialty")?.Value ?? string.Empty;

                var chairValue = elem.Element("Chair")?.Value;
                student.Chair = chairValue == string.Empty ? null : chairValue;

                student.Course = int.Parse(elem.Element("Course")?.Value ?? "0");
                var resElem = elem.Element("Residence");
                student.Address = resElem?.Element("Address")?.Value ?? string.Empty;

                var fromDateStr = resElem?.Element("FromDate")?.Value ?? string.Empty;
                student.FromDate = fromDateStr != string.Empty ? DateOnly.ParseExact(fromDateStr, "dd-MM-yyyy") : null;

                var toDateStr = resElem?.Element("ToDate")?.Value ?? string.Empty;
                student.ToDate = toDateStr != string.Empty ? DateOnly.ParseExact(toDateStr, "dd-MM-yyyy") : null;
                students.Add(student);
            }
            return students;
        }



    }
}
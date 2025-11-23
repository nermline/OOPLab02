using System.Collections.Generic;
using System.Xml.Linq;

namespace Lab02.Parsing
{
    internal class LINQParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var students = new List<Student>();

            foreach (var elem in doc.Descendants("Student"))
            {
                var student = new Student();

                var scholarshipAttr = elem.Attribute("scholarship");
                student.HasScholarship = scholarshipAttr != null && scholarshipAttr.Value == "true";

                var nameElem = elem.Element("Name");
                if (nameElem != null)
                {
                    student.LastName = nameElem.Element("Last")?.Value;
                    student.FirstName = nameElem.Element("First")?.Value;
                    student.Patronymic = nameElem.Element("Patronymic")?.Value;
                }

                var facultyElem = elem.Element("Faculty");
                if (facultyElem != null)
                {
                    student.Faculty = facultyElem.Element("Name")?.Value;
                    student.Department = facultyElem.Element("Department")?.Value;
                }

                student.Course = elem.Element("Course")?.Value;

                var resElem = elem.Element("Residence");
                student.City = resElem?.Element("Address")?.Value;

                students.Add(student);
            }

            return students;
        }

    }
}

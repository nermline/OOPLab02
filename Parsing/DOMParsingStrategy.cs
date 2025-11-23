using System.Collections.Generic;
using System.Xml;

namespace Lab02.Parsing
{
    internal class DOMParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            var doc = new XmlDocument();
            doc.Load(filePath);

            foreach (XmlNode node in doc.SelectNodes("//Student"))
            {
                var student = new Student();

                if (node.Attributes["scholarship"] != null)
                {
                    student.HasScholarship = node.Attributes["scholarship"].Value == "true";
                }

                student.LastName = node.SelectSingleNode("Name/Last")?.InnerText;
                student.FirstName = node.SelectSingleNode("Name/First")?.InnerText;
                student.Patronymic = node.SelectSingleNode("Name/Patronymic")?.InnerText;

                student.Faculty = node.SelectSingleNode("Faculty/Name")?.InnerText;
                student.Department = node.SelectSingleNode("Faculty/Department")?.InnerText;

                student.Course = node.SelectSingleNode("Course")?.InnerText;
                student.City = node.SelectSingleNode("Residence/Address")?.InnerText;

                students.Add(student);
            }

            return students;
        }
    }
}

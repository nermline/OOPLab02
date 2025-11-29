using System.Xml;
using System.Xml.Linq;

namespace Lab02.Parsing
{
    internal class DOMParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            var doc = new XmlDocument();
            doc.Load(filePath);
            var nodes = doc.SelectNodes("//Student");

            if (nodes is not null)
            {
                foreach (XmlNode node in nodes)
                {
                    var student = new Student();

                    var statusAttr = node.Attributes?["status"]?.Value;

                    switch (statusAttr)
                    {
                        case "resident": student.Status = "Проживає"; break;
                        case "ex-resident": student.Status = "Не проживає"; break;
                        case "expectant": student.Status = "Очікує"; break;
                        default: student.Status = "Невідомо"; break;
                    }

                    var scholarship = node.Attributes?["scholarship"]?.Value;
                    student.HasScholarship = (scholarship is not null && scholarship == "true");

                    student.Room = node.Attributes?["room"]?.Value;

                    student.LastName = node.SelectSingleNode("Name/Last")?.InnerText ?? string.Empty;
                    student.FirstName = node.SelectSingleNode("Name/First")?.InnerText ?? string.Empty;
                    student.Patronymic = node.SelectSingleNode("Name/Patronymic")?.InnerText ?? string.Empty;

                    student.Faculty = node.SelectSingleNode("Faculty/Name")?.InnerText ?? string.Empty;
                    student.Specialty = node.SelectSingleNode("Faculty/Specialty")?.InnerText ?? string.Empty;

                    string chairText = node.SelectSingleNode("Chair")?.InnerText ?? string.Empty;
                    student.Chair = chairText == string.Empty ? null : chairText;
                    student.Course = int.Parse(node.SelectSingleNode("Course")?.InnerText ?? "0");
                    student.Address = node.SelectSingleNode("Residence/Address")?.InnerText ?? string.Empty;

                    string dateFromStr = node.SelectSingleNode("Residence/FromDate")?.InnerText ?? string.Empty;
                    student.FromDate = dateFromStr != string.Empty ? DateOnly.ParseExact(dateFromStr, "dd-MM-yyyy") : null;
                 
                    string dateToStr = node.SelectSingleNode("Residence/ToDate")?.InnerText ?? string.Empty;
                    student.ToDate = dateToStr != string.Empty ? DateOnly.ParseExact(dateToStr, "dd-MM-yyyy") : null;

                    students.Add(student);
                }
            }
            return students;
        }




    }
}

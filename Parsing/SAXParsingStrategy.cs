using System.Xml;

namespace Lab02.Parsing
{
    internal class SAXParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            Student? currentStudent = null;
            string currentElement = string.Empty;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            currentElement = reader.Name;

                            if (currentElement == "Student")
                            {
                                currentStudent = new Student();
                                currentStudent.Status = reader.GetAttribute("status") ?? string.Empty;
                                currentStudent.Room = reader.GetAttribute("room");
                                var scholarshipAttr = reader.GetAttribute("scholarship");
                                currentStudent.HasScholarship = scholarshipAttr != null && scholarshipAttr.ToLower() == "true";
                            }
                            break;

                        case XmlNodeType.Text:
                            if (currentStudent != null)
                            {
                                switch (currentElement)
                                {
                                    case "Last": currentStudent.LastName = reader.Value; break;
                                    case "First": currentStudent.FirstName = reader.Value; break;
                                    case "Patronymic": currentStudent.Patronymic = reader.Value; break;
                                    case "Faculty": break;
                                    case "Name":
                                        if (reader.Depth == 4) currentStudent.Faculty = reader.Value;
                                        break;
                                    case "Specialty": currentStudent.Specialty = reader.Value; break;
                                    case "Chair": currentStudent.Chair = reader.Value; break;
                                    case "Course":
                                        if (int.TryParse(reader.Value, out int course))
                                            currentStudent.Course = course;
                                        break;
                                    case "Address": currentStudent.Address = reader.Value; break;
                                    case "FromDate":
                                        if (!string.IsNullOrEmpty(reader.Value))
                                            currentStudent.FromDate = DateOnly.ParseExact(reader.Value, "dd-MM-yyyy");
                                        break;
                                    case "ToDate":
                                        if (!string.IsNullOrEmpty(reader.Value))
                                            currentStudent.ToDate = DateOnly.ParseExact(reader.Value, "dd-MM-yyyy");
                                        break;
                                }
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "Student" && currentStudent != null)
                            {
                                students.Add(currentStudent);
                                currentStudent = null;
                            }
                            break;
                    }
                }
            }

            return students;
        }
    }
}


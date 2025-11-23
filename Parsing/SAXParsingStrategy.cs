using System.Collections.Generic;
using System.Xml;

namespace Lab02.Parsing
{
    internal class SAXParsingStrategy : IParsingStrategy
    {
        public List<Student> Parse(string filePath)
        {
            var students = new List<Student>();
            Student currentStudent = null;
            bool isInFaculty = false;

            using (var reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Student":
                                currentStudent = new Student();
                                string scholarship = reader.GetAttribute("scholarship");
                                currentStudent.HasScholarship = (scholarship == "true");
                                break;

                            case "Faculty":
                                isInFaculty = true;
                                break;

                            case "Last":
                                if (currentStudent != null)
                                    currentStudent.LastName = reader.ReadElementContentAsString();
                                break;
                            case "First":
                                if (currentStudent != null)
                                    currentStudent.FirstName = reader.ReadElementContentAsString();
                                break;
                            case "Patronymic":
                                if (currentStudent != null)
                                    currentStudent.Patronymic = reader.ReadElementContentAsString();
                                break;

                            case "Name":
                                if (isInFaculty && currentStudent != null)
                                {
                                    currentStudent.Faculty = reader.ReadElementContentAsString();
                                }
                                break;

                            case "Department":
                                if (currentStudent != null)
                                    currentStudent.Department = reader.ReadElementContentAsString();
                                break;

                            case "Course":
                                if (currentStudent != null)
                                    currentStudent.Course = reader.ReadElementContentAsString();
                                break;

                            case "Address":
                                if (currentStudent != null)
                                    currentStudent.City = reader.ReadElementContentAsString();
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "Student")
                        {
                            if (currentStudent != null)
                            {
                                students.Add(currentStudent);
                                currentStudent = null;
                            }
                        }
                        else if (reader.Name == "Faculty")
                        {
                            isInFaculty = false;
                        }
                    }
                }
            }
            return students;
        }   
    }
}

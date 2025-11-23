using System;
using System.Collections.Generic;
using System.Text;

namespace Lab02.Parsing
{
    public class Student
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool HasScholarship { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {Patronymic}\n" +
                   $"Ф-т: {Faculty} ({Department}), Курс: {Course}\n" +
                   $"Адреса: {City} | Стипендія: {(HasScholarship ? "Так" : "Ні")}";
        }
    }
}

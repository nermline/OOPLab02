namespace Lab02.Parsing
{
    public class Student
    {
        public string Status { get; set; } = string.Empty;
        public string? Room { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? Chair { get; set; }
        public int Course { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public bool HasScholarship { get; set; }

        public override string ToString()
        {
            string info = $"{LastName} {FirstName} {Patronymic}\n" + 
                          $"Ф-т: {Faculty} ({Specialty}), Курс: {Course}\n";

            if (Chair is not null)
            {
                info += $"Кафедра: {Chair}\n";
            }

            info += $"Статус: {Status}\n" + $"Адреса: {Address}";

            if (Room is not null)
            {
                info += $", Кімната: {Room}";
            }

            if (FromDate is not null && ToDate is not null)
            {
                info += $"\nПеріод проживання: {FromDate:dd.MM.yyyy} - {ToDate:dd.MM.yyyy}";
            }

            info += $"\nСтипендія: {(HasScholarship ? "Так" : "Ні")}";

            return info;
        }
    }
}

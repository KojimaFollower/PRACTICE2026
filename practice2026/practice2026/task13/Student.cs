namespace task13
{
    public class Subject
    {
        public string Name { get; set; }
        public int Grade { get; set; }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Subject> Grades { get; set; }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentException("Имя отсутствует!");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentException("Фамилия отсутствует!");
            foreach (char ch in FirstName)
            {
                if (char.IsPunctuation(ch))
                {
                    throw new ArgumentException("Имя содержит недопустимые символы");
                }
            }
            foreach (char ch in LastName)
            {
                if (char.IsPunctuation(ch))
                {
                    throw new ArgumentException("Фамилия содержит недопустимые символы");
                }
            }

            if (BirthDate.Year < 1900)
            {
                throw new ArgumentException("Некорректная дата рождения (ранее 1900 года)", nameof(BirthDate));
            }
            if (BirthDate.Year > 2011)
            {
                throw new ArgumentException("Некорректная дата рождения (позже 2011)", nameof(BirthDate));
            }
            if (Grades != null)
            {
                foreach (var subject in Grades)
                {
                    if (string.IsNullOrWhiteSpace(subject.Name))
                        throw new ArgumentException("Некорректное название предмета!");

                    if (subject.Grade > 100 || subject.Grade < 0)
                        throw new ArgumentException("Некорректная оценка!");
                }
            }
        }
    }
}
using task13;
namespace task13tests
{
    public class task13tests
    {
        private string testpath = "student_profile.json";
        [Fact]
        public void SerializeAndDeserialize_ValidStudent_ShouldMatchOriginal()
        {
            var student = new Student
            {
                FirstName = "Илон",
                LastName = "Маск",
                BirthDate = new DateTime(1971, 6, 28),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Физика", Grade = 100 },
                    new Subject { Name = "Экономика", Grade = 90 }
                }
            };
            string json = StudentSerializer.Serialize(student);
            Student deserializedStudent = StudentSerializer.Deserialize(json);
            Assert.NotNull(deserializedStudent);
            Assert.Equal(student.FirstName, deserializedStudent.FirstName);
            Assert.Equal(student.LastName, deserializedStudent.LastName);
            Assert.Equal(student.BirthDate, deserializedStudent.BirthDate);
            Assert.Equal(student.Grades.Count, deserializedStudent.Grades.Count);
            Assert.Equal(student.Grades[0].Name, deserializedStudent.Grades[0].Name);
            Assert.Equal(student.Grades[0].Grade, deserializedStudent.Grades[0].Grade);
        }
        [Fact]
        public void UploadAndDownload_ValidFile_ShouldSaveAndReadCorrectly()
        {
            var student = new Student
            {
                FirstName = "Генри",
                LastName = "Шпрот",
                BirthDate = new DateTime(1969, 12, 28)
            };
            StudentSerializer.Upload(testpath, student);
            Assert.True(File.Exists(testpath));
            Student downloadedStudent = StudentSerializer.Download(testpath);
            Assert.NotNull(downloadedStudent);
            Assert.Equal(student.FirstName, downloadedStudent.FirstName);
            Assert.Null(downloadedStudent.Grades); 

        }
        [Fact]
        public void Deserialize_FirstNameContainsInvalidCharacters_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Генри."",
            ""LastName"": ""Форд"",
            ""BirthDate"": ""1995-02-11""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Имя содержит недопустимые символы", exception.Message);
        }
        [Fact]
        public void Deserialize_LastNameContainsInvalidCharacters_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Генри"",
            ""LastName"": ""Форд!!!"",
            ""BirthDate"": ""1995-02-11""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Фамилия содержит недопустимые символы", exception.Message);
        }
        [Fact]
        public void Deserialize_EmptyFirstName_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": """",
            ""LastName"": ""Форд"",
            ""BirthDate"": ""1995-02-11""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Имя отсутствует!", exception.Message);
        }
        [Fact]
        public void Deserialize_EmptyLastName_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Генри"",
            ""LastName"": """",
            ""BirthDate"": ""1995-02-11""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Фамилия отсутствует!", exception.Message);
        }


        [Fact]
        public void Deserialize_InvalidGrade_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Базар"",
            ""LastName"": ""Джексон"",
            ""BirthDate"": ""2007-02-10"",
            ""Grades"": [ { ""Name"": ""Math"", ""Grade"": -5 } ]
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Некорректная оценка!", exception.Message);
        }
        [Fact]
        public void Deserialize_TheStudentUnder15YearsOld_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Энцо"",
            ""LastName"": ""Феррари"",
            ""BirthDate"": ""2012-01-10""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Некорректная дата рождения (позже 2011)", exception.Message);
        }
        [Fact]
        public void Deserialize_TheStudentIsTooOld_ThrowsArgumentException()
        {
            string invalidJson = @"
        {
            ""FirstName"": ""Энцо"",
            ""LastName"": ""Феррари"",
            ""BirthDate"": ""1899-01-10""
        }";

            var exception = Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(invalidJson));
            Assert.Contains("Некорректная дата рождения (ранее 1900 года)", exception.Message);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace task13
{
    public class StudentSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Converters = { new DateTimeConvert() },
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string Serialize(Student student)
        {
            return JsonSerializer.Serialize(student, Options);
        }

        public static Student Deserialize(string json)
        {
            var result = JsonSerializer.Deserialize<Student>(json, Options);

            result?.Validate();

            return result;
        }

        public static void Upload(string path, Student student)
        {
            string json = Serialize(student);
            File.WriteAllText(path, json);
        }

        public static Student Download(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл не найден");

            string json = File.ReadAllText(path);
            return Deserialize(json);
        }
    }
}

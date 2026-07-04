using System.Reflection;
using System.Text.Json.Serialization;

namespace task07
{
    public class AttributeReflection
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
        public class DisplayNameAttribute : Attribute
        {
            public string DisplayName { get; set; }
            public DisplayNameAttribute(string DisplayName)
            {
                this.DisplayName = DisplayName;
            }
        }
        [AttributeUsage(AttributeTargets.Class)]
        public class VersionAttribute : Attribute
        {
            public int Major { get; set; }
            public int Minor { get; set; }
            public string MajorMinor => $"{Major}.{Minor}";
            public VersionAttribute(int Major, int Minor)
            {
                this.Major = Major;
                this.Minor = Minor;
            }
        }
        [DisplayName("Пример класса")]
        [Version(1, 0)]
        public class SampleClass
        {
            [DisplayName("Числовое свойство")]
            public int Number { get; set; }
            [DisplayName("Тестовый метод")]
            public void TestMethod() { }

        }
        public static class ReflectionHelper
        {
            public static void PrintTypeInfo(Type type)
            {
                if (type == null)
                {
                    Console.WriteLine("Переданный тип пуст");
                    return;
                }
                var DisplayClass = type.GetCustomAttribute<DisplayNameAttribute>();
                if (DisplayClass != null)
                {
                    Console.WriteLine($"Отображаемое имя класса: {DisplayClass.DisplayName}");
                }
                else
                {
                    Console.WriteLine("Отображаемое имя класса не указана!");
                }
                var VersionClass = type.GetCustomAttribute<VersionAttribute>();
                if ( VersionClass != null )
                {
                    Console.WriteLine($"Отображаемое версия класса: {VersionClass.Major}.{VersionClass.Minor}");
                }
                else
                {
                    Console.WriteLine("Отображаемое версия класса не указана!");
                }
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => new { prop.Name, Attr = prop.GetCustomAttribute<DisplayNameAttribute>() })
                .Where(x => x.Attr != null)
                .Select(x => $"Свойство: {x.Name}, DisplayName: \"{x.Attr.DisplayName}\"")
                .ToList();
                if (!properties.Any())
                {
                    Console.WriteLine("Нет свойств с атрибутом DisplayName");
                }
            }
        }
    }
}

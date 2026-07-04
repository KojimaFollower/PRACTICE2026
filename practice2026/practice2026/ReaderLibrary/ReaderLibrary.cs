using System.Reflection;
using task07;
using static task07.AttributeReflection;
namespace ReaderLibrary
{
    class ReaderLibrary
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Путь не указан!");
                return;
            }
            string assemblypath = args[0];
            if (!File.Exists(assemblypath))
            {
                Console.WriteLine("Неизвестный путь!");
                return;
            }
            Assembly assembly = Assembly.LoadFrom(assemblypath);

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsClass) continue;

                ReflectionHelper.PrintTypeInfo(type);

                Console.WriteLine("Конструкторы:");
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var construct in constructors)
                {
                    var param = string.Join(", ", construct.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"  {type.Name}({param})");
                }

                Console.WriteLine("Методы:");
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    if (method.IsSpecialName) continue; 

                    var MethodAttribute = method.GetCustomAttribute<DisplayNameAttribute>();
                    string DisplayName = MethodAttribute != null ? $" {MethodAttribute.DisplayName}" :"";

                    var param = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"{method.ReturnType.Name} {method.Name}({param}){DisplayName}");
                }
            }
        }
    }
}

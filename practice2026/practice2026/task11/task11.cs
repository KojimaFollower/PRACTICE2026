using System.Reflection;
using task11;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Loader;

namespace task11
{
    public interface ICalculator
    {
        public int Add(int a, int b);
        public int Minus(int a, int b);
        public int Mul(int a, int b);
        public int Div(int a, int b);
    }
    public static class CalculatorCreator
    {
        public static ICalculator CreateCalculator()
        {
            string code = @"public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Minus(int a, int b) => a - b;
        public int Mul(int a, int b) => a * b;
        public int Div(int a, int b) => a / b;
    }";
            string modified = code.Replace("public class Calculator", "public class Calculator : task11.ICalculator");
            var syntax = CSharpSyntaxTree.ParseText(modified);
            string assname = Path.GetRandomFileName();
            var references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location)
            };

            var compilation = CSharpCompilation.Create(
                assname,
                syntaxTrees: new[] { syntax },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = string.Join("\n", result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).Select(d => d.GetMessage()));
                throw new InvalidOperationException("Ошибка компиляции!");
            }

            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);

            var type = assembly.GetType("Calculator");

            if (type == null)
                throw new TypeLoadException("Не удалось найти класс Calculator");

            return (ICalculator)Activator.CreateInstance(type);

        }

        

    }


}


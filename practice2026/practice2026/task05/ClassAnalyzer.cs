using System.Linq;
using System.Reflection;
namespace task05
{
    public class ClassAnalyzer
    {
        private Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }
 
        public IEnumerable<string> GetPublicMethods()
        {
            MethodInfo[] methods = _type.GetMethods();
            IEnumerable<string> publicmethods = methods.Where(m => !m.IsSpecialName).Select(m => m.Name);
            return publicmethods;
        }
        public IEnumerable<string> GetMethodParams(string methodname)
        {
            var method = _type.GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (method == null)
            {
                return Enumerable.Empty<string>();
            }

            var return_type = new[] { method.ReturnType.Name };
            var param_query = from param in method.GetParameters()
                              select $"{param.ParameterType.Name} {param.Name}";

            return return_type.Concat(param_query);
        }
        public IEnumerable<string> GetAllFields()
        {
            FieldInfo[] fields = _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            IEnumerable<string> result = fields.Select(f => f.Name);
            return result;

        }
        public IEnumerable<string> GetProperties()
        {
            PropertyInfo[] properties = _type.GetProperties();
            IEnumerable<string> result = properties.Select(p => p.Name); 
            return result;
        }
        public bool HasAttribute<T>() where T : Attribute
        {
            object[] attribute = _type.GetCustomAttributes(typeof(T), true);
            return attribute.Length > 0;
        }    
    }
}

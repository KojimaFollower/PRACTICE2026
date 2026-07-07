using System.Reflection;
using CommandLib;

namespace PluginSystem
{
    public class PluginManager
    {
        public List<ICommand> LoadedPlugins { get; set; } = new();

        public void LoadAndExecutePlugins(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Папка не найдена: {folderPath}");

            string[] dllfiles = Directory.GetFiles(folderPath, "*.dll");

            var allplugintypes = new List<Type>();

            foreach (string dll in dllfiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);
                    var plugintypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && t.IsDefined(typeof(PluginLoadAttribute), inherit: false));

                    allplugintypes.AddRange(plugintypes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");
                }
            }

            List<Type> sortedtypes = SortPluginsByDependencies(allplugintypes);

            foreach (Type type in sortedtypes)
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                {
                    var plugin = (ICommand)Activator.CreateInstance(type);
                    LoadedPlugins.Add(plugin);
                    plugin.Execute();
                }
                else
                {
                    throw new InvalidCastException($"Тип {type.FullName} не реализует ICommand.");
                }
            }
        }

        public List<Type> SortPluginsByDependencies(List<Type> types)
        {
            var sorted = new List<Type>();
            var visited = new Dictionary<Type, bool>(); 

            void Visit(Type type)
            {
                if (visited.TryGetValue(type, out bool isFullyVisited))
                {
                    if (!isFullyVisited)
                        throw new InvalidOperationException("Обнаружена циклическая зависимость между плагинами");
                    return;
                }

                visited[type] = false;

                var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attr != null)
                {
                    foreach (Type dependency in attr.dependencies)
                    {
                        Type foundDependency = types.FirstOrDefault(t => t == dependency);
                        if (foundDependency != null)
                        {
                            Visit(foundDependency);
                        }
                    }
                }

                visited[type] = true;
                sorted.Add(type);
            }

            foreach (Type type in types)
            {
                if (!visited.ContainsKey(type))
                {
                    Visit(type);
                }
            }

            return sorted;
        }
    }
}

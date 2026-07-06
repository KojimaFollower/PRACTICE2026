using System.Reflection;
using CommandLib;
namespace CommandRunner
{
    public class CommandRunner
    {
        static void Main()
        {
            string DLLPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");
            try
            {
                if (!File.Exists(DLLPath))
                {
                    Console.WriteLine("Данный DLL-файл не найден!");
                    return;
                }
                Assembly assembly = Assembly.LoadFrom(DLLPath);
                var types = assembly.GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass).ToList();
                var commandtypes = assembly.GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass).ToList();

                string DirectoryTest = Environment.CurrentDirectory;
                string MaskTest = "*";

                foreach (var type in commandtypes)
                {
                    ICommand command = null;
                    if (type.Name == "DirectorySizeCommand")
                    {
                        command = (ICommand)Activator.CreateInstance(type, DirectoryTest);
                    }
                    else if (type.Name == "FindFilesCommand")
                    {
                        command = (ICommand)Activator.CreateInstance(type, DirectoryTest, MaskTest);
                    }
                    command.Execute();
                }
            }
            catch (BadImageFormatException)
            {
                Console.WriteLine("DLL-файл имеет неверный формат!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось загрузить DLL-файл: {ex.Message}");
                return;
            }
            
        }
    }
}

using System.Reflection;
using CommandLib;
namespace CommandRunner
{
    public class CommandRunner
    {
        static void Main()
        {
            string DLLPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");
            Assembly assembly = Assembly.LoadFrom(DLLPath);
            var types = assembly.GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass).ToList();
            var commandtypes = assembly.GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass).ToList();

            string DirectoryTest= Environment.CurrentDirectory;
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
    }
}

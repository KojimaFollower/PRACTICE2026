using CommandLib;
using System.Drawing;
using System.Runtime;
namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private string DirectoryPath;
        public DirectorySizeCommand(string DirectoryPath)
        {
            this.DirectoryPath = DirectoryPath;
        }
        public void Execute()
        {
            var DirectoryInfo = new DirectoryInfo(DirectoryPath);
            long DirectorySize = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
                       .Sum(file => file.Length);
            Console.WriteLine($"Размер {DirectoryPath} - {DirectorySize} байт");
        }
        public class FindFilesCommand : ICommand
        {
            private readonly string DirectoryPath;
            private readonly string Mask;

            public FindFilesCommand(string DirectoryPath, string Mask)
            {
                this.DirectoryPath = DirectoryPath;
                this.Mask = Mask;
            }
            public void Execute()
            {
                var files = Directory.GetFiles(DirectoryPath, Mask);
                string NameFiles = string.Join(",", files);
                Console.WriteLine(NameFiles);
            }
        }
    }
    
}

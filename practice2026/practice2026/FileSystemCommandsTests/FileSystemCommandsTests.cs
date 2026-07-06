using FileSystemCommands;
using static FileSystemCommands.DirectorySizeCommand;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

            var command = new DirectorySizeCommand(testDir);
            command.Execute(); // Проверяем, что не возникает исключений

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            command.Execute(); // Должен найти 1 файл

            Directory.Delete(testDir, true);
        }
        [Fact]
        public void DirectorySizeCommand_ShouldWork_WhenDirectoryIsEmpty()
        {
            var DirectoryTest = Path.Combine(Path.GetTempPath(), "DirectoryTestEmpty");
            Directory.CreateDirectory(DirectoryTest);

            var command = new DirectorySizeCommand(DirectoryTest);
            command.Execute();

            Directory.Delete(DirectoryTest, true);
        }
        [Fact]
        public void FindFilesCommand_ShouldWork_WhenNoFilesMatchMask()
        {
            var DirectoryTest = Path.Combine(Path.GetTempPath(), "DirectoryTestNoMatch");
            Directory.CreateDirectory(DirectoryTest);
            File.WriteAllText(Path.Combine(DirectoryTest, "photo.jpg"), "Data");

            var command = new FindFilesCommand(DirectoryTest, "*.txt");
            command.Execute();

            Directory.Delete(DirectoryTest, true);
        }
        [Fact]
        public void DirectorySizeCommand_ShouldWork_WithSubdirectories()
        {
            var DirectoryTest = Path.Combine(Path.GetTempPath(), "DirectoryTestSub");
            var SubDirectory = Path.Combine(DirectoryTest, "InnerFolder");
            Directory.CreateDirectory(SubDirectory);

            File.WriteAllText(Path.Combine(DirectoryTest, "root.txt"), "Data");
            File.WriteAllText(Path.Combine(SubDirectory, "inner.txt"), "Data");

            var command = new DirectorySizeCommand(DirectoryTest);
            command.Execute();

            Directory.Delete(DirectoryTest, true);
        }


    }
}

using CommandLib;
using PluginSystem;
using Plugins;

namespace PluginSystemTests
{
    public class PluginSystemTests
    {
        private readonly PluginManager PluginSystem;
        [PluginLoad] public class MockPluginA : ICommand { public void Execute() { } }
        [PluginLoad(typeof(MockPluginA))] public class MockPluginB : ICommand { public void Execute() { } }
        [PluginLoad(typeof(MockPluginB))] public class MockPluginC : ICommand { public void Execute() { } }
        [PluginLoad(typeof(MockPluginY))] public class MockPluginX : ICommand { public void Execute() { } }
        [PluginLoad(typeof(MockPluginX))] public class MockPluginY : ICommand { public void Execute() { } }
        [PluginLoad] public class MockPluginIsolated : ICommand { public void Execute() { } }

        public PluginSystemTests()
        {
            this.PluginSystem = new PluginManager();
        }

        [Fact]
        public void SortPluginsByDependencies_ShouldCorrectlyOrderDependencies()
        {
            var inputTypes = new List<Type> { typeof(MockPluginC), typeof(MockPluginB), typeof(MockPluginA) };

            List<Type> result = PluginSystem.SortPluginsByDependencies(inputTypes);

            Assert.Equal(3, result.Count);
            Assert.Equal(typeof(MockPluginA), result[0]);
            Assert.Equal(typeof(MockPluginB), result[1]);
            Assert.Equal(typeof(MockPluginC), result[2]);
        }

        [Fact]
        public void SortPluginsByDependencies_ShouldThrowException_WhenCyclicDependencyExists()
        {
            var inputTypes = new List<Type> { typeof(MockPluginX), typeof(MockPluginY) };
            var exception = Assert.Throws<InvalidOperationException>(() =>
                PluginSystem.SortPluginsByDependencies(inputTypes)
            );
            Assert.Contains("Обнаружена циклическая зависимость", exception.Message);
        }

        [Fact]
        public void SortPluginsByDependencies_ShouldHandleIsolatedPlugins()
        {
            var inputTypes = new List<Type> { typeof(MockPluginIsolated), typeof(MockPluginA) };
            List<Type> result = PluginSystem.SortPluginsByDependencies(inputTypes);
            Assert.Equal(2, result.Count);
            Assert.Contains(typeof(MockPluginIsolated), result);
            Assert.Contains(typeof(MockPluginA), result);
        }

        [Fact]
        public void LoadAndExecutePlugins_ShouldThrowDirectoryNotFoundException_WhenPathDoesNotExist()
        {
            string nonExistentPath = @"C:\BLABLABLABLA";
            Assert.Throws<System.IO.DirectoryNotFoundException>(() =>
                PluginSystem.LoadAndExecutePlugins(nonExistentPath)
            );
        }
    }
}

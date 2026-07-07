using CommandLib;

namespace Plugins
{
    [PluginLoad] 
    public class PluginOne : ICommand
    {
        public void Execute() { }
    }

    [PluginLoad(typeof(PluginOne))] 
    public class PluginTwo : ICommand
    {
        public void Execute() { }
    }

    [PluginLoad(typeof(PluginTwo))] 
    public class PluginThree : ICommand
    {
        public void Execute() { }
    }
}

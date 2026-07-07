namespace CommandLib
{
    public interface ICommand
    {
        void Execute();
    }


    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PluginLoadAttribute : Attribute
    {
        public Type[] dependencies { get; }

        public PluginLoadAttribute(params Type[] dependencies)
        {
            if (dependencies != null)
            {
                this.dependencies = dependencies;
            }
            else
            {
                this.dependencies = Type.EmptyTypes;
            }
        }
    }
}

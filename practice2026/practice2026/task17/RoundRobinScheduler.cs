using task17;
public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> commands = new Queue<ICommand>();
    private readonly object locker = new object();

    public bool HasCommand()
    {
        lock (locker)
        {
            return commands.Count > 0;
        }
    }

    public void Add(ICommand cmd)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        lock (locker)
        {
            commands.Enqueue(cmd);
        }
    }

    public ICommand Select()
    {
        lock (locker)
        {
            if (commands.Count == 0)
            {
                return null;
            }
            return commands.Dequeue();
        }
    }
}
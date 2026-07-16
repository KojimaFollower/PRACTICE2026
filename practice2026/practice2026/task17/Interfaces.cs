using task17;
public interface ILongRunningCommand : ICommand
{
    bool IsCompleted { get; }
}

public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
}
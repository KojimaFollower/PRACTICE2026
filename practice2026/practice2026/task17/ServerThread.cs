using System.Collections.Concurrent;

namespace task17
{
    public interface ICommand
    {
        void Execute();
    }
    public interface IExceptionHandler
    {
        void Handle(Exception exception, ICommand command);
    }
    public class ServerThread
    {
        private readonly BlockingCollection<ICommand> queue = new();
        private readonly Thread thread;
        private readonly IExceptionHandler exceptionhandler;
        private Action behavior;
        private bool IsRunning = true;
        public ServerThread(IExceptionHandler exceptionHandler)
        {
            exceptionhandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
            thread = new Thread(Run);
            behavior = NormalBehavior;
        }

        public void Start() => thread.Start();
        public void QueueCommand(ICommand command)
        {
            if (queue.IsAddingCompleted) return;
            queue.Add(command);
        }

        private void Run()
        {
            while (IsRunning)
            {
                try
                {
                    behavior();
                }
                catch (Exception)
                {
                    StopImmediately();
                }
            }
        }
        private void NormalBehavior()
        {
            try
            {
                ICommand command = queue.Take();
                ExecuteCommand(command);
            }
            catch (InvalidOperationException)
            {
                
            }
        }
        private void SoftStopBehavior()
        {
            if (queue.TryTake(out ICommand command))
            {
                ExecuteCommand(command);
            }
            else
            {
                IsRunning = false; 
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                exceptionhandler.Handle(ex, command);
            }
        }

        public void UpdateBehavior(Action newBehavior)
        {
            behavior = newBehavior;
        }
        public void StopImmediately()
        {
            IsRunning = false;
        }

        public void MarkQueueAsCompleted()
        {
            queue.CompleteAdding();
        }

        public void SwitchToSoftStopBehavior()
        {
            UpdateBehavior(SoftStopBehavior);
        }

        public Thread UnderlyingThread => thread;
    }
    public class HardStop: ICommand
    {
        private readonly ServerThread serverthread;

        public HardStop(ServerThread serverThread)
        {
            serverthread = serverThread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != serverthread.UnderlyingThread)
            {
                throw new InvalidOperationException("HardStop может выполняться только внутри ServerThread!");
            }
            serverthread.StopImmediately();
        }
    }

    public class SoftStop: ICommand
    {
        private readonly ServerThread serverthread;

        public SoftStop(ServerThread serverThread)
        {
            serverthread = serverThread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != serverthread.UnderlyingThread)
            {
                throw new InvalidOperationException("SoftStop может выполняться только внутри ServerThread!");
            }
            serverthread.MarkQueueAsCompleted();
            serverthread.SwitchToSoftStopBehavior();
        }
    }

}

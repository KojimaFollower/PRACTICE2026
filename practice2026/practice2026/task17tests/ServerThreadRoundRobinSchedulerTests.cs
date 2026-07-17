using System.Collections.Concurrent;
using task17;
using Xunit;

namespace task17tests
{
    public class SimpleSpyCommand : ICommand
    {
        public bool WasExecuted { get; private set; }
        public void Execute() => WasExecuted = true;
    }

    public class RoundRobinSlicingTask : ILongRunningCommand
    {
        private readonly ServerThread _server;
        private readonly int _totalSteps;
        public int CompletedSteps { get; private set; } = 0;

        public bool IsCompleted => CompletedSteps >= _totalSteps;

        public RoundRobinSlicingTask(ServerThread server, int steps)
        {
            _server = server;
            _totalSteps = steps;
        }

        public void Execute()
        {
            if (!IsCompleted)
            {
                CompletedSteps++;
                if (!IsCompleted)
                {
                    _server.QueueCommand(this);
                }
            }
        }
    }

    public class TestExceptionHandler : IExceptionHandler
    {
        public ConcurrentQueue<(Exception Exception, ICommand Command)> HandledExceptions { get; } = new();
        public void Handle(Exception exception, ICommand command) => HandledExceptions.Enqueue((exception, command));
    }

    public class RoundRobinServerThreadTests
    {
        [Fact]
        public void ServerThread_ShouldExecuteSimpleCommandCorrectly()
        {
            var handler = new TestExceptionHandler();
            var server = new ServerThread(handler);
            var command = new SimpleSpyCommand();

            server.Start();
            server.QueueCommand(command);
            server.QueueCommand(new SoftStop(server));

            server.UnderlyingThread.Join(2000);

            Assert.True(command.WasExecuted);
        }

        [Fact]
        public void ServerThread_WithSlicingTasks_ShouldFullyCompleteThem()
        {
            var handler = new TestExceptionHandler();
            var server = new ServerThread(handler);
            var slicingTask = new RoundRobinSlicingTask(server, steps: 5);
            server.Start();
            server.QueueCommand(slicingTask);
            Thread.Sleep(100);
            server.QueueCommand(new SoftStop(server));

            server.UnderlyingThread.Join(2000);

            Assert.True(slicingTask.IsCompleted, $"Задача выполнила только {slicingTask.CompletedSteps} шагов из 5.");
            Assert.Equal(5, slicingTask.CompletedSteps);
        }

        [Fact]
        public void HardStop_ShouldTerminateRoundRobinExecutionImmediately()
        {
            var handler = new TestExceptionHandler();
            var server = new ServerThread(handler);

            var firstTask = new SimpleSpyCommand();
            var hardStop = new HardStop(server);
            var secondTask = new SimpleSpyCommand();

            server.Start();
            server.QueueCommand(firstTask);
            server.QueueCommand(hardStop);
            server.QueueCommand(secondTask);

            server.UnderlyingThread.Join(2000);

            Assert.True(firstTask.WasExecuted);
            Assert.False(secondTask.WasExecuted);
        }

        [Fact]
        public void SoftStop_ShouldAllowRunningTasksToFinishAllSlices()
        {
            var handler = new TestExceptionHandler();
            var server = new ServerThread(handler);
            var slicingTask = new RoundRobinSlicingTask(server, steps: 3);
            var softStop = new SoftStop(server);
            server.Start();
            server.QueueCommand(slicingTask);
            while (!slicingTask.IsCompleted)
            {
                Thread.Sleep(5);
            }

            server.QueueCommand(softStop);
            server.UnderlyingThread.Join(2000);
            Assert.True(slicingTask.IsCompleted, "Задача должна успешно завершить все шаги");
        }
    }
}

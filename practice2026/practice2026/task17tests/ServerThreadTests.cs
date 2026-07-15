using task17;
using Xunit;

namespace task17tests
{
    public class ServerThreadTests
    {
        private class RecordingCommand : ICommand
        {
            private readonly List<int> log;
            private readonly int id;
            private readonly Action action;

            public RecordingCommand(List<int> log, int id, Action action = null)
            {
                this.log = log;
                this.id = id;
                this.action = action;
            }

            public void Execute()
            {
                log.Add(id);
                action?.Invoke();
            }
        }
        private class SpyExceptionHandler : IExceptionHandler
        {
            public Exception LastException { get; private set; }
            public ICommand LastCommand { get; private set; }
            public bool Handled => LastException != null;

            public void Handle(Exception exception, ICommand command)
            {
                LastException = exception;
                LastCommand = command;
            }
        }

        [Fact]
        public void HardStop_DiscardsRemainingCommands()
        {
            var log = new List<int>();
            var handler = new SpyExceptionHandler();
            var server = new ServerThread(handler);
            server.QueueCommand(new RecordingCommand(log, 1));
            server.QueueCommand(new RecordingCommand(log, 2));
            server.QueueCommand(new HardStop(server));
            server.QueueCommand(new RecordingCommand(log, 3)); 
            server.Start();
            bool finished = server.UnderlyingThread.Join(2000);
            Assert.True(finished, "Поток не завершил работу.");
            Assert.Equal(new[] { 1, 2 }, log);
        }

        [Fact]
        public void SoftStop_ProcessesAllRemainingCommands_ThenStops()
        {
            var log = new List<int>();
            var handler = new SpyExceptionHandler();
            var server = new ServerThread(handler);
            server.QueueCommand(new RecordingCommand(log, 1));
            server.QueueCommand(new SoftStop(server));
            server.QueueCommand(new RecordingCommand(log, 2));
            server.Start();
            bool finished = server.UnderlyingThread.Join(2000);
            server.QueueCommand(new RecordingCommand(log, 3));
            Assert.True(finished, "Поток не завершил работу!");
            Assert.Equal(new[] { 1, 2 }, log);
        }

        [Fact]
        public void HardStop_ThrowsException_WhenExecutedOnForeignThread()
        {
            var handler = new SpyExceptionHandler();
            var server = new ServerThread(handler);
            var hardStop = new HardStop(server);
            Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
        }

        [Fact]
        public void SoftStop_ThrowsException_WhenExecutedOnForeignThread()
        {
            var handler = new SpyExceptionHandler();
            var server = new ServerThread(handler);
            var softStop = new SoftStop(server);
            Assert.Throws<InvalidOperationException>(() => softStop.Execute());
        }

        [Fact]
        public void ServerThread_PassesExceptionsToExceptionHandler()
        {
            var log = new List<int>();
            var handler = new SpyExceptionHandler();
            var server = new ServerThread(handler);
            var failingCommand = new RecordingCommand(log, 1, () => throw new NullReferenceException("Упс!"));

            server.QueueCommand(failingCommand);
            server.QueueCommand(new HardStop(server)); 
            server.Start();
            server.UnderlyingThread.Join(2000);
            Assert.True(handler.Handled, "Обработчик исключений не был вызван!");
            Assert.IsType<NullReferenceException>(handler.LastException);
            Assert.Same(failingCommand, handler.LastCommand);
        }
    }
}

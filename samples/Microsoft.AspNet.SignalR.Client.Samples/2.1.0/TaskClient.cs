using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Samples;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace Microsoft.AspNet.SignalR.Client.Samples
{
    public class TaskClient : ITaskAgent, ITaskScheduler
    {
        private TextWriter _traceWriter;
        private IHubProxy _hubProxy;

        public TaskClient(TextWriter traceWriter)
        {
            _traceWriter = traceWriter;
        }

        public async Task RunProgram(string url)
        {
            try
            {
                await RunDemo(url);
            }
            catch (Exception exception)
            {
                _traceWriter.WriteLine("Exception: {0}", exception);
                throw;
            }
        }

        private async Task RunDemo(string url)
        {
            var hubConnection = new HubConnection(url);
            hubConnection.TraceWriter = _traceWriter;

            _hubProxy = hubConnection.CreateHubProxy("TaskSchedulerHub");
            _hubProxy.On<TimeSpan>("RunSync", RunSync);
            _hubProxy.On<TimeSpan>("RunAsync", (data) => RunAsync(data));

            await hubConnection.Start(new LongPollingTransport());

            var smallDuration = TimeSpan.FromMilliseconds(400);
            var largeDuration = TimeSpan.FromSeconds(30);

            for (int i = 0; i < 10; i++ )
            {
                AssignMeShortRunningTask(smallDuration);
                AssignMeLongRunningTask(largeDuration);
            }
        }

        private void WriteLine(string value)
        {
            string message = string.Format("{0} - {1}", DateTime.UtcNow.ToString("HH:mm:ss.fffffff"), value);
            _traceWriter.WriteLine(message);
        }

        public void RunSync(TimeSpan duration)
        {
            WriteLine("Begin RunSync");
            Task.Delay(duration).Wait();
            WriteLine("Complete RunSync");
        }

        public async Task RunAsync(TimeSpan duration)
        {
            WriteLine("Begin RunAsync");
            await Task.Delay(duration);
            WriteLine("Complete RunAsync");
        }

        public void AssignMeShortRunningTask(TimeSpan duration)
        {
            _hubProxy.Invoke("AssignMeShortRunningTask", duration);
        }

        public void AssignMeLongRunningTask(TimeSpan duration)
        {
            _hubProxy.Invoke("AssignMeLongRunningTask", duration);
        }
    }
}

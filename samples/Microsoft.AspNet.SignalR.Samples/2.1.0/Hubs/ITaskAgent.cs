using System;
using System.Threading.Tasks;

namespace Microsoft.AspNet.SignalR.Samples
{
    public interface ITaskAgent
    {
        void RunSync(TimeSpan duration);
        Task RunAsync(TimeSpan duration);
    }
}
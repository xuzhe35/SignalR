using System;

namespace Microsoft.AspNet.SignalR.Samples
{
    public interface ITaskScheduler
    {
        void AssignMeShortRunningTask(TimeSpan duration);
        void AssignMeLongRunningTask(TimeSpan duration);
    }
}
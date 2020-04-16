using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assistant.Core.ActionQueue
{
    public static class TaskExtension
    {
        public static Task ToTask(this EventWaitHandle waitHandle)
        {
            if (waitHandle == null)
            {
                throw new ArgumentNullException(nameof(waitHandle));
            }

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            RegisteredWaitHandle rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
                delegate { tcs.TrySetResult(true); }, null, -1, true);

            Task<bool> t = tcs.Task;

            t.ContinueWith(antecedent => rwh.Unregister(null));

            return t;
        }

        public static Task ToTask(this IEnumerable<EventWaitHandle> waitHandles)
        {
            List<Task> tasks = waitHandles.Select(waitHandle => waitHandle.ToTask()).ToList();

            return Task.WhenAll(tasks);
        }
    }
}

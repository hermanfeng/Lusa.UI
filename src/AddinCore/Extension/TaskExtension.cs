using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CommonExtension
{
    public static class TaskExtension
    {
        public static Task<TResult> StartNewWithSynContext<TResult>(this TaskFactory taskFactory, Func<TResult> function)
        {
            var currentContext = SynchronizationContext.Current;
            Func<TResult> warppedAction = () =>
                {
                    SynchronizationContext.SetSynchronizationContext(currentContext);
                    return function();
                };
            return taskFactory.StartNew<TResult>(warppedAction);
        }

        public static Task StartNewWithSynContext(this TaskFactory taskFactory, Action action)
        {
            var currentContext = SynchronizationContext.Current;
            Action warppedAction = () =>
            {
                SynchronizationContext.SetSynchronizationContext(currentContext);
                action();
            };
            return taskFactory.StartNew(warppedAction);
        }
    }
}
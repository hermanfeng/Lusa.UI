using CommonExtension;
using System;
using System.Diagnostics;

namespace CommonLibrary
{
    public class MonitorService
    {
        static Lazy<MonitorService> lazyer = new Lazy<MonitorService>(() => new MonitorService(),true);
        private MonitorService()
        {

        }

        public static MonitorService Instance 
        {
            get
            {
                return lazyer.Value;
            }
        }

        public TimeSpan MonitorMethod(Action action, string methodName="")
        {
            if (action.IsNotNull())
            {
                methodName = methodName.IsNullOrEmpty() ? action.Method.Name : methodName;
                Stopwatch st = new Stopwatch();
                MessageService.Instance.SendMessage("Method named " + methodName + " begins to start.");
                st.Start();

                action();

                st.Stop();
                MessageService.Instance.SendMessage("Method named "+methodName +" ends with taking "+ st.ElapsedMilliseconds +" ms.");
                return st.Elapsed;
            }
            return TimeSpan.Zero;
        }

        public Tuple<T,TimeSpan> MonitorMethod<T>(Func<T> action, string methodName = "")
        {
            if (action.IsNotNull())
            {
                methodName = methodName.IsNullOrEmpty() ? action.Method.Name : methodName;
                Stopwatch st = new Stopwatch();
                MessageService.Instance.SendMessage("Method named " + methodName + " begins to start.");
                st.Start();

                T result = action();

                st.Stop();
                MessageService.Instance.SendMessage("Method named " + methodName + " ends with taking " + st.ElapsedMilliseconds + " ms.");
                return Tuple.Create(result,st.Elapsed);
            }
            return Tuple.Create(default(T),TimeSpan.Zero);
        }
    }
}

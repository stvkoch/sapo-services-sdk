using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace pt.sapo.gis.trace
{
    public class TraceEnabledTask : Task
    {
        
        private static Action EncloseAction(Action action) {
            var newTrace = TraceManager.Trace;
            return () =>
            {
                var oldTrace = TraceManager.Trace;
                TraceManager.Trace = newTrace;
                action();
                TraceManager.Trace = oldTrace;
            };
        }

        private static Action<T> EncloseActionWithParameter<T>(Action<T> action)
        {
            var newTrace = TraceManager.Trace;
            return t =>
            {
                var oldTrace = TraceManager.Trace;
                TraceManager.Trace = newTrace;
                action(t);
                TraceManager.Trace = oldTrace;
            };
        }

        public TraceEnabledTask(Action action) : base(EncloseAction(action)) { }

        public TraceEnabledTask(Action action, CancellationToken cancellationToken) : base(EncloseAction(action), cancellationToken) { }

        public TraceEnabledTask(Action action, TaskCreationOptions options) : base(EncloseAction(action), options) { }

        public TraceEnabledTask(Action action, CancellationToken cancellationToken, TaskCreationOptions options) : base(EncloseAction(action), cancellationToken, options) { }

        public TraceEnabledTask(Action<object> action, object state) : base(EncloseActionWithParameter<object>(action), state) { }

        public TraceEnabledTask(Action<object> action, object state, CancellationToken cancellationToken) : base(EncloseActionWithParameter<object>(action), state, cancellationToken) { }

        public TraceEnabledTask(Action<object> action, object state, TaskCreationOptions options) : base(EncloseActionWithParameter<object>(action), state, options) { }

        public TraceEnabledTask(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions options) : base(EncloseActionWithParameter<object>(action), state, cancellationToken, options) { }
    }
}

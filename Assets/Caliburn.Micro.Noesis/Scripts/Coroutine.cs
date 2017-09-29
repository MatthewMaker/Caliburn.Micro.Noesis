#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

namespace Caliburn.Micro {
    using System;
    using System.Collections.Generic;
#if ENABLE_TASKS
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// Manages coroutine execution.
    /// </summary>
    public static class Coroutine {
        static readonly ILog Log = LogManager.GetLog(typeof(Coroutine));

        /// <summary>
        /// Creates the parent enumerator.
        /// </summary>
        public static Func<IEnumerator<IResult>, IResult> CreateParentEnumerator = inner => new SequentialResult(inner);

        /// <summary>
        /// Executes a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to execute.</param>
        /// <param name="context">The context to execute the coroutine within.</param>
        /// /// <param name="callback">The completion callback for the coroutine.</param>
        public static void BeginExecute(IEnumerator<IResult> coroutine, CoroutineExecutionContext context = null, EventHandler<ResultCompletionEventArgs> callback = null) {
            Log.Info("Executing coroutine.");

            var enumerator = CreateParentEnumerator(coroutine);
            IoC.BuildUp(enumerator);

            if (callback != null) {
                ExecuteOnCompleted(enumerator, callback);
            }

            ExecuteOnCompleted(enumerator, Completed);
            enumerator.Execute(context ?? new CoroutineExecutionContext());
        }

#if ENABLE_TASKS
        /// <summary>
        /// Executes a coroutine asynchronous.
        /// </summary>
        /// <param name="coroutine">The coroutine to execute.</param>
        /// <param name="context">The context to execute the coroutine within.</param>
        /// <returns>A task that represents the asynchronous coroutine.</returns>
        public static Task ExecuteAsync(IEnumerator<IResult> coroutine, CoroutineExecutionContext context = null) {
            var taskSource = new TaskCompletionSource<object>();

            BeginExecute(coroutine, context, (s, e) => {
                if (e.Error != null)
                    taskSource.SetException(e.Error);
                else if (e.WasCancelled)
                    taskSource.SetCanceled();
                else
                    taskSource.SetResult(null);
            });

            return taskSource.Task;
        }
#endif

        static void ExecuteOnCompleted(IResult result, EventHandler<ResultCompletionEventArgs> handler) {
            EventHandler<ResultCompletionEventArgs> onCompledted = null;
            onCompledted = (s, e) => {
                result.Completed -= onCompledted;
                handler(s, e);
            };
            result.Completed += onCompledted;
        }

        /// <summary>
        /// Called upon completion of a coroutine.
        /// </summary>
        public static event EventHandler<ResultCompletionEventArgs> Completed = (s, e) => {
            if(e.Error != null) {
                Log.Error(e.Error);
            }
            else if(e.WasCancelled) {
                Log.Info("Coroutine execution cancelled.");
            }
            else {
                Log.Info("Coroutine execution completed.");
            }
        };
    }
}

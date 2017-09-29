#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

namespace Caliburn.Micro {
    using System.Threading;
#if ENABLE_TASKS
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// Extensions for <see cref="IEventAggregator"/>.
    /// </summary>
    public static class EventAggregatorExtensions {
        /// <summary>
        /// Publishes a message on the current thread (synchrone).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnCurrentThread(this IEventAggregator eventAggregator, object message) {
            eventAggregator.Publish(message, action => action());
        }

#if ENABLE_TASKS
        /// <summary>
        /// Publishes a message on a background thread (async).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnBackgroundThread(this IEventAggregator eventAggregator, object message) {
            eventAggregator.Publish(message, action => Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default));
        }
#endif

        /// <summary>
        /// Publishes a message on the UI thread.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnUIThread(this IEventAggregator eventAggregator, object message) {
            eventAggregator.Publish(message, Execute.OnUIThread);
        }

        /// <summary>
        /// Publishes a message on the UI thread asynchrone.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void BeginPublishOnUIThread(this IEventAggregator eventAggregator, object message) {
            eventAggregator.Publish(message, Execute.BeginOnUIThread);
        }

#if ENABLE_TASKS
        /// <summary>
        /// Publishes a message on the UI thread asynchrone.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="message">The message instance.</param>
        public static Task PublishOnUIThreadAsync(this IEventAggregator eventAggregator, object message) {
            Task task = null;
            eventAggregator.Publish(message, action => task = action.OnUIThreadAsync());
            return task;
        }
#endif
    }
}

#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

namespace Caliburn.Micro {
    using System;
#if ENABLE_TASKS
    using System.Threading.Tasks;
#endif

    /// <summary>
    ///   Enables easy marshalling of code to the UI thread.
    /// </summary>
    public static class Execute {
        /// <summary>
        ///   Indicates whether or not the framework is in design-time mode.
        /// </summary>
        public static bool InDesignMode {
            get {
                return PlatformProvider.Current.InDesignMode;
            }
        }

        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void BeginOnUIThread(this System.Action action) {
            PlatformProvider.Current.BeginOnUIThread(action);
        }

#if ENABLE_TASKS
        /// <summary>
        ///   Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static Task OnUIThreadAsync(this System.Action action) {
            return PlatformProvider.Current.OnUIThreadAsync(action);
        }
#endif

        /// <summary>
        ///   Executes the action on the UI thread.
        /// </summary>
        /// <param name = "action">The action to execute.</param>
        public static void OnUIThread(this System.Action action) {
            PlatformProvider.Current.OnUIThread(action);
        }
    }
}

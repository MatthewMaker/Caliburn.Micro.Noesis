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
    /// Default implementation for <see cref="IPlatformProvider"/> that does no platform enlightenment.
    /// </summary>
    public class DefaultPlatformProvider : IPlatformProvider {
        /// <summary>
        /// Indicates whether or not the framework is in design-time mode.
        /// </summary>
        public bool InDesignMode {
            get { return true; }
        }

        /// <summary>
        /// Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void BeginOnUIThread(System.Action action) {
            action();
        }

#if ENABLE_TASKS
        /// <summary>
        /// Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        public Task OnUIThreadAsync(System.Action action) {
            return Task.Factory.StartNew(action);
        }
#endif

        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void OnUIThread(System.Action action) {
            action();
        }

        /// <summary>
        /// Used to retrieve the root, non-framework-created view.
        /// </summary>
        /// <param name="view">The view to search.</param>
        /// <returns>
        /// The root element that was not created by the framework.
        /// </returns>
        /// <remarks>
        /// In certain instances the services create UI elements.
        /// For example, if you ask the window manager to show a UserControl as a dialog, it creates a window to host the UserControl in.
        /// The WindowManager marks that element as a framework-created element so that it can determine what it created vs. what was intended by the developer.
        /// Calling GetFirstNonGeneratedView allows the framework to discover what the original element was.
        /// </remarks>
        public object GetFirstNonGeneratedView(object view) {
            return view;
        }

        /// <summary>
        /// Executes the handler the fist time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        /// <returns>true if the handler was executed immediately; false otherwise</returns>
        public void ExecuteOnFirstLoad(object view, Action<object> handler) {
            handler(view);
        }

        /// <summary>
        /// Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public void ExecuteOnLayoutUpdated(object view, Action<object> handler) {
            handler(view);
        }

        /// <summary>
        /// Get the close action for the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model to close.</param>
        /// <param name="views">The associated views.</param>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns>
        /// An <see cref="Action" /> to close the view model.
        /// </returns>
        public System.Action GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult) {
            return () => { };
        }
    }
}

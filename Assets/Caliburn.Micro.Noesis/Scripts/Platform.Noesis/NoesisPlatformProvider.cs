// <copyright file="NoesisPlatformProvider.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Threading;

    #endregion

    public class NoesisPlatformProvider : IPlatformProvider
    {
        /// <summary>
        ///     Gets a value indicating whether the framework is in design-time mode.
        /// </summary>
        public bool InDesignMode
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Executes the action on the UI thread asynchronously.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void BeginOnUIThread(System.Action action)
        {
            MainThreadDispatcher.Instance.Enqueue(action);
        }

        /// <summary>
        ///     Executes the handler the first time the view is loaded.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public void ExecuteOnFirstLoad(object view, System.Action<object> handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Executes the handler the next time the view's LayoutUpdated event fires.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="handler">The handler.</param>
        public void ExecuteOnLayoutUpdated(object view, System.Action<object> handler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Used to retrieve the root, non-framework-created view.
        /// </summary>
        /// <param name="view">The view to search.</param>
        /// <returns>The root element that was not created by the framework.</returns>
        /// <remarks>
        ///     In certain instances the services create UI elements.
        ///     For example, if you ask the window manager to show a UserControl as a dialog, it creates a window to host the
        ///     UserControl in.
        ///     The WindowManager marks that element as a framework-created element so that it can determine what it created vs.
        ///     what was intended by the developer.
        ///     Calling GetFirstNonGeneratedView allows the framework to discover what the original element was.
        /// </remarks>
        public object GetFirstNonGeneratedView(object view)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Get the close action for the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model to close.</param>
        /// <param name="views">The associated views.</param>
        /// <param name="dialogResult">The dialog result.</param>
        /// <returns>An <see cref="Action" /> to close the view model.</returns>
        public System.Action GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void OnUIThread(System.Action action)
        {
            if (Thread.CurrentThread == MainThreadDispatcher.Instance.MainThread)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exception)
                {
                    MainThreadDispatcher.Instance.UnhandledExceptionCallback(exception);
                }
            }
            else
            {
                MainThreadDispatcher.Instance.Enqueue(action);
            }
        }
    }
}
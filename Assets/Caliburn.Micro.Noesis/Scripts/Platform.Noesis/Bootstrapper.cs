#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

// <copyright file="Bootstrapper.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Noesis;
    using UnityEngine;

    #endregion

    /// <summary>
    ///     Inherit from this class in order to customize the configuration of the framework.
    /// </summary>
    public abstract class BootstrapperBase : MonoBehaviour
    {
        #region Constants and Fields

        private const string RootContentControlName = "Content";

        private bool isInitialized;

        [SerializeField]
        private NoesisView noesisView;

        #endregion

        protected NoesisView NoesisView
        {
            get
            {
                return this.noesisView;
            }
        }

        /// <summary>
        ///     Initialize the framework.
        /// </summary>
        public void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }

            this.isInitialized = true;

            PlatformProvider.Current = new NoesisPlatformProvider();
            Func<Assembly, IEnumerable<Type>> baseExtractTypes = AssemblySourceCache.ExtractTypes;

            AssemblySourceCache.ExtractTypes = assembly =>
            {
                IEnumerable<Type> baseTypes = baseExtractTypes(assembly);
                IEnumerable<Type> elementTypes = assembly.GetExportedTypes().Where(t => typeof(UIElement).IsAssignableFrom(t));

                return baseTypes.Union(elementTypes);
            };

            AssemblySource.Instance.Refresh();

            if (Execute.InDesignMode)
            {
                try
                {
                    StartDesignTime();
                }
                catch
                {
                    // If something fails at design-time, there's really nothing we can do...
                    this.isInitialized = false;
                    throw;
                }
            }
            else
            {
                StartRuntime();
            }
        }

        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected virtual void BuildUp(object instance)
        {
        }

        /// <summary>
        ///     Override to configure the framework and setup your IoC container.
        /// </summary>
        protected virtual void Configure()
        {
        }

        /// <summary>
        ///     Locates the view model, locates the associate view, binds them and shows it as the root view.
        /// </summary>
        /// <param name="viewModelType">The view model type.</param>
        protected void DisplayRootViewFor(Type viewModelType)
        {
            var rootElement = NoesisView.Content;
            var contentControl = (ContentControl)rootElement.FindName(RootContentControlName);

            object viewModel = IoC.GetInstance(viewModelType, null);

            UIElement view = ViewLocator.LocateForModel(viewModel, null, null);

            ViewModelBinder.Bind(viewModel, view, null);

            var activator = viewModel as IActivate;

            if (activator != null)
            {
                activator.Activate();
            }

            contentControl.Content = view;
        }

        /// <summary>
        ///     Locates the view model, locates the associate view, binds them and shows it as the root view.
        /// </summary>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        protected void DisplayRootViewFor<TViewModel>()
        {
            DisplayRootViewFor(typeof(TViewModel));
        }

        /// <summary>
        ///     Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        protected virtual IEnumerable<object> GetAllInstances(Type service)
        {
            return new[]
                   {
                       Activator.CreateInstance(service)
                   };
        }

        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The located service.</returns>
        protected virtual object GetInstance(Type service, string key)
        {
            return Activator.CreateInstance(service);
        }

        /// <summary>
        ///     Override this to add custom behavior on exit.
        /// </summary>
        protected virtual void OnExit()
        {
        }

        /// <summary>
        ///     Override this to add custom behavior to execute after the application starts.
        /// </summary>
        protected virtual void OnStartup()
        {
        }

        /// <summary>
        ///     Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="exception">
        ///     The unhandled exception.
        /// </param>
        protected virtual void OnUnhandledException(Exception exception)
        {
        }

        /// <summary>
        ///     Provides an opportunity to hook into the application object.
        /// </summary>
        protected virtual void PrepareApplication()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => OnUnhandledException(args.ExceptionObject as Exception);
            MainThreadDispatcher.RegisterUnhandledExceptionCallback(OnUnhandledException);
        }

        /// <summary>
        ///     Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected virtual IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
                   {
                       GetType().Assembly
                   };
        }

        /// <summary>
        ///     Called by the bootstrapper's constructor at design time to start the framework.
        /// </summary>
        protected virtual void StartDesignTime()
        {
            AssemblySource.Instance.Clear();
            AssemblySource.Instance.AddRange(SelectAssemblies());

            Configure();

            IoC.GetInstance = GetInstance;
            IoC.GetAllInstances = GetAllInstances;
            IoC.BuildUp = BuildUp;
        }

        /// <summary>
        ///     Called by the bootstrapper's constructor at runtime to start the framework.
        /// </summary>
        protected virtual void StartRuntime()
        {
            EventAggregator.HandlerResultProcessing = (target, result) =>
            {
#if ENABLE_TASKS
                var task = result as System.Threading.Tasks.Task;

                if (task != null)
                {
                    result = new IResult[] { task.AsResult() };
                }
#endif
                var coroutine = result as IEnumerable<IResult>;

                if (coroutine != null)
                {
                    var viewAware = target as IViewAware;
                    object view = viewAware != null ? viewAware.GetView() : null;
                    var context = new CoroutineExecutionContext { Target = target, View = view };

                    Coroutine.BeginExecute(coroutine.GetEnumerator(), context);
                }
            };

            AssemblySourceCache.Install();
            AssemblySource.Instance.AddRange(SelectAssemblies());

            PrepareApplication();
            Configure();

            IoC.GetInstance = GetInstance;
            IoC.GetAllInstances = GetAllInstances;
            IoC.BuildUp = BuildUp;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<MainThreadDispatcher>();
            Initialize();
            OnStartup();
        }

        private void OnApplicationQuit()
        {
            OnExit();
        }
    }
}
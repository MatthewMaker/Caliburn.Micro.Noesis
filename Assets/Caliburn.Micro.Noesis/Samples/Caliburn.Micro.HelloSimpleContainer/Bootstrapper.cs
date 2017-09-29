// <copyright file="Bootstrapper.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloSimpleContainer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;

    #endregion

    [UsedImplicitly, AddComponentMenu("Caliburn.Micro/Samples/HelloSimpleContainerBootstrapper")]
    public class Bootstrapper : BootstrapperBase
    {
        #region Constants and Fields

        private SimpleContainer container;

        #endregion

        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }

        /// <summary>
        ///     Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this.container = new SimpleContainer();

            //this.container.Singleton<IWindowManager, WindowManager>();
            this.container.Singleton<IEventAggregator, EventAggregator>();
            this.container.PerRequest<IShell, ShellViewModel>();
        }

        /// <summary>
        ///     Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        /// <summary>
        ///     Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The located service.</returns>
        protected override object GetInstance(Type service, string key)
        {
            return this.container.GetInstance(service, key);
        }

        /// <summary>
        ///     Override this to add custom behavior to execute after the application starts.
        /// </summary>
        protected override void OnStartup()
        {
            DisplayRootViewFor<IShell>();
        }

        /// <summary>
        ///     Override this to add custom behavior for unhandled exceptions.
        /// </summary>
        /// <param name="exception">
        ///     The unhandled exception.
        /// </param>
        protected override void OnUnhandledException(Exception exception)
        {
            Debug.LogError(exception);
        }
    }
}
// <copyright file="NoesisBootstrapper.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloWorld
{
    #region Using Directives

    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    #endregion

    [UsedImplicitly, AddComponentMenu("Caliburn.Micro/Samples/HelloWorldBootstrapper")]
    public class NoesisBootstrapper : BootstrapperBase
    {
        /// <summary>
        ///     Override this to add custom behavior to execute after the application starts.
        /// </summary>
        protected override void OnStartup()
        {
            DisplayRootViewFor<ShellViewModel>();
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
// <copyright file="HelloBootstrapper.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.Hello
{
    #region Using Directives

    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    #endregion

    [UsedImplicitly, AddComponentMenu("Caliburn.Micro/Samples/HelloBootstrapper")]
    public class HelloBootstrapper : BootstrapperBase
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
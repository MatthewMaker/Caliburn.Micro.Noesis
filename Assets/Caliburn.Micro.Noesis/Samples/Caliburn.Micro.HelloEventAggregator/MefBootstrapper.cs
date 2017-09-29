// <copyright file="MEFBootstrapper.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloEventAggregator
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using JetBrains.Annotations;
    using UnityEngine;

    #endregion

    [UsedImplicitly, AddComponentMenu("Caliburn.Micro/Samples/HelloEventAggregatorBootstrapper")]
    public class MefBootstrapper : BootstrapperBase
    {
        #region Constants and Fields

        private CompositionContainer container;

        #endregion

        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        protected override void Configure()
        {
            // An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();

            // Adds all the parts found in the same assembly as the Program class  
            catalog.Catalogs.Add(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            // Create the CompositionContainer with the parts in the catalog  
            this.container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            //batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(this.container);

            //Fill the imports of this object  
            try
            {
                this.container.Compose(batch);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            IEnumerable<object> exports = this.container.GetExportedValues<object>(contract);

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override void OnStartup()
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
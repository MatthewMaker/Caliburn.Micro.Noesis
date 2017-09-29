// <copyright file="ShellViewModel.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloEventAggregator
{
    #region Using Directives

    using System.ComponentModel.Composition;

    #endregion

    [Export(typeof(IShell))]
    public class ShellViewModel : PropertyChangedBase, IShell, IHandle<object>
    {
        [ImportingConstructor]
        public ShellViewModel(LeftViewModel left, RightViewModel right, IEventAggregator events)
        {
            Left = left;
            Right = right;

            events.Subscribe(this);
        }

        public string LastEvent { get; private set; } = "No Events Yet";

        public LeftViewModel Left { get; private set; }
        public RightViewModel Right { get; private set; }

        public void Handle(object message)
        {
            LastEvent = "Last Event: " + message;
            NotifyOfPropertyChange(() => LastEvent);
        }
    }
}
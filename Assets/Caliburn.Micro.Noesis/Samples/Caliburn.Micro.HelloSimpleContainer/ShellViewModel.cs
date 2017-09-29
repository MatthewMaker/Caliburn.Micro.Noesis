// <copyright file="ShellViewModel.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloSimpleContainer
{
    #region Using Directives

    using System.Linq;
    using Noesis.Samples;
    using UnityEngine;

    #endregion

    public class ShellViewModel : PropertyChangedBase, IShell
    {
        #region Constants and Fields

        private string name;

        #endregion

        /// <summary>
        ///     Creates an instance of <see cref="ShellViewModel" />.
        /// </summary>
        public ShellViewModel()
        {
            SayHelloCommand = new DelegateCommand(o => CanSayHello, o => SayHello());
        }

        public bool CanSayHello => !IsNullOrWhiteSpace(Name);

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                NotifyOfPropertyChange(() => Name);
                SayHelloCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SayHelloCommand { get; private set; }

        public void SayHello()
        {
            Debug.Log($"Hello {Name}!");
        }

        private static bool IsNullOrWhiteSpace(string s)
        {
            return string.IsNullOrEmpty(s) || s.All(char.IsWhiteSpace);
        }
    }
}
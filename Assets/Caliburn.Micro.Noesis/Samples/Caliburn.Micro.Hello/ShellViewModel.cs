namespace Caliburn.Micro.Hello
{
    using System.Windows;
    using UnityEngine;
    using System.Linq;
    using HelloWorld;
    using Noesis.Samples;

    public class ShellViewModel : PropertyChangedBase
    {
        public DelegateCommand SayHelloCommand { get; private set; }

        /// <summary>
        ///     Creates an instance of <see cref="ShellViewModel" />.
        /// </summary>
        public ShellViewModel()
        {
            SayHelloCommand = new DelegateCommand(o => CanSayHello, o => SayHello());
        }

        private string name;

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

        public bool CanSayHello => !IsNullOrWhiteSpace(Name);

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
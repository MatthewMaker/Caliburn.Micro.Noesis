// <copyright file="ShellView.xaml.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>


#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

// <copyright file="ShellView.xaml.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro.HelloSimpleContainer
{
    #region Using Directives

#if NOESIS
    using Noesis;

#else
    using System.Windows.Controls;

#endif

    #endregion

    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            GUI.LoadComponent(this, "Assets/Caliburn.Micro.Noesis/Samples/Caliburn.Micro.HelloSimpleContainer/ShellView.xaml");
        }
#endif
    }
}
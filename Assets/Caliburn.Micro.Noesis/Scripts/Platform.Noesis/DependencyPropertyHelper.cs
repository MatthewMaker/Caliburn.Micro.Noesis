#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

using System;
#if NOESIS
using Noesis;
#elif XFORMS
using Xamarin.Forms;
using DependencyProperty = Xamarin.Forms.BindableProperty;
#elif WinRT
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Caliburn.Micro
{
    /// <summary>
    /// Class that abstracts the differences in creating a DepedencyProperty / BindableProperty on the different platforms.
    /// </summary>
    public static class DependencyPropertyHelper
    {
        /// <summary>
        /// Register an attached dependency / bindable property
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="propertyType">The property type</param>
        /// <param name="ownerType">The owner type</param>
        /// <param name="defaultValue">The default value</param>
        /// <param name="propertyChangedCallback">Callback to executed on property changed</param>
        /// <returns>The registred attached dependecy property</returns>
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, object defaultValue = null, PropertyChangedCallback propertyChangedCallback = null) {
#if XFORMS
            return DependencyProperty.CreateAttached(name, propertyType, ownerType, defaultValue, propertyChanged: (obj, oldValue, newValue) => {
                if (propertyChangedCallback != null)
                    propertyChangedCallback(obj, new DependencyPropertyChangedEventArgs(newValue, oldValue, null));
            });
#else
            return DependencyProperty.RegisterAttached(name, propertyType, ownerType, new PropertyMetadata(defaultValue, propertyChangedCallback));
#endif
        }

        /// <summary>
        /// Register a dependency / bindable property
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="propertyType">The property type</param>
        /// <param name="ownerType">The owner type</param>
        /// <param name="defaultValue">The default value</param>
        /// <param name="propertyChangedCallback">Callback to executed on property changed</param>
        /// <returns>The registred dependecy property</returns>
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, object defaultValue = null, PropertyChangedCallback propertyChangedCallback = null)
        {
#if XFORMS
            return DependencyProperty.Create(name, propertyType, ownerType, defaultValue, propertyChanged: (obj, oldValue, newValue) =>
            {
                if (propertyChangedCallback != null)
                    propertyChangedCallback(obj, new DependencyPropertyChangedEventArgs(newValue, oldValue, null));
            });
#else
            return DependencyProperty.Register(name, propertyType, ownerType, new PropertyMetadata(defaultValue, propertyChangedCallback));
#endif
        }
    }
}

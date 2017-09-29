#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

namespace Caliburn.Micro {
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///   Interface used to define an object associated to a collection of children.
    /// </summary>
    public interface IParent {
        /// <summary>
        ///   Gets the children.
        /// </summary>
        /// <returns>
        ///   The collection of children.
        /// </returns>
        IEnumerable GetChildren();
    }

    /// <summary>
    /// Interface used to define a specialized parent.
    /// </summary>
    /// <typeparam name="T">The type of children.</typeparam>

#if NOESIS
    public interface IParent<T> : IParent
#else
    public interface IParent<out T> : IParent
#endif
    {
        /// <summary>
        ///   Gets the children.
        /// </summary>
        /// <returns>
        ///   The collection of children.
        /// </returns>
        new IEnumerable<T> GetChildren();
    }
}

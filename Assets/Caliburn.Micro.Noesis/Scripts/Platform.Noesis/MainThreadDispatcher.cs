// <copyright file="MainThreadDispatcher.cs" company="VacuumBreather">
//      Copyright © 2017 VacuumBreather. All rights reserved.
// </copyright>

namespace Caliburn.Micro
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    #endregion

    /// <summary>
    ///     A thread-safe class which holds a queue with actions to execute on the next Update() method. It can be used to make
    ///     calls to the main thread for
    ///     things such as UI Manipulation in Unity. It was developed for use in combination with the Firebase Unity plugin,
    ///     which uses separate threads for event handling
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour
    {
        #region Constants and Fields

        private static readonly Queue<System.Action> _executionQueue = new Queue<System.Action>();

        private static MainThreadDispatcher _instance = null;

        private Action<Exception> unhandledExceptionCallback = exception => { };

        private readonly object _gate = new object();

        #endregion

        public MainThreadDispatcher()
        {
            MainThread = null;
        }

        public static MainThreadDispatcher Instance
        {
            get
            {
                if (!Exists())
                {
                    throw new Exception("MainThreadDispatcher not found.");
                }

                return _instance;
            }
        }

        public Thread MainThread { get; private set; }

        public Action<Exception> UnhandledExceptionCallback
        {
            get
            {
                return this.unhandledExceptionCallback;
            }
        }

        public static bool Exists()
        {
            return _instance != null;
        }

        public static void RegisterUnhandledExceptionCallback(Action<Exception> exceptionCallback)
        {
            if (exceptionCallback == null)
            {
                Instance.unhandledExceptionCallback = exception => { };
            }
            else
            {
                Instance.unhandledExceptionCallback = exceptionCallback;
            }
        }

        /// <summary>
        ///     Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            lock (this._gate)
            {
                _executionQueue.Enqueue(() => { StartCoroutine(action); });
            }
        }

        /// <summary>
        ///     Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(System.Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        public void Update()
        {
            lock (this._gate)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        private IEnumerator ActionWrapper(System.Action action)
        {
            action();
            yield return null;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                MainThread = Thread.CurrentThread;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
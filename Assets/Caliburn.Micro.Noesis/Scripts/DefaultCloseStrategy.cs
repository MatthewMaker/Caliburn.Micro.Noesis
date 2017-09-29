namespace Caliburn.Micro {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Used to gather the results from multiple child elements which may or may not prevent closing.
    /// </summary>
    /// <typeparam name="T">The type of child element.</typeparam>
    public class DefaultCloseStrategy<T> : ICloseStrategy<T> {
        readonly bool closeConductedItemsWhenConductorCannotClose;

        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="closeConductedItemsWhenConductorCannotClose">Indicates that even if all conducted items are not closable, those that are should be closed. The default is FALSE.</param>
        public DefaultCloseStrategy(bool closeConductedItemsWhenConductorCannotClose = false) {
            this.closeConductedItemsWhenConductorCannotClose = closeConductedItemsWhenConductorCannotClose;
        }

        /// <summary>
        /// Executes the strategy.
        /// </summary>
        /// <param name="toClose">Items that are requesting close.</param>
        /// <param name="callback">The action to call when all enumeration is complete and the close results are aggregated.
        /// The bool indicates whether close can occur. The enumerable indicates which children should close if the parent cannot.</param>
        public void Execute(IEnumerable<T> toClose, Action<bool, IEnumerable<T>> callback)
        {
            var toCloseArray = toClose.ToArray();
            Evaluate(new EvaluationState() { StillToEvaluate = toCloseArray.Length }, toCloseArray, callback);
        }

        void Evaluate(EvaluationState state, IEnumerable<T> toClose, Action<bool, IEnumerable<T>> callback) {
            foreach (var closable in toClose) {
                var current = closable;
                var guard = current as IGuardClose;

                if (guard != null) {
                    guard.CanClose(canClose => {
                        if (canClose) {
                            state.Closable.Add(current);
                        }

                        state.FinalResult = state.FinalResult && canClose;
                        state.StillToEvaluate--;
                        Evaluate(state, new T[] { }, callback);
                    });
                } else {
                    state.Closable.Add(current);
                    state.StillToEvaluate--;
                }
            }

            if (state.StillToEvaluate == 0 && !state.FinishedEvaluation)
            {
                state.FinishedEvaluation = true;
                callback(state.FinalResult, closeConductedItemsWhenConductorCannotClose ? state.Closable : new List<T>());
            }
        }

        class EvaluationState {
            public readonly List<T> Closable = new List<T>();
            public bool FinalResult = true;
            public bool FinishedEvaluation;
            public int StillToEvaluate = 0;
        }
    }
}

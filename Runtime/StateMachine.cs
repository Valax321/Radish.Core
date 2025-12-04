using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Radish
{
    /// <summary>
    /// A re-usable and simple state machine controller.
    /// </summary>
    /// <typeparam name="TState">The base type for states.</typeparam>
    /// <typeparam name="TStateKey">The type of key representing each state.</typeparam>
    [PublicAPI]
    public class StateMachine<TState, TStateKey> where TStateKey : IEquatable<TStateKey>
    {
        /// <summary>
        /// The currently active state.
        /// </summary>
        public TState current => m_Lookup[m_CurrentStateKey];

        protected bool isCurrentStateValid => m_Lookup.ContainsKey(m_CurrentStateKey);

        /// <summary>
        /// Enumerable collection of all states in the state machine.
        /// </summary>
        public IEnumerable<TState> states => m_Lookup.Values;

        private readonly Dictionary<TStateKey, TState> m_Lookup;
        private TStateKey m_CurrentStateKey;
        private readonly Action<TState, TStateKey> m_OnEnterMethod;
        private readonly Action<TState, TStateKey> m_OnExitMethod;

        /// <summary>
        /// Creates a new state machine.
        /// </summary>
        /// <param name="states">List of states that will be available.</param>
        /// <param name="initialState">Key for the initially active state.</param>
        /// <param name="keyGetter">Method for getting a state's key. Only used at construction time.</param>
        /// <param name="onEnterMethod">Callback for activating a state when it becomes active.</param>
        /// <param name="onExitMethod">Callback for deactivating a state when it becomes inactive.</param>
        /// <param name="comparer">Optional comparer for the state key.</param>
        public StateMachine(IEnumerable<TState> states, TStateKey initialState,
            Func<TState, TStateKey> keyGetter,
            Action<TState, TStateKey> onEnterMethod,
            Action<TState, TStateKey> onExitMethod,
            IEqualityComparer<TStateKey> comparer = null)
        {
            m_Lookup = comparer is not null 
                ? new Dictionary<TStateKey, TState>(comparer) 
                : new Dictionary<TStateKey, TState>();
            
            m_CurrentStateKey = initialState;
            m_OnEnterMethod = onEnterMethod;
            m_OnExitMethod = onExitMethod;
            foreach (var state in states)
            {
                var key = keyGetter(state);
                m_Lookup.Add(key, state);
            }
        }
        
        /// <summary>
        /// Sets the active state from the given state key.
        /// </summary>
        /// <param name="newState">The state to make active.</param>
        public void SetState(TStateKey newState)
        {
            if (newState.Equals(m_CurrentStateKey))
                return;

            var oldStateKey = m_CurrentStateKey;
            if (isCurrentStateValid)
                m_OnExitMethod(current, newState);
            m_CurrentStateKey = newState;
            if (isCurrentStateValid)
                m_OnEnterMethod(current, oldStateKey);
        }
    }
}
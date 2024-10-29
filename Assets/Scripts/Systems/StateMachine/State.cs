using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.FSM
{
    public class State<T>
    {
        public string Name => name;
        string name;

        Dictionary<T, State<T>> transitions = new();

        public State(string name)
        {
            this.name = name;
        }

        public event Action<State<T>> OnEnter = delegate { };
        public void Enter(State<T> previousState) => OnEnter(previousState);

        public event Action OnUpdate = delegate { };
        public void Update() => OnUpdate();

        public event Action OnFixedUpdate = delegate { };
        public void FixedUpdate() => OnFixedUpdate();

        public event Action<T> OnExit = delegate { };
        public void Exit(T nextInput) => OnExit(nextInput);

        /// <summary>
        ///  If this state can transition to state of type T, returns true. Otherwise, returns false
        /// </summary>
        /// <param name="input"> The type of the desired state. </param>
        /// <param name="next"> The next state, if the transition is valid. </param>
        /// <returns></returns>
        public bool TryToTransitionTo(T input, Dictionary<T, State<T>> alwaysAccepting, out State<T> next)
        {
            State<T> result;

            if (transitions.TryGetValue(input, out result) || alwaysAccepting.TryGetValue(input, out result))
            {
                next = result;
                return true;
            }

            next = this;
            return false;
        }

        public State<T> AddTransition(T input, State<T> target)
        {
            transitions.Add(input, target);
            return this;
        }

        public State<T> OverrideTransitions(Dictionary<T, State<T>> newTransitions)
        {
            transitions = newTransitions;
            return this;
        }
    }
}

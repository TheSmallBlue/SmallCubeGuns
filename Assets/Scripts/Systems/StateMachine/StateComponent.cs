using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CubeGuns.FSM
{
    public abstract class StateComponent<T> : MonoBehaviour
    {
        [SerializeField] StateComponentTransition[] Transitions;

        public FiniteStateMachine<T> SourceFSM => sourceFSM;
        FiniteStateMachine<T> sourceFSM;

        public State<T> State { get => state; }
        State<T> state;

        public virtual void BuildState(FiniteStateMachine<T> Source)
        {
            sourceFSM = Source;

            state = new(GetType().ToString());
            state.OnEnter += OnStateEnter;
            state.OnUpdate += OnStateUpdate;
            state.OnFixedUpdate += OnStateFixedUpdate;
            state.OnExit += OnStateExit;
        }

        public void BuildTransitions()
        {
            if (State == null) throw new System.Exception("Trying to build transitions before building the state is not allowed!");

            State.OverrideTransitions(SetTransitions().ToDictionary(x => x.Key, x => x.Value.State));
        }

        public virtual void OnStateEnter(State<T> previousState) { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateFixedUpdate() { }
        public virtual void OnStateExit(T nextInput) { }

        public virtual Dictionary<T, StateComponent<T>> SetTransitions()
        {
            return Transitions.ToDictionary(x => x.Input, x => x.State);
        }

        [System.Serializable]
        public struct StateComponentTransition
        {
            public T Input;
            public StateComponent<T> State;
        }
    }
}

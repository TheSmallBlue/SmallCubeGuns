using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine<T>
{
    public State<T> Current => current;
    State<T> current;

    Dictionary<T, State<T>> acceptAllTransitions = new();

    public FiniteStateMachine(State<T> initial)
    {
        current = initial;
        current.Enter();
    }

    public FiniteStateMachine(StateComponent<T> initial)
    {
        var statesInGameObject = initial.GetComponents<StateComponent<T>>();

        // Create all states
        foreach (var stateComponent in statesInGameObject)
        {
            stateComponent.BuildState(this);
        }

        // Now that the states are created, create their transitions
        foreach (var stateComponent in statesInGameObject)
        {
            stateComponent.BuildTransitions();
        }

        current = initial.State;
        current.Enter();
    }

    public bool ChangeState(T desiredState)
    {
        if(current.TryToTransitionTo(desiredState, acceptAllTransitions, out State<T> nextState))
        {
            current.Exit(desiredState);
            current = nextState;
            current.Enter();

            return true;
        }

        return false;
    }

    public State<T> AcceptAnyTransitionsTo(T input, State<T> target)
    {
        acceptAllTransitions.Add(input, target);
        return target;
    }

    public State<T> AcceptAnyTransitionsTo(T input, StateComponent<T> targetComponent)
    {
        var target = targetComponent.State;
        acceptAllTransitions.Add(input, target);
        return target;
    }

    public void Update()
    {
        current.Update();
    }

    public void FixedUpdate()
    {
        current.FixedUpdate();
    }
}

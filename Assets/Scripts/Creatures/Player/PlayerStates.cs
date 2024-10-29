using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using CubeGuns.FSM;

public class PlayerStates : MonoBehaviour
{
    public FiniteStateMachine<StateType> FSM { get; private set; }

    [SerializeField] PlayerState initialState;

    private void Awake() 
    {
        FSM = new FiniteStateMachine<StateType>(initialState);
    }

    public void ChangeState(StateType stateType)
    {
        FSM.ChangeState(stateType);
    }

    private void Update() 
    {
        FSM.Update();
    }

    private void FixedUpdate() 
    {
        FSM.FixedUpdate();
    }

    private void OnGUI() 
    {
        GUI.Label(new Rect(10, 10, 100, 20), FSM.Current.Name);
    }

    public enum StateType
    {
        Grounded,
        Jumping,
        Falling
    }
}

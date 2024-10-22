using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract class PlayerState : StateComponent<PlayerStates.StateType>
{
    public Player Source => source;
    Player source;

    public override void BuildState(FiniteStateMachine<PlayerStates.StateType> Source)
    {
        base.BuildState(Source);

        this.source = GetComponent<Player>();
    }
}

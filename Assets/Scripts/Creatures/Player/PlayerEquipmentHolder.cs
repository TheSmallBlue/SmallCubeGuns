using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerEquipmentHolder : EquipmentHolder
{
    protected override void Awake()
    {
        base.Awake();

        var input = GetComponent<Player>().Input;
        input.SubscribeToButton("Fire", UseObject);
        input.SubscribeToButton("Drop", Drop);
    }
}

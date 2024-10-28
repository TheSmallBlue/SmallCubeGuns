using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    /// <summary>
    /// Called when the player uses the interact key while looking at this object.
    /// </summary>
    /// <param name="source"> The source player. </param>
    public void Interact(Player source);
}

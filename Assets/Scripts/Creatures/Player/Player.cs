using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Dictionary<Type, IPlayerComponent> playerComponents { get; private set; } = new Dictionary<Type, IPlayerComponent>();

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogWarning("There is more than one player! Deleting youngest one...");
            Destroy(Instance.gameObject);
            return;
        } else Instance = this;

        foreach (var playerComponent in GetComponents<IPlayerComponent>())
        {
            playerComponents.Add(playerComponent.GetType(), playerComponent);
        }
    }

    /// <summary>
    /// Returns a component of type T that exists on the same gameobject as this player
    /// </summary>
    /// <typeparam name="T">The type of the PlayerComponent, must implement IPlayerComponent</typeparam>
    /// <returns></returns>
    public T GetPlayerComponent<T>() where T : IPlayerComponent
    {
        if(!playerComponents.ContainsKey(typeof(T))) return default;
        
        return (T)playerComponents[typeof(T)];
    }
}

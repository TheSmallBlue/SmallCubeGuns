using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float pickupRaylength = 2f;

    public Player Player { get; private set; }

    private void Awake() 
    {
        var input = GetComponent<PlayerInput>();
        input.SubscribeToButton("Use", Interact);

        Player = GetComponent<Player>();
    }

    void Interact()
    {
        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, pickupRaylength)) return;
        if (!hit.transform.root.TryGetComponent(out IPlayerInteractable interactable)) return;

        gameObject.RunEvent<PlayerVisuals>( x => x.OnInteract() );

        interactable.Interact(Player);
    }
}

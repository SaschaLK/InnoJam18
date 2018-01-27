using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractiveComponent))]
public sealed class ItemComponent : MonoBehaviour {

    public PlayerController Holder;

    public Vector3 HoldOffset;
    public Quaternion HoldRotation;

    /// <summary>
    /// Invoked by PlayerController.PickupItem
    /// </summary>
    public UnityEvent OnPickup = new UnityEvent();
    /// <summary>
    /// Invoked by PlayerController.DropItem
    /// </summary>
    public UnityEvent OnDrop = new UnityEvent();
    /// <summary>
    /// Invoked by PlayerController.UseItem
    /// </summary>
    public UnityEvent OnUse = new UnityEvent();
    /// <summary>
    /// Invoked by PlayerController.UseItemWith
    /// </summary>
    public ItemUseWithEvent OnUseWith = new ItemUseWithEvent();

    public Func<PlayerController, bool> CanUse;
    public Func<PlayerController, InteractiveComponent, bool> CanUseWith;

    public InteractiveComponent interactive { get; private set; }
    private void Awake() {
        interactive = GetComponent<InteractiveComponent>();
    }

}

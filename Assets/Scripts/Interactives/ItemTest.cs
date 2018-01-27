using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : ItemBase {

    protected override void Awake() {
        base.Awake();

    }

    public override void Pickup() {
        Debug.Log("Pickup " + name);
    }

    public override void Drop() {
        Debug.Log("Drop " + name);
    }

    public override void Use() {
        Debug.Log("Use " + name);
    }

    public override void UseWith(InteractiveComponent with) {
        Debug.Log("Use " + name + " with " + with.name);
    }

    public override bool CanPickup(PlayerController player) {
        return true; // Can always be picked up.
    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return with.name == "InteractiveTest Top";
    }

}

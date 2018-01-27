using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHammer : ItemBase {

    protected override void Awake() {
        base.Awake();
    }

    public override void UseWith(InteractiveComponent with) {

    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return true;
    }

}

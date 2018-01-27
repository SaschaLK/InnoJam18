using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProjectile : ItemBase_Or {

    protected override void Awake() {
        base.Awake();
    }

    public override void OnUseWith(InteractiveComponent with) {
        InteractiveCannon cannon = with.GetComponent<InteractiveCannon>();
        if (cannon == null)
            return;
        cannon.Container.PickupItem(item);
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return with.GetComponent<InteractiveCannon>() != null;
    }

}

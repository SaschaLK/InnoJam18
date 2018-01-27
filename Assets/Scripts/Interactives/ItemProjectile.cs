using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProjectile : ItemHandler {

    protected override void Awake() {
        base.Awake();

        __CanUseWith = item.CanUseWith;
        item.CanUseWith = _CanUseWith;
    }

    public override void OnUseWith(InteractiveComponent with) {
        InteractiveCannon cannon = with.GetComponent<InteractiveCannon>();
        if (cannon == null)
            return;
        cannon.PickupItem(item);
    }

    // We need our custom CanUseWith combiner: || instead of &&
    private Func<PlayerController, InteractiveComponent, bool> __CanUseWith;
    private bool _CanUseWith(PlayerController player, InteractiveComponent with) { return CanUseWith(player, with) || (__CanUseWith != null ? __CanUseWith(player, with) : false); }
    public bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return with.GetComponent<InteractiveCannon>() != null;
    }

}

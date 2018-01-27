using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemComponent))]
public class ItemBase : ItemHandlerBase {

    protected override void Awake() {
        base.Awake();

        item.interactive.CanUse = _CanPickup;
        item.CanUse = _CanUse;
        item.CanUseWith = _CanUseWith;
    }

    private bool _CanPickup(PlayerController player) { return CanPickup(player); }
    public virtual bool CanPickup(PlayerController player) {
        return true;
    }

    private bool _CanUse(PlayerController player) { return CanUse(player); }
    public virtual bool CanUse(PlayerController player) {
        return true;
    }

    private bool _CanUseWith(PlayerController player, InteractiveComponent with) { return CanUseWith(player, with); }
    public virtual bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return true;
    }

}

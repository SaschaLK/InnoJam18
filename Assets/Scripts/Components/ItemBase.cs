using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemComponent))]
public class ItemBase : ItemHandler {

    protected override void Awake() {
        base.Awake();

        __CanPickup = item.interactive.CanInteract;
        item.interactive.CanInteract = _CanPickup;
        __CanUse = item.CanUse;
        item.CanUse = _CanUse;
        __CanUseWith = item.CanUseWith;
        item.CanUseWith = _CanUseWith;
    }

    private Func<PlayerController, bool> __CanPickup;
    private bool _CanPickup(PlayerController player) { return CanPickup(player) && (__CanPickup != null ? __CanPickup(player) : true); }
    public virtual bool CanPickup(PlayerController player) {
        return true;
    }

    private Func<PlayerController, bool> __CanUse;
    private bool _CanUse(PlayerController player) { return CanUse(player) &&( __CanUse != null ? __CanUse(player) : true); }
    public virtual bool CanUse(PlayerController player) {
        return true;
    }

    private Func<PlayerController, InteractiveComponent, bool> __CanUseWith;
    private bool _CanUseWith(PlayerController player, InteractiveComponent with) { return CanUseWith(player, with) && (__CanUseWith != null ? __CanUseWith(player, with) : true); }
    public virtual bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return true;
    }

}

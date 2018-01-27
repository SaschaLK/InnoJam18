using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBase : InteractiveHandler {

    protected override void Awake() {
        base.Awake();
        interactive.CanUse = _CanUse;
    }

    private bool _CanUse(PlayerController player) { return CanUse(player); }
    public virtual bool CanUse(PlayerController player) {
        return true;
    }

}

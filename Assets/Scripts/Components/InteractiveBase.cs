using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBase : InteractiveHandler {

    protected override void Awake() {
        base.Awake();
        __CanInteract = interactive.CanInteract;
        interactive.CanInteract = _CanInteract;
    }

    private Func<PlayerController, bool> __CanInteract;
    private bool _CanInteract(PlayerController player) { return CanInteract(player) && (__CanInteract != null ? __CanInteract(player) : true); }
    public virtual bool CanInteract(PlayerController player) {
        return true;
    }

}

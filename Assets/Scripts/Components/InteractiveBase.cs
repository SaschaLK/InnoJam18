using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBase : InteractiveHandler {

    protected override void Awake() {
        base.Awake();
        __CanInteract = interactive.CanInteract;
        interactive.CanInteract = _CanInteract;

        __CanMinigame = interactive.CanMinigame;
        interactive.CanMinigame = _CanMinigame;
    }

    private Func<PlayerController, bool> __CanInteract;
    private bool _CanInteract(PlayerController player) { return CanInteract(player) && (__CanInteract != null ? __CanInteract(player) : true); }
    public virtual bool CanInteract(PlayerController player) {
        return true;
    }

    private Func<PlayerController, bool> __CanMinigame;
    private bool _CanMinigame(PlayerController player) { return CanMinigame(player) && (__CanMinigame != null ? __CanMinigame(player) : true); }
    public virtual bool CanMinigame(PlayerController player) {
        return true;
    }

}

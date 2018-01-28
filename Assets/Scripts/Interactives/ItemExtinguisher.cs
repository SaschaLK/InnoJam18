using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtinguisher : ItemBase {

    protected ItemStateRemover remover;

    protected override void Awake() {
        base.Awake();

        remover = GetComponent<ItemStateRemover>();
    }

    public override void OnUseWith(InteractiveComponent with) {

    }

    public override bool CanUse(PlayerController player) {
        return true; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return true;
    }

    public override bool CanMinigame(PlayerController player, InteractiveComponent with) {
        if (with == null)
            return false;

        return remover.CanUseWith(player, with);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExtinguisher : ItemBase {

    public float DepletePerUsage = 10f;

    protected override void Awake() {
        base.Awake();
    }

    public override void OnUseWith(InteractiveComponent with) {
        Destroy(with.GetComponent<FireState>());
        item.interactive.InflictDamage(DepletePerUsage);
    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        FireState fire = with.GetComponent<FireState>();
        return fire != null && item.interactive.health >= DepletePerUsage;
    }

}

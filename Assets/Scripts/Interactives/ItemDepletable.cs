using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDepletable : ItemBase {

    public float DepletePerUsage = 25f;

    protected override void Awake() {
        base.Awake();
    }

    public override void OnUseWith(InteractiveComponent with) {
        item.interactive.InflictDamage(DepletePerUsage);
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return item.interactive.health >= DepletePerUsage;
    }

}

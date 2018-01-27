using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOneUse : ItemHandler {

    protected override void Awake() {
        base.Awake();
    }

    public override void OnUse() {
        item.Holder.DropItem();
        Destroy(gameObject);
    }

    public override void OnUseWith(InteractiveComponent with) {
        item.Holder.DropItem();
        Destroy(gameObject);
    }

}

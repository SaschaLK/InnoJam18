using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOneUse : ItemHandlerBase {

    protected override void Awake() {
        base.Awake();
    }

    public override void Use() {
        item.Holder.DropItem();
        Destroy(gameObject);
    }

    public override void UseWith(InteractiveComponent with) {
        item.Holder.DropItem();
        Destroy(gameObject);
    }

}

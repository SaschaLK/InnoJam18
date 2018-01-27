using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStick : ItemBase {

    protected override void Awake() {
        base.Awake();

    }

    public override void Use() {
        Debug.Log("STICK DOES THINGS");
    }

    public override void UseWith(InteractiveComponent with) {
    }

}

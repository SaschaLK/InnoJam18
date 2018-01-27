using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrystal : ItemBase {

    protected override void Awake() {
        base.Awake();
    }

    public override void Use() {
        Debug.Log("CRYSTAL DOES THINGS");
    }

    public override void UseWith(InteractiveComponent with) {
    }

}

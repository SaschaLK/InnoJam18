using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateRemover : ItemBase_Or {

    public string Type;
    private System.Type _Type;

    protected override void Awake() {
        base.Awake();
        _Type = System.Type.GetType(Type);
    }

    public override void OnUseWith(InteractiveComponent with) {
        Component target = with.GetComponentInChildren(_Type);
        if (target == null)
            return;
        Destroy(target.gameObject);
    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return with.GetComponentInChildren(_Type) != null;
    }

}

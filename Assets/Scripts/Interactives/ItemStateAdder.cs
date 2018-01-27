using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateAdder : ItemBase_Or {

    public string RequiredType;
    private System.Type _Type;
    public string Path;

    protected override void Awake() {
        base.Awake();
        _Type = System.Type.GetType(RequiredType);
    }

    public override void OnUseWith(InteractiveComponent with) {
        Component target = with.GetComponentInChildren(_Type);
        if (target == null)
            return;
        Instantiate(Resources.Load<GameObject>(Path), with.transform.position, Quaternion.identity, with.transform);
    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        return with.GetComponentInChildren(_Type) != null;
    }

}

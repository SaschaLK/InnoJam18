using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBucket : ItemBase {

    protected string _DisplayName;

    [SerializeField]
    protected BucketState _State;
    public BucketState State {
        get {
            return _State;
        }
        set {
            if (_State == value)
                return;

            if (value == BucketState.Empty) {
                item.interactive.DisplayName = _DisplayName;
            } else {
                item.interactive.DisplayName = _DisplayName + " + " + value;
            }

            _State = value;
        }
    }

    protected override void Awake() {
        base.Awake();
        _DisplayName = item.interactive.DisplayName;
    }

    public override void OnUseWith(InteractiveComponent with) {
        with.GetComponent<InteractiveBucketTarget>().HandleBucket(this);
    }

    public override bool CanUse(PlayerController player) {
        return false; // Can only be used with something else.
    }

    public override bool CanUseWith(PlayerController player, InteractiveComponent with) {
        InteractiveBucketTarget target = with.GetComponent<InteractiveBucketTarget>();
        if (target == null)
            return false;
        return target.StateInput == BucketState.Any || State == target.StateInput;
    }

}

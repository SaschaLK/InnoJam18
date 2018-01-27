using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBucketTarget : InteractiveBase {

    public BucketState StateInput;
    public BucketState StateOutput;

    public InteractiveItemContainer Container;

    public BucketEvent OnBucket;

    protected override void Awake() {
        base.Awake();

        if (Container == null)
            Container = GetComponent<InteractiveItemContainer>();
        if (Container == null)
            Container = gameObject.AddComponent<InteractiveItemContainer>();

        Container.IsValid = (ci, item) => IsValid(item.GetComponent<ItemBucket>());

    }

    public bool IsValid(ItemBucket bucket) {
        return bucket != null && (bucket.State == StateInput || (StateInput == BucketState.NotEmpty && bucket.State != BucketState.Empty));
    }

    public void Update() {
    }

    public override bool CanMinigame(PlayerController player, InteractiveComponent with) {
        if (Container.Count != 0)
            return false;

        return true;
    }

    public void HandleBucket(ItemBucket bucket) {
        if (!IsValid(bucket))
            return;
        bucket.State = StateOutput;
        OnBucket.Invoke(bucket);
    }

}

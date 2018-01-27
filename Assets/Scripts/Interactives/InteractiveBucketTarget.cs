using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBucketTarget : InteractiveBase {

    public BucketState StateInput;
    public BucketState StateOutput;

    public Transform Container;

    public ItemBucket Bucket;

    public float UsageRadius = 3f;

    public BucketEvent OnBucket;

    protected override void Awake() {
        base.Awake();

        if (Container == null)
            Container = transform.Find("Container");
	}

    public void Update() {
        if (Bucket == null) {
            Vector3 pos = transform.position;

            ItemBucket[] buckets = FindObjectsOfType<ItemBucket>();
            float closestDist = UsageRadius * UsageRadius;
            ItemBucket closest = null;
            for (int i = 0; i < buckets.Length; i++) {
                ItemBucket bucket = buckets[i];
                if (bucket == null || (StateInput != BucketState.Any && bucket.State != StateInput) || bucket.transform.parent != null)
                    continue;

                float dist = (pos - bucket.transform.position).sqrMagnitude;
                if (dist > closestDist) {
                    continue;
                }

                closestDist = dist;
                closest = bucket;
            }

            PickupItem(closest);
        }

        if (Bucket == null)
            return;
        if (Bucket.item.Holder != null) {
            Bucket = null;
            return;
        }
    }

    public void HandleBucket(ItemBucket bucket) {
        if (StateInput != BucketState.Any && bucket.State != StateInput)
            return;
        bucket.State = StateOutput;
        OnBucket.Invoke(bucket);
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up contained object instead.
        if (Bucket != null)
            player.PickupItem(Bucket.item);
    }

    public override bool CanInteract(PlayerController player) {
        if (Bucket != null)
            return true;

        if (player.Item == null)
            return false;
        ItemBucket bucket = player.Item.GetComponent<ItemBucket>();
        if (bucket == null || (StateInput != BucketState.Any && bucket.State != StateInput))
            return false;
        return true;
    }

    public void PickupItem(ItemBucket bucket) {
        if (bucket == null)
            return;

        DropItem(); // Drop any previous items.

        Bucket = bucket;

        if (bucket.item.Holder != null)
            bucket.item.Holder.DropItem();

        bucket.transform.parent = Container;
        bucket.transform.localPosition = bucket.item.HoldOffset;
        bucket.transform.localRotation = bucket.item.HoldRotation;

        Rigidbody body = bucket.GetComponent<Rigidbody>();
        if (body != null)
            Destroy(body);
        Collider collider = bucket.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        HandleBucket(bucket);
    }

    public void DropItem() {
        if (Bucket == null)
            return;

        Bucket.transform.parent = null;

        Bucket.gameObject.AddComponent<Rigidbody>();
        Collider collider = Bucket.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Bucket = null;
    }

}

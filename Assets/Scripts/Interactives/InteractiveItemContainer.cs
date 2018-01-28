using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractiveItemContainer : InteractiveBase {

    public List<Transform> Containers = new List<Transform>();
    public Dictionary<int, ItemComponent> Items = new Dictionary<int, ItemComponent>();

    public float UsageRadius = 3f;

    public System.Func<int, ItemComponent, bool> IsValid;
    public ItemContainerEvent OnPickup = new ItemContainerEvent();
    public ItemContainerEvent OnDrop = new ItemContainerEvent();

    public int Count {
        get {
            int count = 0;
            for (int ci = Containers.Count - 1; ci > -1; --ci) {
                ItemComponent item;
                if (!Items.TryGetValue(ci, out item) || item == null)
                    continue;
                count++;
            }
            return count;
        }
    }

    protected override void Awake() {
        base.Awake();

        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith("Container"))
                Containers.Add(child);
        }

	}

    public void Update() {
        if (IsValid != null) {
            for (int ci = 0; ci < Containers.Count; ci++) {
                ItemComponent existing;
                if (Items.TryGetValue(ci, out existing) && existing != null)
                    continue;

                Vector3 pos = transform.position;

                Collider[] items = Physics.OverlapSphere(pos, UsageRadius);
                float closestDist = UsageRadius * UsageRadius;
                ItemComponent closest = null;
                for (int i = 0; i < items.Length; i++) {
                    ItemComponent item = items[i].GetComponent<ItemComponent>();
                    if (item == null)
                        item = items[i].GetComponentInParent<ItemComponent>();
                    if (item == null || item.transform.parent != null || !IsValid(ci, item))
                        continue;

                    float dist = (pos - item.transform.position).sqrMagnitude;
                    if (dist > closestDist) {
                        continue;
                    }

                    closestDist = dist;
                    closest = item;
                }

                PickupItem(ci, closest);
            }
        }

        for (int ci = 0; ci < Containers.Count; ci++) {
            ItemComponent item;
            if (!Items.TryGetValue(ci, out item) || item == null)
                continue;
            if (item.transform.parent != Containers[ci])
                Items[ci] = null;
        }
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up charging object instead.
        for (int ci = 0; ci < Containers.Count; ci++) {
            ItemComponent item;
            if (!Items.TryGetValue(ci, out item) || item == null)
                continue;

            player.Interact(item.interactive);
        }
    }

    public override bool CanInteract(PlayerController player) {
        // Following checks only pass if a valid item is being held.
        if (player.Item != null) {
            for (int ci = 0; ci < Containers.Count; ci++) {
                if (IsValid(ci, player.Item))
                    return true;
            }
            return false;
        }

        return Count != 0; // Allow player taking stored items.
    }

    public void PickupItem(ItemComponent item) {
        if (item == null)
            return;
        for (int ci = 0; ci < Containers.Count; ci++) {
            if (!IsValid(ci, item))
                continue;
            CmdPickupItem(this.gameObject, ci, item.gameObject);
            return;
        }
    }

    public void PickupItem(int ci, ItemComponent item) {
        if (item == null || !IsValid(ci, item))
            return;
        CmdPickupItem(this.gameObject, ci, item.gameObject);
    }

    [Command]
    public void CmdPickupItem(GameObject station, int ci, GameObject interac)
    {
        RpcPickupItem(ci, interac);
    }

    [ClientRpc]
    public void RpcPickupItem(int ci, GameObject other)
    {
        ItemComponent item = other.GetComponent<ItemComponent>();
        ItemComponent prevItem;
        if (Items.TryGetValue(ci, out prevItem) && prevItem != null) {
            LocalDropItem(ci); // Drop any previous items.
            // Put the prev item at the replacing item's pos.
            prevItem.transform.position = item.transform.position;
        }

        Items[ci] = item;

        if (item.Holder != null) {
            if (prevItem != null)
                item.Holder.PickupItem(prevItem);
            else
                item.Holder.DropItem();
        }

        item.transform.parent = Containers[ci];
        item.transform.localPosition = item.HoldOffset;
        item.transform.localEulerAngles = item.HoldRotation;

        Collider collider = item.GetComponentInChildren<Collider>();
        if (collider != null) {
            collider.enabled = false;
            Rigidbody body = item.GetComponent<Rigidbody>();
            if (body != null)
                Destroy(body);
        }

        OnPickup.Invoke(ci, item);
    }

    public void DropItem(int ci) {
        CmdDropItem(this.gameObject, ci);
    }

    [Command]
    public void CmdDropItem(GameObject station, int ci)
    {
        RpcDropItem(ci);
    }

    [ClientRpc]
    public void RpcDropItem(int ci)
    {
        LocalDropItem(ci);
    }

    public void LocalDropItem(int ci) {
        ItemComponent item;
        if (!Items.TryGetValue(ci, out item) || item == null)
            return;

        OnDrop.Invoke(ci, item);

        item.transform.parent = null;

        Collider collider = item.GetComponentInChildren<Collider>();
        if (collider != null) {
            collider.enabled = true;
            item.gameObject.AddComponent<Rigidbody>();
        }

        Items[ci] = null;
    }

}

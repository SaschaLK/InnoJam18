using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCannon : InteractiveBase {

    public Transform ContainerProjectile;
    public Transform ContainerCrystal;

    public ItemComponent Projectile;
    public ItemCrystal Crystal;

    public float UsageRadius = 3f;

    protected override void Awake() {
        base.Awake();

        if (ContainerProjectile == null)
            ContainerProjectile = transform.Find("ContainerProjectile");
        if (ContainerCrystal == null)
            ContainerCrystal = transform.Find("ContainerCrystal");
    }

    public void Update() {
        Vector3 pos = transform.position;

        if (Crystal == null) {
            ItemCrystal[] crystals = FindObjectsOfType<ItemCrystal>();
            float closestDist = UsageRadius * UsageRadius;
            ItemCrystal closest = null;
            for (int i = 0; i < crystals.Length; i++) {
                ItemCrystal crystal = crystals[i];
                if (crystal == null || crystal.item.Holder != null)
                    continue;

                float dist = (pos - crystal.transform.position).sqrMagnitude;
                if (dist > closestDist) {
                    continue;
                }

                closestDist = dist;
                closest = crystal;
            }

            PickupCrystal(closest);
        }

        if (Crystal != null && Crystal.item.Holder != null)
            Crystal = null;

        if (Projectile == null) {
            ItemComponent[] items = FindObjectsOfType<ItemComponent>();
            float closestDist = UsageRadius * UsageRadius;
            ItemComponent closest = null;
            for (int i = 0; i < items.Length; i++) {
                ItemComponent item = items[i];
                if (item == null || item.Holder != null || item.GetComponent<ItemCrystal>() != null)
                    continue;

                float dist = (pos - item.transform.position).sqrMagnitude;
                if (dist > closestDist) {
                    continue;
                }

                closestDist = dist;
                closest = item;
            }

            PickupProjectile(closest);
        }

        if (Projectile != null && Projectile.Holder != null)
            Projectile = null;
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up contained objects instead.
        if (Projectile != null)
            player.PickupItem(Projectile);
        else if (Crystal != null)
            player.PickupItem(Crystal.item);
    }

    public override bool CanInteract(PlayerController player) {
        if (Projectile != null)
            return true;

        if (player.Item == null)
            return false;
        return true;
    }

    public void PickupItem(ItemComponent item) {
        ItemCrystal crystal = item.GetComponent<ItemCrystal>();
        if (crystal != null)
            PickupCrystal(crystal);
        else
            PickupProjectile(item);
    }

    public void PickupCrystal(ItemCrystal crystal) {
        if (crystal == null)
            return;

        DropCrystal(); // Drop any previous items.

        Crystal = crystal;

        if (crystal.item.Holder != null)
            crystal.item.Holder.DropItem();

        crystal.transform.parent = ContainerCrystal;
        crystal.transform.localPosition = crystal.item.HoldOffset;
        crystal.transform.localRotation = crystal.item.HoldRotation;

        Rigidbody body = crystal.GetComponent<Rigidbody>();
        if (body != null)
            Destroy(body);
        Collider collider = crystal.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
    }

    public void DropCrystal() {
        if (Crystal == null)
            return;

        Crystal.transform.parent = null;

        Crystal.gameObject.AddComponent<Rigidbody>();
        Collider collider = Crystal.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Crystal = null;
    }

    public void PickupProjectile(ItemComponent projectile) {
        if (projectile == null)
            return;

        DropProjectile(); // Drop any previous items.

        Projectile = projectile;

        if (projectile.Holder != null)
            projectile.Holder.DropItem();

        projectile.transform.parent = ContainerProjectile;
        projectile.transform.localPosition = projectile.HoldOffset;
        projectile.transform.localRotation = projectile.HoldRotation;

        Rigidbody body = projectile.GetComponent<Rigidbody>();
        if (body != null)
            Destroy(body);
        Collider collider = projectile.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
    }

    public void DropProjectile() {
        if (Projectile == null)
            return;

        Projectile.transform.parent = null;

        Projectile.gameObject.AddComponent<Rigidbody>();
        Collider collider = Projectile.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Projectile = null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCharger : InteractiveBase {

    public string ChargingName;
    public float ChargePerSecond = 3f;

    public Transform Container;

    public ItemChargeable Charging;

	protected override void Awake() {
        base.Awake();

        if (Container == null)
            Container = transform.Find("Container");
	}

    public void Update() {
        if (Charging == null)
            return;
        if (Charging.item.Holder != null) {
            Charging = null;
            return;
        }

        if (Charging.item.interactive.health < 0f)
            Charging.item.interactive.health = 0f;
        Charging.item.interactive.health += ChargePerSecond * Time.deltaTime;
        if (Charging.item.interactive.health > 100f)
            Charging.item.interactive.health = 100f;
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up charging object instead.
        if (Charging != null)
            player.PickupItem(Charging.item);
    }

    public override bool CanUse(PlayerController player) {
        if (Charging != null)
            return true;

        if (player.Item == null)
            return false;
        ItemChargeable charging = player.Item.GetComponent<ItemChargeable>();
        if (charging == null || !ChargingName.Contains(charging.name))
            return false;
        return true;
    }

    public void PickupItem(ItemChargeable charging) {
        if (charging == null || !ChargingName.Contains(charging.name))
            return;

        DropItem(); // Drop any previous items.

        Charging = charging;

        if (charging.item.Holder != null)
            charging.item.Holder.DropItem();

        charging.transform.parent = Container;
        charging.transform.localPosition = charging.item.HoldOffset;
        charging.transform.localRotation = charging.item.HoldRotation;

        Rigidbody body = charging.GetComponent<Rigidbody>();
        if (body != null)
            Destroy(body);
        Collider collider = charging.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
    }

    public void DropItem() {
        if (Charging == null)
            return;

        Charging.transform.parent = null;

        Charging.gameObject.AddComponent<Rigidbody>();
        Collider collider = Charging.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Charging = null;
    }

}

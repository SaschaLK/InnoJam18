using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractiveCharger : InteractiveBase
{

    public string ChargingName;
    public float ChargePerSecond = 3f;

    public Transform Container;

    public ItemChargeable Charging;

    public float UsageRadius = 3f;

    protected override void Awake() {
        base.Awake();

        if (Container == null)
            Container = transform.Find("Container");
	}

    public void Update() {
        // only run this update on the server, with authority.
        // health is synchronized via a SyncVar
        if (!isServer) return;

        if (Charging == null) {
            Vector3 pos = transform.position;

            // ItemChargeable[] chargeables = FindObjectsOfType<ItemChargeable>();
            Collider[] chargeables = Physics.OverlapSphere(pos, UsageRadius);
            float closestDist = UsageRadius * UsageRadius;
            ItemChargeable closest = null;
            // check all chargeables if they are valid and in reach
            for (int i = 0; i < chargeables.Length; i++) {
                ItemChargeable chargeable = chargeables[i].GetComponent<ItemChargeable>();
                if (chargeable == null || !ChargingName.Contains(chargeable.name) || chargeable.transform.parent != null)
                    continue;

                float dist = (pos - chargeable.transform.position).sqrMagnitude;
                if (dist > closestDist) {
                    continue;
                }

                closestDist = dist;
                closest = chargeable;
            }

            PickupItem(closest);
        }

        if (Charging == null)
            return;
        if (Charging.item.Holder != null) {
            Charging = null;
            return;
        }

        Charging.ChargeUp(ChargePerSecond * Time.deltaTime);
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up charging object instead.
        if (Charging != null)
            CmdPlayerInteract(player.gameObject, Charging.item.gameObject);
    }

    [Command]
    public void CmdPlayerInteract(GameObject player, GameObject interac)
    {
        player.GetComponent<PlayerController>().RpcPickupItem(interac);
    }

    public override bool CanInteract(PlayerController player) {
        if (Charging != null)
            return true;

        if (player.Item == null)
            return false;

        ItemChargeable charging = player.Item.GetComponent<ItemChargeable>();
        if (charging == null || !ChargingName.Contains(charging.name))
            return false;
        return true;
    }

    public override bool CanMinigame(PlayerController player, InteractiveComponent with) {
        if (Charging != null)
            return false;

        return true;
    }

    public void PickupItem(ItemChargeable charging) {
        if (charging == null || !ChargingName.Contains(charging.name))
            return;
        CmdPickupItem(this.gameObject, charging.gameObject);
    }

    [Command]
    public void CmdPickupItem(GameObject station, GameObject interac)
    {
        station.GetComponent<InteractiveCharger>().RpcPickupItem(interac);
    }

    [ClientRpc]
    public void RpcPickupItem(GameObject other)
    {
        ItemChargeable charging = other.GetComponent<ItemChargeable>();
        ItemChargeable prevItem = Charging;
        DropItem(); // Drop any previous items.
        if (prevItem != null)
            // Put the prev item at the replacing item's pos.
            prevItem.transform.position = charging.transform.position;

        Charging = charging;

        if (charging.item.Holder != null) {
            if (prevItem != null)
                charging.item.Holder.PickupItem(prevItem.item);
            else
                charging.item.Holder.DropItem();
        }

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

        CmdDropItem(this.gameObject);
    }

    [Command]
    public void CmdDropItem(GameObject station)
    {
        station.GetComponent<InteractiveCharger>().RpcDropItem();
    }

    [ClientRpc]
    public void RpcDropItem()
    {
        LocalDropItem();
    }

    public void LocalDropItem() { 
        Charging.transform.parent = null;

        Charging.gameObject.AddComponent<Rigidbody>();
        Collider collider = Charging.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Charging = null;
    }

}

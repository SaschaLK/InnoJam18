using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractiveCharger : InteractiveBase
{

    protected string _DisplayName;

    public string ChargingName;
    public float ChargePerSecond = 3f;

    public InteractiveItemContainer Container;

    protected ItemChargeable Charging;

    protected override void Awake() {
        base.Awake();
        _DisplayName = interactive.DisplayName;

        if (Container == null)
            Container = GetComponent<InteractiveItemContainer>();
        if (Container == null)
            Container = gameObject.AddComponent<InteractiveItemContainer>();

        Container.IsValid = (ci, item) => IsValid(item.GetComponent<ItemChargeable>());
        Container.OnPickup.AddListener((ci, item) => Charging = item.GetComponent<ItemChargeable>());
        Container.OnDrop.AddListener((ci, item) => Charging = null);
    }

    public bool IsValid(ItemChargeable chargeable) {
        return chargeable != null && ChargingName.Contains(chargeable.name);
    }

    public void Update() {
        if (Charging != null) {
            interactive.DisplayName = _DisplayName + "\n(" + Charging.item.interactive.Health.ToString("N2").Replace(',', '.') + " %)";
        } else {
            interactive.DisplayName = _DisplayName;
        }

        // only run this update on the server, with authority.
        // health is synchronized via a SyncVar
        if (!isServer) return;

        if (Charging == null)
            return;

        Charging.ChargeUp(ChargePerSecond * Time.deltaTime);
    }

    public override bool CanMinigame(PlayerController player, InteractiveComponent with) {
        if (Charging != null)
            return false;

        return true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCannon : InteractiveBase {

    public InteractiveItemContainer Container;

    public ItemComponent Projectile;
    public ItemCrystal Crystal;

    public float UsageRadius = 3f;

    protected override void Awake() {
        base.Awake();

        if (Container == null)
            Container = GetComponent<InteractiveItemContainer>();
        if (Container == null)
            Container = gameObject.AddComponent<InteractiveItemContainer>();

        Container.IsValid = (ci, item) => (ci == 1) == (item.GetComponent<ItemCrystal>() != null);
        Container.OnPickup.AddListener((ci, item) => {
            if (ci == 0)
                Projectile = item;
            else if (ci == 1)
                Crystal = item.GetComponent<ItemCrystal>();
        });
        Container.OnPickup.AddListener((ci, item) => {
            if (ci == 0)
                Projectile = item;
            else if (ci == 1)
                Crystal = item.GetComponent<ItemCrystal>();
        });
        Container.OnDrop.AddListener((ci, item) => {
            if (ci == 0)
                Projectile = null;
            else if (ci == 1)
                Crystal = null;
        });
    }

    public override void OnInteract(PlayerController player) {
        // Tell player to pick up contained objects instead.
        if (Projectile != null)
            player.PickupItem(Projectile);
        else if (Crystal != null)
            player.PickupItem(Crystal.item);
    }

}

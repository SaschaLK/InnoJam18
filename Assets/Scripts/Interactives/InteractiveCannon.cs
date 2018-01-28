using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractiveCannon : InteractiveBase {

    public InteractiveItemContainer Container;

    public ItemComponent Projectile;
    public ItemCrystal Crystal;

    public float UsageRadius = 3f;

    public float DepleteCrystalPerUsage = 30f;

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

    public EnemyApproach GetEnemyApproach() {
        foreach (GameEvent e in SceneController.instance.currentEvents) {
            if (e != null && e is EnemyApproach)
                return (EnemyApproach) e;
        }
        return null;
    }

    public bool CanShoot() {
        return Projectile != null && Crystal != null && Crystal.item.interactive.HealthLargerThan(DepleteCrystalPerUsage) && GetEnemyApproach() != null;
    }

    public void Shoot() {
        if (!CanShoot())
            return;

        ItemComponent projectile = Projectile;
        CmdShoot(projectile.gameObject);
    }

    [Command]
    public void CmdShoot(GameObject projObj) {
        Crystal.item.interactive.InflictDamage(DepleteCrystalPerUsage);
        RpcShoot(projObj);
    }

    [ClientRpc]
    public void RpcShoot(GameObject projObj) {
        Container.LocalDropItem(0);
        Destroy(projObj);
        GetEnemyApproach().OnSuccess.Invoke();
    }

}

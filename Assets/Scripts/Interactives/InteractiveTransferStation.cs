using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTransferStation : InteractiveBase {

    private Vector3 LeftSpawn;
    private Vector3 LeftVel;

    private Vector3 RightSpawn;
    private Vector3 RightVel;

    protected override void Awake() {
        base.Awake();

        Transform left = transform.Find("Output 1");
        Transform right = transform.Find("Output 2");

        LeftSpawn = left.position;
        LeftVel = (left.position - transform.position).normalized;

        RightSpawn = right.position;
        RightVel = (right.position - transform.position).normalized;
    }

    public override bool CanInteract(PlayerController player) {
        return true;
    }

    public override void OnInteract(PlayerController player) {
        if (player.Item == null)
            return;

        Send(player);
    }

    public void Send(PlayerController player) {
        // Notiz: Das alles sollte schon auf dem Rechner des Clients passieren.

        int numberFrom = player.PlayerNumber;
        int numberTo = (numberFrom % 2) + 1;

        ItemComponent item = player.Item;
        player.LocalDropItem();

        item.transform.SetLayerDeep(LayerMask.NameToLayer("Items " + numberTo));
        item.transform.position = numberTo == 1 ? LeftSpawn : RightSpawn;

        item.GetComponent<Rigidbody>().velocity = (numberTo == 1 ? LeftVel : RightVel) * 32f;
    }

}

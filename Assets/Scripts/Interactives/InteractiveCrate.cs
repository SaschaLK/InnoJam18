using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractiveCrate : InteractiveBase {

    public ItemComponent Prefab;

    public int MaxCount = 1;
        
    private List<ItemComponent> Existing = new List<ItemComponent>();

    protected override void Awake() {
        base.Awake();

        ClientScene.RegisterPrefab(Prefab.gameObject);
    }

    public override bool CanInteract(PlayerController player) {
        if (player.Item != null)
            return false;

        int count = 0;
        for (int i = 0; i < Existing.Count && count < MaxCount; i++) {
            if (Existing[i] != null)
                count++;
        }

        return count < MaxCount;
    }

    public override void OnInteract(PlayerController player) {
        CmdGiveItem(gameObject, player.gameObject);
    }

    [Command]
    public void CmdGiveItem(GameObject crate, GameObject player) {
        ItemComponent item = Instantiate(Prefab);
        NetworkServer.Spawn(item.gameObject);
        crate.GetComponent<InteractiveCrate>().RpcGiveItem(player, item.gameObject);
    }

    [ClientRpc]
    public void RpcGiveItem(GameObject playerObj, GameObject itemObj) {
        PlayerController player = playerObj.GetComponent<PlayerController>();
        ItemComponent item = itemObj.GetComponent<ItemComponent>();
        player.LocalPickupItem(item);

        for (int i = Existing.Count - 1; i > -1; --i) {
            if (Existing[i] == null)
                Existing.RemoveAt(i);
        }
        Existing.Add(item);
    }

}

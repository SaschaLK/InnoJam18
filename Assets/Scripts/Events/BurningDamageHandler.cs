﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BurningDamageHandler : NetworkBehaviour {

    InteractiveComponent interactive;
    GameObject fire;

    private void Awake()
    {
        interactive = GetComponent<InteractiveComponent>();
    }

    public void FireOutbreak()
    {
        if (!isServer) return;
        if (Random.Range(0, 100) <= 20 && fire == null)
            CmdFireOutbreak();
    }

    [Command]
    public void CmdFireOutbreak()
    {
        RpcFireOutbreak();
    }

    [ClientRpc]
    public void RpcFireOutbreak()
    {
        Debug.Log("FIRE!!!");
        fire = Instantiate(Resources.Load<GameObject>("Fire"), this.transform.position, Quaternion.identity, this.transform);
    }
}

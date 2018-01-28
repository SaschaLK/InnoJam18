using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EvadeHandler : NetworkBehaviour {

    private float timeToFail = BalancingConstant.EVADE_TIME; //change this value to control event difficulty

    Airplane airplane;
    private bool success = false;
    public Evade ev;

    public void BindEvent(Evade ev)
    {
        this.ev = ev;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(ev);
        Debug.Log("evade ");

        // TODO: HIER BLITZGRAFIK STARTEN

        success = false;
        if (isServer)
            Invoke("CmdInvokeFailure", timeToFail);
    }

    [Command]
    private void CmdInvokeFailure()
    {
        if (success) return;
         RpcInvolkeFailure();
    }

    [ClientRpc]
    public void RpcInvolkeFailure()
    {
        ev.OnFailed.Invoke();
    }

    private void StopVisuals()
    {
        // TODO: HIER BLITZGRAFIK BEENDEN
        SceneController.instance.currentEvents.Remove(ev);
        SceneController.instance.currentEvents.Remove(ev);
    }

    public void EvadeEventFailed()
    {
        Debug.Log("evade event failed, we take damage");
        StopVisuals();
        GameManager.instance.TakeDamage(BalancingConstant.EVADE_DAMAGE);
    }

    public void EvadeEventSuccess()
    {
        success = true;
        StopVisuals();
    }
}

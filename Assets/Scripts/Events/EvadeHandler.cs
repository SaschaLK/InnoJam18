using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EvadeHandler : NetworkBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;
    private bool success = false;
    public Evade ev;

    public ControlLamp evadeLamp;

    public void BindEvent(Evade ev)
    {
        this.ev = ev;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(ev);
        Debug.Log("evade right");

        evadeLamp.OnActivation.Invoke();
        success = false;
        if (isServer)
            Invoke("FallDown", timeToFail);
    }

    private void FallDown()
    {
        if (success) return;
        if (isServer)
            CmdInvokeFailure();
    }

    [Command]
    private void CmdInvokeFailure()
    {
        RpcInvolkeFailure();
    }

    [ClientRpc]
    public void RpcInvolkeFailure()
    {
        ev.OnFailed.Invoke();
    }

    public void EvadeEventFailed()
    {
        SceneController.instance.currentEvents.Remove(ev);
        GameManager.instance.TakeDamage(2f);
        evadeLamp.OnDeactivation.Invoke();
    }

    public void EvadeEventSuccess()
    {
        success = true;
        evadeLamp.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(ev);
    }
}

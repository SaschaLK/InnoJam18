using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EvadeLeftHandler : NetworkBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;
    bool success = false;
    public EvadeLeft el;

    public ControlLamp evadeLeftLamb;

    public void BindEvent(EvadeLeft el)
    {
        this.el = el;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(el);
        Debug.Log("evade left");

        evadeLeftLamb.OnActivation.Invoke();
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
        el.OnFailed.Invoke();
    }

    public void EvadeLeftEventFailed()
    {
        SceneController.instance.currentEvents.Remove(el);

        if(GameManager.instance != null)
            GameManager.instance.TakeDamage(2f);

        evadeLeftLamb.OnDeactivation.Invoke();
    }

    public void EvadeLeftEventSuccess()
    {
        success = true;
        evadeLeftLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(el);
    }
}

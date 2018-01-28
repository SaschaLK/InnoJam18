using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EvadeRightHandler : NetworkBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;
    private bool success = false;
    public EvadeRight er;

    public ControlLamp evadeRightLamb;

    public void BindEvent(EvadeRight er)
    {
        this.er = er;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(er);
        Debug.Log("evade right");

        evadeRightLamb.OnActivation.Invoke();
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
        er.OnFailed.Invoke();
    }

    public void EvadeRightEventFailed()
    {
        SceneController.instance.currentEvents.Remove(er);
        GameManager.instance.TakeDamage(2f);
        evadeRightLamb.OnDeactivation.Invoke();
    }

    public void EvadeRightEventSuccess()
    {
        success = true;
        evadeRightLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(er);
    }
}

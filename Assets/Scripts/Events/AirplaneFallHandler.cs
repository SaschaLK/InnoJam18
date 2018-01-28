using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AirplaneFallHandler : NetworkBehaviour {

    private float timeToFail = BalancingConstant.AIRPLANEFALL_TIME; //change this value to control event difficulty

    Airplane airplane;

    public AirplaneFall af;
    private bool success = false;
    public ControlLamp fallLamb;

    public void BindEvent(AirplaneFall af)
    {
        this.af = af;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(af);
        Debug.Log("airplane is falling");

        // trigger camera animation here please!
        // camera isfalling = true
        fallLamb.OnActivation.Invoke();
        success = false;
        if (isServer)
            Invoke("FallDown", timeToFail);
    }

    private void FallDown()
    {
        if (success) return;
        if (isServer)
            af.OnFailed.Invoke();
    }

    [Command]
    private void CmdFallDown()
    {
        RpcFallDown();
    }

    [ClientRpc]
    private void RpcFallDown()
    {
        fallLamb.OnDeactivation.Invoke();
        GameManager.instance.EndGame();
    }

	public void FallEventFailed()
    {
        SceneController.instance.currentEvents.Remove(af);
        CmdFallDown();
    }

    public void FallEventSuccess()
    {
        // trigger camera animation here please!
        // camera isfalling = false
        success = true;
        fallLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(af);
    }
}

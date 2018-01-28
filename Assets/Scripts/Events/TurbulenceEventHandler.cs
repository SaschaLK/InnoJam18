using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurbulenceEventHandler : NetworkBehaviour {

    private float timeToEnd = BalancingConstant.TURBULENCES_TIME;

    Airplane airplane;
    private bool success = false;
    public TurbulenceEvent te;

    public ControlLamp turbulenceLamb;

    public void BindEvent(TurbulenceEvent te)
    {
        this.te = te;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(te);
        Debug.Log("turbulences");

        if (turbulenceLamb != null) turbulenceLamb.OnActivation.Invoke();

        success = false;
        if(isServer)
            Invoke("FallDown", timeToEnd);  
    }

    private void FallDown()
    {
        if (success) return;
        if (isServer && te != null)
            CmdOnFailed();
    }

    [Command]
    private void CmdOnFailed()
    {
        RpcOnFailed();
    }

    [ClientRpc]
    private void RpcOnFailed()
    {
        te.OnFailed.Invoke();
    }

    public void TurbulenceEventFailed()
    {
        ScreenShakeController.Instance.Trigger(Camera.main.transform, 4f, 0.2f);

        if (isServer) CmdEventFailed();
        SceneController.instance.currentEvents.Remove(te);

    }

    [Command]
    private void CmdEventFailed()
    {
        List<InteractiveComponent> stations = airplane.stations;
        if (stations.Count > 0)
        {
            int count = Random.Range(1, stations.Count);

            while (count > 0)
            {
                int rand = Random.Range(0, stations.Count);
                stations[rand].InflictDamage(10);

                RpcEventFailed(rand);
                stations.Remove(stations[rand]);
                count--;
            }
        }
    }

    [ClientRpc]
    private void RpcEventFailed(int rand)
    {
        if(turbulenceLamb != null)
            turbulenceLamb.OnDeactivation.Invoke();
        List<InteractiveComponent> stations = airplane.stations;
        if (stations.Count > rand) 
            ScreenShakeController.Instance.Trigger(stations[rand].transform, 1f, 1f);
    }

    public void TurbulenceEventSuccess()
    {
        success = true;
        if (turbulenceLamb != null) turbulenceLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(te);
    }
}

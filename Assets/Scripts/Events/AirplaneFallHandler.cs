using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AirplaneFallHandler : NetworkBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;

    public AirplaneFall af;

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

        fallLamb.OnActivation.Invoke();

        if(isServer)
            Invoke("FallDown", timeToFail);
    }

    private void FallDown()
    {
        if (isServer)
            af.OnFailed.Invoke();
    }

	public void FallEventFailed()
    {
        SceneController.instance.currentEvents.Remove(af);
        GameManager.instance.EndGame();
        fallLamb.OnDeactivation.Invoke();
    }

    public void FallEventSuccess()
    {
        fallLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(af);
    }
}

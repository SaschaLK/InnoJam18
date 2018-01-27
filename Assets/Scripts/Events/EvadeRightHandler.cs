using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeRightHandler : MonoBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;

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

        Invoke("FallDown", timeToFail);
    }

    private void FallDown()
    {
        er.OnFailed.Invoke();
    }

    public void EvadeRightEventFailed()
    {
        SceneController.instance.currentEvents.Remove(er);

        airplane.OnDamage.Invoke(2f);

        evadeRightLamb.OnDeactivation.Invoke();
    }

    public void EvadeRightEventSuccess()
    {
        evadeRightLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(er);
    }
}

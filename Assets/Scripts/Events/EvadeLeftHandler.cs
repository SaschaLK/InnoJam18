using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeLeftHandler : MonoBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;

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

        Invoke("FallDown", timeToFail);
    }

    private void FallDown()
    {
        el.OnFailed.Invoke();
    }

    public void EvadeLeftEventFailed()
    {
        SceneController.instance.currentEvents.Remove(el);

        airplane.OnDamage.Invoke(2f);

        evadeLeftLamb.OnDeactivation.Invoke();
    }

    public void EvadeLeftEventSuccess()
    {
        evadeLeftLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(el);
    }
}

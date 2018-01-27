using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeRight : GameEvent {

    public EvadeRight(ControlLamp lamp , float timeToHit)
    {
        Debug.Log("evade right or we will crash");

        lamp.OnActivation.Invoke();

        SceneController.instance.StartHitCountdown(timeToHit);
    }
}

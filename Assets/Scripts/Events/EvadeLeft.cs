using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeLeft : GameEvent {

    public EvadeLeft(ControlLamp lamp , float timeToHit)
    {
        if (lamp == null) return;
        lamp.OnActivation.Invoke();

        SceneController.instance.StartHitCountdown(timeToHit);
    }
}

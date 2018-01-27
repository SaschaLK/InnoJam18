﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeRight : GameEvent {

    public EvadeRight(EvadeRightHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.EvadeRightEventFailed);
        this.OnSuccess.AddListener(handler.EvadeRightEventSuccess);
    }

    /* public EvadeRight(ControlLamp lamp, float timeToHit)
     {
         if (lamp == null) return;
         Debug.Log("evade right or we will crash");

         lamp.OnActivation.Invoke();

         SceneController.instance.StartHitCountdown(timeToHit);
     }*/
}

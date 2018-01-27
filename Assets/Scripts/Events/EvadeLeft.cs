﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeLeft : GameEvent {

    public EvadeLeft(EvadeLeftHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.EvadeLeftEventFailed);
        this.OnSuccess.AddListener(handler.EvadeLeftEventSuccess);
    }

    /*public EvadeLeft(ControlLamp lamp , float timeToHit)
    {
        if (lamp == null) return;
        Debug.Log("evade left or we will crash");

        lamp.OnActivation.Invoke();

        SceneController.instance.StartHitCountdown(timeToHit);
    }*/
}

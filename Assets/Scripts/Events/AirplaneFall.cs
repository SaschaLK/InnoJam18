﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

    public AirplaneFall(AirplaneFallHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.EnemyEventFailed);
        this.OnSuccess.AddListener(handler.EnemyEventSuccess);
    }

    /*public AirplaneFall(ControlLamp lamp)
    {
        if (lamp == null) return;
        Debug.Log("airplane is falling");

        lamp.OnActivation.Invoke();
    }*/
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : GameEvent {

    public Evade(EvadeHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.EvadeEventFailed);
        this.OnSuccess.AddListener(handler.EvadeEventSuccess);
    }

    public void TriggerOnComplete()
    {
        this.OnSuccess.Invoke();
    }


}

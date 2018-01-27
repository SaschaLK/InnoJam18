using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEvent : GameEvent {


    public TurbulenceEvent(TurbulenceEventHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.TurbulenceEventFailed);
        this.OnSuccess.AddListener(handler.TurbulenceEventSuccess);
    }

   
}

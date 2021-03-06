﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

    public AirplaneFall(AirplaneFallHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.FallEventFailed);
        this.OnSuccess.AddListener(handler.FallEventSuccess);
    }

}

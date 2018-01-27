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


}

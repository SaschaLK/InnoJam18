﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyApproach : GameEvent {

   
    public EnemyApproach (EnemyApproachHandler handler) {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.EnemyEventFailed);
        this.OnSuccess.AddListener(handler.EnemyEventSuccess);
	}

}

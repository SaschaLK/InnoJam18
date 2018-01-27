﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproach : GameEvent {

	public EnemyApproach (SirenLamp lamp) {
        if (lamp == null) return;
        Debug.Log("enemy is approaching");
        lamp.StartAlarm();
	}
}

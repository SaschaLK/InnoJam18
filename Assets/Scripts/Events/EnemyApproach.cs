﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproach : GameEvent {

	public EnemyApproach (Airplane airplane) {

        if (airplane == null) return;
        Debug.Log("enemy is approaching");
        airplane.StartEnemyLamps();
	}
}

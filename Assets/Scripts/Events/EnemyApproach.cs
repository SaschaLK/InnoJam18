using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproach : GameEvent {

	public EnemyApproach (SirenLamp lamp) {
        Debug.Log("enemy is approaching");
        lamp.StartAlarm();
	}
}

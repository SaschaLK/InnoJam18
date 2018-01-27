using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproach : GameEvent {

	// Use this for initialization
	public EnemyApproach (GameObject lamp) {
		//TODO: make warning light blink red and start alarm
		lamp.GetComponent<AudioSource>().Play();
	}
}

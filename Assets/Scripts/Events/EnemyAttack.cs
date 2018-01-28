﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : GameEvent {

	public EnemyAttack(Airplane airplane , Camera camera)
	{
        Debug.Log("enemy is shooting at us");

        ScreenShakeController.Instance.Trigger(Camera.main.transform, 0.5f, 1f);

		List<InteractiveComponent> stations = airplane.stations;
        if (stations.Count <= 0) return;
        stations[Random.Range(0, stations.Count)].TakeFatalDamage();
    }

}

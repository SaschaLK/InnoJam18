﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : GameEvent {


	public EnemyAttack(Airplane airplane)
	{
		//TODO: Camerashake

		int count = Random.Range(1, 3);
		List<InteractiveComponent> stations = airplane.stations;

		while (count < 0)
		{
			int rand = Random.Range(0, stations.Count);
			stations[rand].InflictDamage(10);
			stations.Remove(stations[rand]);
			count--;
		}
	}
}

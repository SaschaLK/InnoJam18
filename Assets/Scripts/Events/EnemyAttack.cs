using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : GameEvent {

	public EnemyAttack(Airplane airplane , Camera camera)
	{
        //TODO: Camerashake
       // ScreenShakeController.Instance.Trigger();

		List<InteractiveComponent> stations = airplane.stations;

		int count = Random.Range(1, stations.Count);

		while (count > 0)
		{
			int rand = Random.Range(0, stations.Count);
			stations[rand].InflictDamage(10);
			stations.Remove(stations[rand]);
			count--;
		}
	}
}

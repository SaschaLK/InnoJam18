using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEvent : GameEvent {

	public TurbulenceEvent(Airplane airplane)
    {
        List<InteractiveComponent> stations = airplane.stations;

        int rand = Random.Range(0, stations.Count);

        stations[rand].InflictDamage(stations[rand].health);
    }
}

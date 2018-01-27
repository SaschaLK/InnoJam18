using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEvent : GameEvent {

	public TurbulenceEvent(Airplane airplane)
    {
        Debug.Log("turbulences incoming");
        //TODO: Camera shake
        ScreenShakeController.Instance.Trigger(4f, 0.2f);

        List<InteractiveComponent> stations = airplane.stations;

        int rand = Random.Range(0, stations.Count);

        stations[rand].InflictDamage(stations[rand].health);
    }
}

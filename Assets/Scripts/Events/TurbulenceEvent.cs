using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEvent : GameEvent {

    private Airplane airplane;
    List<InteractiveComponent> stations;
    int[] randoms;

    public TurbulenceEvent(Airplane _airplane)
    {
        airplane = _airplane;
        stations = airplane.stations;
        int count = UnityEngine.Random.Range(1, stations.Count);
        randoms = new int[count];

        for(int i = 0; i < count; i++) { 
            randoms[i] = UnityEngine.Random.Range(0, stations.Count);
        }
    }

    public override void triggerStart()
    {
        Debug.Log("turbulences incoming");
        ScreenShakeController.Instance.Trigger(Camera.main.transform, 4f, 0.2f);

        for(int i = 0; i < randoms.Length; i++) {
            stations[randoms[i]].InflictDamage(10);
            ScreenShakeController.Instance.Trigger(stations[randoms[i]].transform, 1f, 1f);
            stations.Remove(stations[randoms[i]]);
        }
    }
}

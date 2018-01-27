using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEvent : GameEvent {

    public TurbulenceEvent(TurbulenceEventHandler handler)
    {

        handler.BindEvent(this);

        this.OnEventStart.AddListener(handler.OnEventStart);
        this.OnFailed.AddListener(handler.TurbulenceEventFailed);
        this.OnSuccess.AddListener(handler.TurbulenceEventSuccess);
    }

    /*public TurbulenceEvent(Airplane airplane)
    {
        Debug.Log("turbulences incoming");
        //TODO: Camera shake
        ScreenShakeController.Instance.Trigger(Camera.main.transform, 4f, 0.2f);

        List<InteractiveComponent> stations = airplane.stations;

        int count = Random.Range(1, stations.Count);

        while (count > 0)
        {
            int rand = Random.Range(0, stations.Count);
            stations[rand].InflictDamage(10);

            ScreenShakeController.Instance.Trigger(stations[rand].transform, 1f, 1f);

            stations.Remove(stations[rand]);

            count--;
        }
    }*/
}

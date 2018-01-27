using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEventHandler : MonoBehaviour {

    private float timeToEnd = 5f;

    Airplane airplane;

    public TurbulenceEvent te;

    public ControlLamp turbulenceLamb;

    public void BindEvent(TurbulenceEvent te)
    {
        this.te = te;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        SceneController.instance.currentEvents.Add(te);
        Debug.Log("turbulences");

        turbulenceLamb.OnActivation.Invoke();

        Invoke("FallDown", timeToEnd);
    }

    private void FallDown()
    {
        te.OnFailed.Invoke();
    }

    public void TurbulenceEventFailed()
    {
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
        SceneController.instance.currentEvents.Remove(te);
        turbulenceLamb.OnDeactivation.Invoke();
    }

    public void TurbulenceEventSuccess()
    {
        turbulenceLamb.OnDeactivation.Invoke();
        SceneController.instance.currentEvents.Remove(te);
    }
}

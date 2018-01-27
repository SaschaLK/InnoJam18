using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachHandler : MonoBehaviour {

    private float timeToFail = 20f; //change this value to control event difficulty

    Airplane airplane;

    public EnemyApproach ea;

    public void BindEvent(EnemyApproach ea)
    {
        this.ea = ea;
        airplane = SceneController.instance.airplane;
    }

    public void OnEventStart()
    {
        if (airplane == null) return;
        Debug.Log("enemy is approaching");

        //TODO see is only three or check stuff
        InvokeRepeating("AttackShip", timeToFail / 4, timeToFail / 4);
        airplane.StartEnemyLamps();

    }

    private void AttackShip()
    {
        List<InteractiveComponent> stations = airplane.stations;

        int rand = Random.Range(0, stations.Count);

        
        stations[rand].InflictDamage(stations[rand].health);
    }

	public void EnemyEventFailed()
    {
        SceneController.instance.currentEvents.Remove(ea);
        airplane.StopEnemyLamps();
    }

    public void EnemyEventSuccess()
    {
        airplane.StopEnemyLamps();
        SceneController.instance.currentEvents.Remove(ea);
    }
}

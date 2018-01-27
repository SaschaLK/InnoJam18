using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachHandler : MonoBehaviour {

    Airplane airplane;

    public EnemyApproach ea;

    public void OnEventStart()
    {
        if (airplane == null) return;
        Debug.Log("enemy is approaching");

        airplane.StartEnemyLamps();

    }

	public void EnemyEventFailed()
    {

    }

    public void EnemyEventSuccess()
    {

    }
}

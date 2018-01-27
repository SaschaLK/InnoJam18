using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Airplane : NetworkBehaviour {

    [SyncVar]
    public float hitPoints = 10;

    public List<InteractiveComponent> stations;

    public ControlLamp[] enemyLamps;

    public Transform enemyLampContainer;

    public AirplaneDamageEvent OnDamage = new AirplaneDamageEvent();

    private void Awake()
    {
        enemyLamps = enemyLampContainer.GetComponentsInChildren<ControlLamp>();
    }

    public void StartEnemyLamps()
    {
        Debug.Log("start lamps "+ enemyLamps.Length);
        foreach(ControlLamp lamp in enemyLamps)
        {
            lamp.OnActivation.Invoke();
        }
    }

    public void StopEnemyLamps()
    {
        foreach (ControlLamp lamp in enemyLamps)
        {
            lamp.OnDeactivation.Invoke();
        }
    }

    private void CrashAirplane()
	{
		GameManager.instance.EndGame();
	}
}

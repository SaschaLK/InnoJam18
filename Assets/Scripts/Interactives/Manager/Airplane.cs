using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

	public List<InteractiveComponent> stations;

    public ControlLamp[] enemyLamps;

    public Transform enemyLampContainer;

    private void Start()
    {
        enemyLamps = enemyLampContainer.GetComponentsInChildren<ControlLamp>();
    }

    public void StartEnemyLamps()
    {
        foreach(ControlLamp lamp in enemyLamps)
        {
            lamp.OnActivation.Invoke();
        }
    }

	private void CrashAirplane()
	{
		GameManager.instance.EndGame();
	}
}

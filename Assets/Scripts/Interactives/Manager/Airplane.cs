using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

	public List<InteractiveComponent> stations;

    public List<ControlLamp> enemyLamps;

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

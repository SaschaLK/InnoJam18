using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

	public List<InteractiveComponent> stations;

	private void CrashAirplane()
	{
		GameManager.instance.EndGame();
	}
}

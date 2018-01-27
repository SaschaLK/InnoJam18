using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

	public List<InteractiveComponent> stations;

	public float totalHeight;
	public float riseFall = 0;

	private void Start()
	{
		InvokeRepeating ("RiseFall", 0f, 1f);
	}

	private void Update()
	{
		if (riseFall <= 5) {

		}

		if (totalHeight <= 0)
		{
			CrashAirplane();
		}
	}

	private void RiseFall()
	{
		totalHeight += riseFall;
	}

	private void CrashAirplane()
	{
		GameManager.instance.EndGame();
	}
}

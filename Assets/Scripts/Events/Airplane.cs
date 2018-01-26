using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

	//public List<Station> stations;

	public float totalHeight;
	public float riseFall = 0;

	private void Start()
	{
		StartCoroutine(RiseFall());
	}

	private void Update()
	{
		if (totalHeight <= 0)
		{
			CrashAirplane();
		}
	}

	private IEnumerator RiseFall()
	{
		while (true)
		{
			totalHeight += riseFall;
			yield return new WaitForSeconds(1f);
		}
	}

	private void CrashAirplane()
	{
		GameManager.instance.EndGame();
	}
}

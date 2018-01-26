using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : StationState{

	private void OnEnable()
	{
		StartCoroutine(FireDamage());
	}

	private void OnDisable()
	{
		StopCoroutine(FireDamage());
	}

	private IEnumerator FireDamage()
	{
		while (true)
		{
			int damage = Random.Range(5, 10);
			//GetComponentInParent<Station>().InflictDamage(damage);
			yield return new WaitForSeconds(1);
		}
	}
}

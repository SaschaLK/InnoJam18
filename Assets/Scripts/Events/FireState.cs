using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : StationState{

	private void OnEnable()
	{
		InvokeRepeating ("FireDamage", 1f, 1f);
		//Spawn fire 
	}

	private void OnDisable()
	{

		//delete fire
	}

	private void FireDamage()
	{
		float damage = Random.Range(5, 10);
		GetComponentInParent<InteractiveComponent>().InflictDamage(damage);
	}
}

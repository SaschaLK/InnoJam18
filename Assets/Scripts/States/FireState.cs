using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : LockedUpStateBase {

    protected override void Awake() {
        base.Awake();
    }

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
		interactive.InflictDamage(damage);
	}

}

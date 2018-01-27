using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : StationState{

    InteractiveComponent interactive;

    private void Awake() {
        interactive = GetComponentInParent<InteractiveComponent>();

        _CanUse = interactive.CanUse;
        interactive.CanUse = CanUse;
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

    System.Func<PlayerController, bool> _CanUse; // vorherige Bedingung
    bool CanUse(PlayerController player) {
        // if (!fireState && vorherigeBedingung)

        if (this != null)
            return false;

        if (_CanUse != null)
            return _CanUse(player);
        return true; // Standardwert: Wir können es verwenden.
    }

}

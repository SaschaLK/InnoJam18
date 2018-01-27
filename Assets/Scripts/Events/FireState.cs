using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : StationState{

    InteractiveComponent interactive;

    private void Awake() {
        interactive = GetComponentInParent<InteractiveComponent>();

        _CanUse = interactive.CanInteract; // Speichere vorherige Bedingung zwischen.
        interactive.CanInteract = CanUse; // Neue Interactive CanUse Bedingung.
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

        if (this != null) // Während das Feuer am Leben ist...
            return false; // ... können wir das derzeitige Interactive nicht verwenden.

        if (_CanUse != null)
            return _CanUse(player); // Rufe vorherige Bedingung ab.
        return true; // Standardwert: Wir können es verwenden.
    }

}

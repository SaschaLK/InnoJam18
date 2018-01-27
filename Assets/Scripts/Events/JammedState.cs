using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammedState : StationState
{
    private float jammedTime = 3f; //Set this value to change jammed time

    InteractiveComponent interactive;

    private void Awake()
    {
        interactive = GetComponentInParent<InteractiveComponent>();

        _CanUse = interactive.CanInteract; // Speichere vorherige Bedingung zwischen.
        interactive.CanInteract = CanUse; // Neue Interactive CanUse Bedingung.
    }

    private void OnEnable()
    {
        StartCoroutine(BrokenTimer()); 
    }

    /// <summary>
    /// wait for the time to pass and make machine usable again
    /// </summary>
    /// <returns></returns>
    private IEnumerator BrokenTimer()
    {
        yield return new WaitForSeconds(jammedTime);
        Destroy(this.gameObject);
    }

    System.Func<PlayerController, bool> _CanUse; // vorherige Bedingung
    bool CanUse(PlayerController player)
    {
        // if (!fireState && vorherigeBedingung)

        if (this != null) // Während die Maschine jammed ist...
            return false; // ... können wir das derzeitige Interactive nicht verwenden.

        if (_CanUse != null)
            return _CanUse(player); // Rufe vorherige Bedingung ab.
        return true; // Standardwert: Wir können es verwenden.
    }

}

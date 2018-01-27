using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammedState : LockedUpStateBase
{
    private float jammedTime = 3f; //Set this value to change jammed time

    protected override void Awake()
    {
        base.Awake();
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

    protected override bool CanUse(PlayerController player)
    {
        // if (!fireState && vorherigeBedingung)

        if (this != null) // Während die Maschine jammed ist...
            return false; // ... können wir das derzeitige Interactive nicht verwenden.

        if (__CanUse != null)
            return __CanUse(player); // Rufe vorherige Bedingung ab.
        return true; // Standardwert: Wir können es verwenden.
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LockedUpStateBase : StationState
{

    string type;
    protected InteractiveComponent interactive;

    protected virtual void Awake()
    {
        type = GetType().Name;
        interactive = GetComponentInParent<InteractiveComponent>();

        __CanUse = interactive.CanInteract; // Speichere vorherige Bedingung zwischen.
        interactive.CanInteract = _CanUse; // Neue Interactive CanUse Bedingung.
    }

    protected System.Func<PlayerController, bool> __CanUse; // vorherige Bedingung
    private bool _CanUse(PlayerController player) { return CanUse(player); }
    protected virtual bool CanUse(PlayerController player)
    {

        if (this == null) { // Falls das derzeitige State destroyed wurde
            if (__CanUse != null)
                return __CanUse(player); // Rufe vorherige Bedingung ab.
            return true; // Standardwert: Wir können es verwenden.
        }

        if (player.Item != null) {
            // Wenn der Player ein Item hält...
            ItemStateRemover remover = player.Item.GetComponent<ItemStateRemover>();
            if (remover != null) {
                // ... und es ein ItemStateRemover ist...
                if (remover.State + "State" == type) {
                    // ... und der Remover das State entfernt...
                    return true; // ... erlauben wir Interaktion.
                }
            }
        }

        return false; // Wir können das Objekt normalerweise nicht mehr verwenden.
    }

}

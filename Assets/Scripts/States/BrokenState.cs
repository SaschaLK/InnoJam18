﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenState : StationState
{

    InteractiveComponent interactive;

    private void Awake()
    {
        interactive = GetComponentInParent<InteractiveComponent>();

        _CanUse = interactive.CanInteract; // Speichere vorherige Bedingung zwischen.
        interactive.CanInteract = CanUse; // Neue Interactive CanUse Bedingung.
    }

    System.Func<PlayerController, bool> _CanUse; // vorherige Bedingung
    bool CanUse(PlayerController player)
    {
        // if (!fireState && vorherigeBedingung)

        if (player.Item != null) {
            // Wenn der Player ein Item hält...
            ItemStateRemover remover = player.Item.GetComponent<ItemStateRemover>();
            if (remover != null) {
                // ... und es ein ItemStateRemover ist...
                if (remover.Type == "BrokenState") {
                    // ... und der Remover das State entfernt...
                    return true; // ... erlauben wir Interaktion.
                }
            }
        }

        if (this != null) // Während die Maschine broken ist ist...
            return false; // ... können wir das derzeitige Interactive nicht verwenden.

        if (_CanUse != null)
            return _CanUse(player); // Rufe vorherige Bedingung ab.
        return true; // Standardwert: Wir können es verwenden.
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class InteractiveComponent : MonoBehaviour {

    public string DisplayName;

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public UnityEvent OnInteract = new UnityEvent();

    public Func<PlayerController, bool> CanUse;

    /// <summary>
    /// Inflicts Damage caused by fire etc
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(int damage)
	{
		Debug.Log(this.name + " is damaged by " + damage);
	}

}

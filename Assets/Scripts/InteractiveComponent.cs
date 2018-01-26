using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveComponent : MonoBehaviour {

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public UnityEvent OnInteract = new UnityEvent();

	/// <summary>
	/// Inflicts Damage caused by fire etc
	/// </summary>
	/// <param name="damage"></param>
	public void InflictDamage(int damage)
	{
		Debug.Log(this.name + " is damaged by " + damage);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveComponent : MonoBehaviour {

    public float health = 100;

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public UnityEvent OnInteract = new UnityEvent();

    public InternalDamageEvent OnDamage = new InternalDamageEvent();

    public DestroyEvent OnDestroy = new DestroyEvent();

    /// <summary>
    /// Inflicts Damage caused by fire etc
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(float damage)
	{
        Debug.Log(this.name + " is damaged by " + damage);
        health -= damage;

        OnDamage.Invoke();

        if(health <= 0)
        {
            OnDestroy.Invoke();
        }
	}



}

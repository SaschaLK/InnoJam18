﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class InteractiveComponent : MonoBehaviour {

    public string DisplayName;

    public float health = 100;

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public UnityEvent OnInteract = new UnityEvent();

    /// <summary>
    /// Invoked by InflictDamage
    /// </summary>
    public InternalDamageEvent OnDamage = new InternalDamageEvent();

    /// <summary>
    /// Invoked by InflictDamage if health &lt;= 0
    /// </summary>
    public DestroyEvent OnDestroy = new DestroyEvent();

    public Func<PlayerController, bool> CanUse;

    /// <summary>
    /// Inflicts Damage caused by fire etc
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(float damage)
    {
        Debug.Log(this.name + " is damaged by " + damage);
        health -= damage;

        OnDamage.Invoke();

        if (health <= 0)
        {
            OnDestroy.Invoke();
        }
    }

}

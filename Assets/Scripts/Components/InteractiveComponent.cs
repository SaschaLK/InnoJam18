﻿using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public sealed class InteractiveComponent : NetworkBehaviour {

    public string DisplayName;

    [SyncVar]
    private float health = 100;

    public bool HealthLargerThan(float t)
    {
        return health >= t;
    }

    public void ChargeUp(float chrg)
    {
        if (!isServer) return;
        health = Math.Min(100, Math.Max(0, health) + chrg);
    }

    public void TakeFatalDamage()
    {
        TakeDamage(health);
    }

        public void TakeDamage(float damage)
    {
        if (!isServer) return;
        health -= damage;

        // let the clients see some fun screenshake, yeah!
        CmdTakeDamage(damage);

        if (health <= 0)
        {
            health = 0f;
            CmdDestroyCommand(); // destroy on all clients
        }
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        RpcTakeDamage(damage);
    }

    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        ScreenShakeController.Instance.Trigger(transform, 1f, damage / 200);
    }

    [Command]
    public void CmdDestroyCommand()
    {
        RpcDestroyCommand();
    }

    [ClientRpc]
    public void RpcDestroyCommand()
    {
        OnDestroy.Invoke();
    }
   
    public bool Highlight = false;
    public Outline Outline { get; private set; }

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public InteractEvent OnInteract = new InteractEvent();

    /// <summary>
    /// Invoked by InflictDamage
    /// </summary>
    public InternalDamageEvent OnDamage = new InternalDamageEvent();

    /// <summary>
    /// Invoked by InflictDamage if health &lt;= 0
    /// </summary>
    public DestroyEvent OnDestroy = new DestroyEvent();

    public Func<PlayerController, bool> CanInteract;
    public Func<PlayerController, InteractiveComponent, bool> CanMinigame;

    /// <summary>
    /// Inflicts Damage caused by fire etc
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(float damage) {
        Debug.Log(this.name + " is damaged by " + damage);
        
        OnDamage.Invoke(damage);
    }

    private void Awake() {
        if (Outline == null)
            Outline = GetComponentInChildren<Outline>();
        if (Outline == null) {
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null) {
                Outline = renderer.gameObject.AddComponent<Outline>();
            }
        }
    }

    private void LateUpdate() {
        if (Outline != null) {
            Outline.enabled = Highlight;
            Highlight = false;
        }
    }

}

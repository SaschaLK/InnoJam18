using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class InteractiveComponent : MonoBehaviour {

    public string DisplayName;

    public float health = 100;

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

    public Func<PlayerController, bool> CanUse;

    /// <summary>
    /// Inflicts Damage caused by fire etc
    /// </summary>
    /// <param name="damage"></param>
    public void InflictDamage(float damage) {
        Debug.Log(this.name + " is damaged by " + damage);
        health -= damage;

        OnDamage.Invoke(damage);

        if (health <= 0) {
            health = 0f;
            OnDestroy.Invoke();
        }
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

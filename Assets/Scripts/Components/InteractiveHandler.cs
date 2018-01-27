using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractiveComponent))]
public class InteractiveHandler : MonoBehaviour {

    public InteractiveComponent interactive { get; private set; }

    protected virtual void Awake() {
        interactive = GetComponent<InteractiveComponent>();
        interactive.OnInteract.AddListener(_OnInteract);
        interactive.OnDamage.AddListener(_OnDamage);
        interactive.OnDestroy.AddListener(_OnDestroy);
    }

    private void _OnInteract(PlayerController player) { OnInteract(player); }
    public virtual void OnInteract(PlayerController player) {
    }

    private void _OnDamage(int damage) { OnDamage(damage); }
    public virtual void OnDamage(int damage) {
    }

    private void _OnDestroy() { OnDestroyI(); }
    public virtual void OnDestroyI() {
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemComponent))]
public class ItemHandler : MonoBehaviour {

    public ItemComponent item { get; private set; }

    protected virtual void Awake() {
        item = GetComponent<ItemComponent>();
        item.OnPickup.AddListener(_OnPickup);
        item.OnDrop.AddListener(_OnDrop);
        item.OnUse.AddListener(_OnUse);
        item.OnUseWith.AddListener(_OnUseWith);
    }

    private void _OnPickup() { OnPickup(); }
    public virtual void OnPickup() {
    }

    private void _OnDrop() { OnDrop(); }
    public virtual void OnDrop() {
    }

    private void _OnUse() { OnUse(); }
    public virtual void OnUse() {
    }

    private void _OnUseWith(InteractiveComponent with) { OnUseWith(with); }
    public virtual void OnUseWith(InteractiveComponent with) {
    }

}

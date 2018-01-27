using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemComponent))]
public class ItemHandlerBase : MonoBehaviour {

    protected ItemComponent item;

    protected virtual void Awake() {
        item = GetComponent<ItemComponent>();
        item.OnPickup.AddListener(_Pickup);
        item.OnDrop.AddListener(_Drop);
        item.OnUse.AddListener(_Use);
        item.OnUseWith.AddListener(_UseWith);
    }

    private void _Pickup() { Pickup(); }
    public virtual void Pickup() {
    }

    private void _Drop() { Drop(); }
    public virtual void Drop() {
    }

    private void _Use() { Use(); }
    public virtual void Use() {
    }

    private void _UseWith(InteractiveComponent with) { UseWith(with); }
    public virtual void UseWith(InteractiveComponent with) {
    }

}

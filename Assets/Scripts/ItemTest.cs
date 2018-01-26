using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemComponent))]
public class ItemTest : MonoBehaviour {

    ItemComponent item;

    private void Awake() {
        item = GetComponent<ItemComponent>();
        item.OnPickup.AddListener(Pickup);
        item.OnDrop.AddListener(Drop);
        item.OnUse.AddListener(Use);
        item.OnUseWith.AddListener(UseWith);
    }

    public void Pickup() {
        Debug.Log("Pickup " + name);
    }

    public void Drop() {
        Debug.Log("Drop " + name);
    }

    public void Use() {
        Debug.Log("Use " + name);
    }

    public void UseWith(InteractiveComponent with) {
        Debug.Log("Use " + name + " with " + with.name);
    }

}

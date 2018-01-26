using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float MovementSpeed = 10f;
    public float UsageRadius = 5f;

    float closestDist;
    InteractiveComponent closest;

    Collider collider;
    Rigidbody rigidbody;

    public ItemComponent Item { get; protected set; }

	void Awake() {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update() {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rigidbody.velocity = new Vector3(
            x * MovementSpeed,
            rigidbody.velocity.y,
            y * MovementSpeed
        );

        InteractiveComponent[] interactives = FindObjectsOfType<InteractiveComponent>();
        Vector3 pos = transform.position;
        closestDist = UsageRadius * UsageRadius;
        closest = null;
        for (int i = 0; i < interactives.Length; i++) {
            InteractiveComponent interactive = interactives[i];

            float dist = (pos - interactive.transform.position).sqrMagnitude;
            bool canUse = true; // TODO: Dependencies

            if (!canUse || dist > closestDist) {
                continue;
            }

            closestDist = dist;
            closest = interactive;
        }

        bool use = Input.GetButtonDown("Fire1");
        if (closest != null) {
            closestDist = Mathf.Pow(closestDist, 0.5f);
            if (use) {
                if (Item != null) {
                    UseItemWith();
                } else {
                    Interact();
                }
            }
        } else if (use) {
            UseItem();
        }

    }

    /// <summary>
    /// Interact with passed object or with closest object.
    /// </summary>
    public void Interact(InteractiveComponent interactive = null) {
        if (interactive == null)
            interactive = closest;
        if (interactive == null)
            return;

        interactive.OnInteract.Invoke();
        PickupItem();
    }

    /// <summary>
    /// Pick up passed item or closest item.
    /// </summary>
    public void PickupItem(ItemComponent item = null) {
        if (item == null && closest != null)
            item = closest.GetComponent<ItemComponent>();
        if (item == null)
            return;

        DropItem();
        Item = item;
        item.OnPickup.Invoke();
    }

    /// <summary>
    /// Use passed or currently held item.
    /// </summary>
    public void UseItem(ItemComponent item = null) {
        if (item == null)
            item = Item;
        if (item == null)
            return;

        item.OnUse.Invoke();
    }

    /// <summary>
    /// Use passed or currently held item with / on the passed or closest interactive object.
    /// </summary>
    public void UseItemWith(ItemComponent item = null, InteractiveComponent with = null) {
        if (item == null)
            item = Item;
        if (item == null)
            return;

        if (with == null)
            with = closest;
        if (with == null)
            return;

        item.OnUseWith.Invoke(with);
    }

    /// <summary>
    /// Drop currently held item.
    /// </summary>
    public void DropItem() {
        if (Item == null)
            return;

        Item.OnDrop.Invoke();
        Item = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, UsageRadius);
        if (closest != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closestDist);
        }
    }

}

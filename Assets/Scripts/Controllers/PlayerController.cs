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
    public InteractiveComponent Closest { get { return closest; } }

    new Collider collider;
    new Rigidbody rigidbody;
    public PlayerTooltipController Tooltip { get; protected set; }

    bool mouseTurning;
    Vector3 mousePosPrev = new Vector3(0f, 0f, 0f);
    Vector2 lookPrev = new Vector3(0f, 0f);

    public ItemComponent Item { get; protected set; }

    public Transform HoldingPoint;

	void Awake() {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        Tooltip = GetComponentInChildren<PlayerTooltipController>();

        if (HoldingPoint == null)
            HoldingPoint = transform.Find("HoldingPoint");

    }
	
	void Update() {
        Vector2 pos = transform.position.XZ();

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rigidbody.velocity = new Vector3(
            x * MovementSpeed,
            rigidbody.velocity.y,
            y * MovementSpeed
        );

        Vector3 mousePos = Input.mousePosition;
        if (new Vector2(
            mousePos.x - mousePosPrev.x,
            mousePos.y - mousePosPrev.y
        ).sqrMagnitude >= 1f) {
            mouseTurning = true;
            mousePosPrev = mousePos;
        }

        Vector2 look = new Vector2(
            Input.GetAxis("Horizontal Look"),
            Input.GetAxis("Vertical Look")
        );

        bool stickTurning = look.sqrMagnitude > 0.01;
        mouseTurning &= !stickTurning;

        if (stickTurning)
            lookPrev = look;

        if (mouseTurning) {
            Vector3 posScreen = Camera.main.WorldToScreenPoint(pos);
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(
                posScreen.y - mousePos.y,
                mousePos.x - posScreen.x
            ) * Mathf.Rad2Deg, 0f);
        } else {
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(
                lookPrev.y,
                lookPrev.x
            ) * Mathf.Rad2Deg, 0f);
        }

        Vector2 lookDir = transform.right.XZ();
        float lookRayAngle = Mathf.Atan2(
            lookDir.y,
            lookDir.x
        ) * Mathf.Rad2Deg;

        rigidbody.angularVelocity = Vector3.zero;

        InteractiveComponent[] interactives = FindObjectsOfType<InteractiveComponent>();
        closestDist = UsageRadius * UsageRadius;
        closest = null;
        for (int i = 0; i < interactives.Length; i++) {
            InteractiveComponent interactive = interactives[i];
            Vector2 interactivePos = interactive.transform.position.XZ();

            float distToCenter = (pos - interactivePos).sqrMagnitude;

            Vector2 delta = interactivePos - pos;
            float angle = Mathf.Atan2(
                delta.y,
                delta.x
            ) * Mathf.Rad2Deg;
            angle -= lookRayAngle;
            Debug.Log(interactive.name + " " + angle);
            if (angle >= 180f)
                continue; // Behind us.
            angle = Mathf.Abs(angle);

            float dist = 0.5f * distToCenter + 0.6f * UsageRadius * UsageRadius * angle / 45f;

            bool canUse = interactive.CanInteract != null ? interactive.CanInteract(this) : true;

            if (Item != null && interactive.transform == Item.transform)
                continue;

            if (!canUse || dist > closestDist) {
                continue;
            }

            closestDist = dist;
            closest = interactive;
        }

        bool use = Input.GetButtonDown("Fire1");
        bool drop = Input.GetButtonDown("Fire2");
        if (closest != null) {
            closest.Highlight = true;
            Tooltip.ShowText(closest.DisplayName);
            closestDist = Mathf.Pow(closestDist, 0.5f);
            if (use && !drop) {
                if (Item != null && closest.GetComponent<ItemComponent>() == null) {
                    UseItemWith();
                } else {
                    Interact();
                }
            }
        } else if (use && !drop) {
            UseItem();
        }

        if (drop) {
            DropItem();
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

        interactive.OnInteract.Invoke(this);
        ItemComponent item = interactive.GetComponent<ItemComponent>();
        if (item != null)
            PickupItem(item);
    }

    /// <summary>
    /// Pick up passed item or closest item.
    /// </summary>
    public void PickupItem(ItemComponent item = null) {
        if (item == null && closest != null)
            item = closest.GetComponent<ItemComponent>();
        if (item == null)
            return;

        ItemComponent prevItem = Item;
        DropItem(); // Drop any previous items.
        if (prevItem != null) {
            // Put the prev item at the replacing item's pos.
            prevItem.transform.position = item.transform.position;
        }

        Item = item;

        Item.Holder = this;
        Item.transform.parent = HoldingPoint;
        Item.transform.localPosition = Item.HoldOffset;
        Item.transform.localRotation = Item.HoldRotation;

        Rigidbody body = Item.GetComponent<Rigidbody>();
        if (body != null)
            Destroy(body);
        Collider collider = Item.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        item.OnPickup.Invoke();
    }

    /// <summary>
    /// Drop currently held item.
    /// </summary>
    public void DropItem() {
        if (Item == null)
            return;

        Item.OnDrop.Invoke();

        Item.Holder = null;
        Item.transform.parent = null;

        Item.gameObject.AddComponent<Rigidbody>();
        Collider collider = Item.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        Item = null;
    }

    /// <summary>
    /// Use passed or currently held item.
    /// </summary>
    public void UseItem(ItemComponent item = null) {
        if (item == null)
            item = Item;
        if (item == null)
            return;

        if (item.CanUse != null ? item.CanUse(this) : true)
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

        if (item.CanUseWith != null ? item.CanUseWith(this, with) :
            item.CanUse != null ? item.CanUse(this) : true)
            item.OnUseWith.Invoke(with);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, UsageRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * UsageRadius);
        if (closest != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closestDist);
            Gizmos.DrawWireCube(closest.transform.position, closest.transform.lossyScale);
        }
    }

}

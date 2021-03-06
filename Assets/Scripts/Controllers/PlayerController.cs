﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {

    public static PlayerController LocalPlayer;

    public float MovementSpeed = 10f;
    public float UsageRadius = 5f;

    float closestDist;
    InteractiveComponent closest;
    public InteractiveComponent Closest { get { return closest; } }

    new Collider collider;
    new Rigidbody rigidbody;
    public PlayerUIController Tooltip { get; protected set; }

    bool mouseTurning;
    Vector3 mousePosPrev = new Vector3(0f, 0f, 0f);
    Vector2 lookPrev = new Vector3(0f, 0f);

    public ItemComponent Item { get; protected set; }

    public Transform HoldingPoint;

    public bool Locked;

    public int PlayerNumber;

    void Awake() {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        Tooltip = GetComponentInChildren<PlayerUIController>();

        if (HoldingPoint == null)
            HoldingPoint = transform.Find("HoldingPoint");

        Locked = false;
    }

    private void Start() {
        if (isLocalPlayer) {
            LocalPlayer = this;
            
        }

        GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().enabled = false;

        GameObject model = this.transform.Find(isServer ? "Model2" : "Model").gameObject;
        model.SetActive(false);

        PlayerNumber = transform.SetLayerByRegion();
    }

    void Update() {
        // animate the player speed
        GameObject model = this.transform.Find(isServer ? "Model" : "Model2").gameObject; 
        if(model != null)
        {
            float speed = this.GetComponent<Rigidbody>().velocity.magnitude;
            model.GetComponent<Animator>().SetFloat("speed", speed / 4);
        }


        if (Locked || !isLocalPlayer)
            return;

        Vector3 pos3 = transform.position;
        Vector2 pos = pos3.XZ();

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
            Camera cam = Camera.main;
            mousePos.z = Mathf.Abs((cam.transform.position - pos3).magnitude);
            Vector3 mousePosWorld = cam.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(
                pos3.z - mousePosWorld.z,
                mousePosWorld.x - pos.x
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

        // InteractiveComponent[] interactives = FindObjectsOfType<InteractiveComponent>();
        Collider[] interactives = Physics.OverlapSphere(pos3, UsageRadius);
        closestDist = UsageRadius * UsageRadius;
        closest = null;
        for (int i = 0; i < interactives.Length; i++) {
            InteractiveComponent interactive = interactives[i].GetComponent<InteractiveComponent>();
            if (interactive == null)
                interactive = interactives[i].GetComponentInParent<InteractiveComponent>();
            if (interactive == null)
                continue;

            string layer = LayerMask.LayerToName(interactive.gameObject.layer);
            if (PlayerNumber == 1 && (layer == "Player 2" || layer == "Items 2"))
                continue;
            if (PlayerNumber == 2 && (layer == "Player 1" || layer == "Items 1"))
                continue;

            Vector3 interactivePos3 = interactive.transform.position;
            Vector2 interactivePos = interactivePos3.XZ();

            float distToCenter = (pos - interactivePos).sqrMagnitude;

            Vector2 delta = interactivePos - pos;
            float angle = Mathf.Atan2(
                delta.y,
                delta.x
            ) * Mathf.Rad2Deg;
            angle -= lookRayAngle;
            if (angle < -180f)
                angle += 360f;
            // Debug.Log(interactive.name + " " + angle);
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
            Tooltip.ShowText(closest, closest.DisplayName);
            closestDist = Mathf.Pow(closestDist, 0.5f);
            if (use && !drop) {
                if (Item != null && closest.GetComponent<ItemComponent>() == null) {
                    UseItemWith(Item, closest);
                } else {
                    Interact();
                }
            }
        } else if (use && !drop) {
            UseItem();
        }

        if (drop) {
            Debug.Log("DROP PRESSED");
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

        CmdPlayerInteract(this.gameObject, interactive.gameObject);
    }

    [Command]
    public void CmdPlayerInteract(GameObject player, GameObject interac)
    {
        RpcInteract(interac);
    }

    [ClientRpc]
    public void RpcInteract(GameObject other)
    {
        InteractiveComponent interactive = other.GetComponent<InteractiveComponent>();
       
        ItemComponent item = interactive.GetComponent<ItemComponent>();
        if (item != null) {
            // Don't run minigames when picking up an item.
            interactive.OnInteract.Invoke(this);
            PickupItem(item);
            return;
        }

        MinigameBase minigame = interactive.GetComponent<MinigameBase>();
        if (minigame == null)
            interactive.OnInteract.Invoke(this);
        else
            minigame.StartMinigame(this, null);
    }

    /// <summary>
    /// Pick up passed item or closest item.
    /// </summary>
    public void PickupItem(ItemComponent item = null) {
        if (item == null && closest != null)
            item = closest.GetComponent<ItemComponent>();
        if (item == null)
            return;

        CmdPlayerPickUp(this.gameObject, item.gameObject);
    }

    [Command]
    public void CmdPlayerPickUp(GameObject player, GameObject item)
    {
        RpcPickupItem(item);
    }

    [ClientRpc]
    public void RpcPickupItem(GameObject other)
    {
        LocalPickupItem(other.GetComponent<ItemComponent>());
    }

    public void LocalPickupItem(ItemComponent item) {
        ItemComponent prevItem = Item;

        if (prevItem != null) {
            LocalDropItem(); // Drop any previous items.
            // Put the prev item at the replacing item's pos.
            prevItem.transform.position = item.transform.position;
        }

        Item = item;

        Item.Holder = this;
        Item.transform.parent = HoldingPoint;
        Item.transform.localPosition = Item.HoldOffset;
        Item.transform.localEulerAngles = Item.HoldRotation;

        Collider collider = Item.GetComponentInChildren<Collider>();
        if (collider != null) {
            collider.enabled = false;
            Rigidbody body = Item.GetComponent<Rigidbody>();
            if (body != null)
                Destroy(body);
        }

        NetworkTransform ntrans = Item.GetComponent<NetworkTransform>();
        if (ntrans != null) {
            ntrans.enabled = false;
        }

        item.OnPickup.Invoke();

        item.transform.SetLayerDeep(LayerMask.NameToLayer("Player " + PlayerNumber));
    }

    /// <summary>
    /// Drop currently held item.
    /// </summary>
    public void DropItem() {
        Debug.Log("DROPPING");
        CmdDropItem(this.gameObject);
    }

    [Command]
    public void CmdDropItem(GameObject player)
    {
        RpcDropItem();
    }

    [ClientRpc]
    public void RpcDropItem() {
        LocalDropItem();
    }

    public void LocalDropItem() { 
        if (Item == null)
            return;

        Item.OnDrop.Invoke();

        Item.Holder = null;
        Item.transform.parent = null;
        Item.transform.position = new Vector3(
            Item.transform.position.x,
            2f,
            Item.transform.position.z
        );

        NetworkTransform ntrans = Item.GetComponent<NetworkTransform>();
        if(ntrans != null)
        {
            ntrans.enabled = true;
        }

        Collider collider = Item.GetComponentInChildren<Collider>();
        if (collider != null) {
            collider.enabled = true;
            Item.gameObject.AddComponent<Rigidbody>();
        }

        Item.transform.SetLayerDeep(LayerMask.NameToLayer("Items " + PlayerNumber));

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

        UnityAction callback = () => {
            CmdUseItem(this.gameObject, item.gameObject);
        };

        MinigameBase minigame = item.GetComponent<MinigameBase>();
        if (minigame == null)
            callback();
        else {
            minigame.Callback = callback;
            minigame.StartMinigame(this, null);
        }
    }

    [Command]
    public void CmdUseItem(GameObject player, GameObject item)
    {
        RpcUseItem(item);
    }

    [ClientRpc]
    public void RpcUseItem(GameObject other)
    {
        ItemComponent item = other.GetComponent<ItemComponent>();
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

        UnityAction callback = () => {
            CmdUseItemWith(this.gameObject, item.gameObject, with.gameObject);
        };

        MinigameBase minigame = item.GetComponent<MinigameBase>();
        if (minigame == null)
            minigame = with.GetComponent<MinigameBase>();

        if (minigame == null)
            callback();
        else {
            minigame.Callback = callback;
            minigame.StartMinigame(this, with);
        }
    }

    [Command]
    public void CmdUseItemWith(GameObject player, GameObject item, GameObject with)
    {
        RpcUseItemWith(item, with);
    }

    [ClientRpc]
    public void RpcUseItemWith(GameObject itemObj, GameObject withObj) {
        ItemComponent item = itemObj.GetComponent<ItemComponent>();
        InteractiveComponent with = withObj.GetComponent<InteractiveComponent>();

        if (item.CanUseWith != null ? item.CanUseWith(this, with) : true)
            item.OnUseWith.Invoke(with);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, UsageRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * UsageRadius);
        Gizmos.color = Color.yellow;
        float ground = 0.3f;
        Gizmos.DrawLine(
            new Vector3(
                transform.position.x,
                ground,
                transform.position.z
            ),
            new Vector3(
                transform.position.x,
                ground,
                transform.position.z
            ) + transform.right * UsageRadius);
        if (closest != null) {
            Vector3 posGrounded = new Vector3(
                transform.position.x,
                ground,
                transform.position.z
            );
            Vector3 interactivePosGrounded = new Vector3(
                closest.transform.position.x,
                ground,
                closest.transform.position.z
            );
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(posGrounded, posGrounded + (interactivePosGrounded - posGrounded).normalized * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closestDist);
            Gizmos.DrawWireCube(closest.transform.position, closest.transform.lossyScale);
        }
    }

}

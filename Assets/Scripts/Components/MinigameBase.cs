﻿using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractiveComponent))]
public abstract class MinigameBase : MonoBehaviour {

    protected RectTransform PlayerUIRoot;
    protected PlayerUIController PlayerUIController;

    protected abstract string UIPath { get; }
    protected RectTransform UITree;

    protected PlayerController Player;

    public bool Win { get; protected set; }
    public bool Active { get; protected set; }
    private bool Activating;

    public UnityAction Callback;

    public InteractiveComponent interactive { get; private set; }
    protected virtual void Awake() {
        interactive = GetComponent<InteractiveComponent>();
    }

    public void StartMinigame(PlayerController player, InteractiveComponent with) {
        // TODO: If network player, return.

        if (Active || Activating)
            return;

        PlayerUIRoot = player.transform.Find("PlayerUIRoot").GetComponent<RectTransform>();
        PlayerUIController = PlayerUIRoot.GetComponent<PlayerUIController>();

        if (interactive.CanMinigame != null && !interactive.CanMinigame(player, with)) {
            if (Callback != null)
                Callback();
            else
                interactive.OnInteract.Invoke(Player);
            Callback = null;
            return;
        }

        UITree = Instantiate(Resources.Load<GameObject>("Minigames/" + UIPath), PlayerUIRoot).GetComponent<RectTransform>();
        UITree.anchorMin = Vector2.zero;
        UITree.anchorMax = Vector2.one;
        UITree.sizeDelta = Vector2.zero;

        Player = player;

        Win = false;
        Activating = true;
        Player.Locked = true;
        PlayerUIController.ShowMinigame();
        _StartMinigame();
        StartCoroutine(DelayedActivate());
    }

    public void CancelMinigame() {
        // TODO: If network player, return.

        _EndMinigame();
        Win = false;
        StartCoroutine(DelayedDeactivate());

        PlayerUIController.ShowMinigame();
        Destroy(UITree.gameObject);
        Player.Locked = false;
    }

    public void EndMinigame() {
        // TODO: If network player, return.

        if (!Active)
            return;

        _EndMinigame();
        StartCoroutine(DelayedDeactivate());

        if (Win) {
            ScreenShakeController.Instance.Trigger(Camera.main.transform, 0.1f, 0.2f);
            if (Callback != null)
                Callback();
            else
                interactive.OnInteract.Invoke(Player);
            Callback = null;
        }

        PlayerUIController.HideMinigame();
        Destroy(UITree.gameObject);
        Player.Locked = false;
    }

    private IEnumerator DelayedActivate() {
        yield return new WaitForEndOfFrame();
        Activating = false;
        Active = true;
    }

    private IEnumerator DelayedDeactivate() {
        yield return new WaitForEndOfFrame();
        Active = false;
    }

    protected abstract void _StartMinigame();
    protected abstract void _EndMinigame();

}

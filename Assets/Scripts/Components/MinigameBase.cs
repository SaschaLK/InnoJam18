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

    public UnityAction Callback;

    public InteractiveComponent interactive { get; private set; }
    protected virtual void Awake() {
        interactive = GetComponent<InteractiveComponent>();
        if (PlayerUIRoot == null)
            PlayerUIRoot = GameObject.Find("PlayerUIRoot").GetComponent<RectTransform>();
        if (PlayerUIController == null)
            PlayerUIController = PlayerUIRoot.GetComponent<PlayerUIController>();
    }

    public void StartMinigame(PlayerController player) {
        // TODO: If network player, return.

        if (interactive.CanMinigame != null && !interactive.CanMinigame(player)) {
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
        Active = true;
        Player.Locked = true;
        PlayerUIController.ShowMinigame();
        _StartMinigame();
    }

    public void CancelMinigame() {
        // TODO: If network player, return.

        _EndMinigame();
        Win = false;
        Active = false;

        PlayerUIController.ShowMinigame();
        Destroy(UITree.gameObject);
        Player.Locked = false;
    }

    public void EndMinigame() {
        // TODO: If network player, return.

        if (!Active)
            return;

        _EndMinigame();
        Active = false;

        if (Win) {
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

    protected abstract void _StartMinigame();
    protected abstract void _EndMinigame();

}
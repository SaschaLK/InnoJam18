using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePump : MinigameBase {

    protected override string UIPath {
        get {
            return "Pump";
        }
    }

    protected Text CountdownText;
    protected Image ImageLeft;
    protected Image ImageRight;

    protected Sprite SpriteLeft;
    protected Sprite SpriteLeftDown;
    protected Sprite SpriteRight;
    protected Sprite SpriteRightDown;

    protected int Times;
    protected float Timeout;
    protected bool Right;

    protected override void Awake() {
        base.Awake();

    }

    protected override void _StartMinigame() {
        CountdownText = UITree.Find("CountdownText").GetComponent<Text>();
        ImageLeft = UITree.Find("ImageLeft").GetComponent<Image>();
        ImageRight = UITree.Find("ImageRight").GetComponent<Image>();

        Times = Random.Range(10, 20);
        Timeout = Time.time + 2f;
        Right = Random.value > 0.5f;

        SpriteLeft = ImageLeft.sprite;
        if (SpriteLeftDown == null)
            SpriteLeftDown = Resources.Load<Sprite>("UIKeys/" + SpriteLeft.name + "_Down");
        SpriteRight = ImageRight.sprite;
        if (SpriteRightDown == null)
            SpriteRightDown = Resources.Load<Sprite>("UIKeys/" + SpriteRight.name + "_Down");

        UpdateSprites();
    }

    private void UpdateSprites() {
        if (Right) {
            ImageLeft.sprite = SpriteLeft;
            ImageLeft.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            ImageRight.sprite = SpriteRightDown;
            ImageRight.color = new Color(1f, 1f, 1f, 1f);
        } else {
            ImageLeft.sprite = SpriteLeftDown;
            ImageLeft.color = new Color(1f, 1f, 1f, 1f);
            ImageRight.sprite = SpriteRight;
            ImageRight.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

    private void Update() {
        if (!Active)
            return;

        if ((Right  && Input.GetButtonDown("FireLB")) ||
            (!Right && Input.GetButtonDown("FireRB"))) {
            CancelMinigame();
            return;
        }

        if ((Right  && Input.GetButtonDown("FireRB")) ||
            (!Right && Input.GetButtonDown("FireLB"))) {
            Right = !Right;
            Timeout = Time.time + 0.3f;
            Times--;
            UpdateSprites();
        }

        if (Times < 0)
            Times = 0;
        CountdownText.text = Times.ToString();

        if (Times <= 0) {
            Times = 0;
            EndMinigame();
            return;
        }

        if (Time.time >= Timeout) {
            CancelMinigame();
            return;
        }

    }

    protected override void _EndMinigame() {
        if (!Active)
            return;

        Win = Times <= 0;

    }

}

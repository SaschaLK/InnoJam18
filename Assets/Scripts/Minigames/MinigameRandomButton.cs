using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameRandomButton : MinigameBase {

    protected override string UIPath {
        get {
            return "RandomButton";
        }
    }

    protected readonly static string[] Keys = {
        "Fire1",
        "Fire2",
        "Fire3",
        "Fire4"
    };

    protected Text CountdownText;
    protected Image ButtonImage;

    protected int Times;
    protected float Timeout;
    protected string Key;

    protected override void Awake() {
        base.Awake(); // Wichtig!

    }

    protected override void _StartMinigame() {
        CountdownText = UITree.Find("CountdownText").GetComponent<Text>();
        ButtonImage = UITree.Find("ButtonImage").GetComponent<Image>();

        Times = Random.Range(5, 8);

        Times++;
        NextKey();
    }

    private void NextKey() {
        Times--;
        if (Times <= 0) {
            Times = 0;
            CountdownText.text = "0";
            return;
        }
        CountdownText.text = Times.ToString();

        Timeout = Time.time + 2f;

        Key = Keys[Random.Range(0, Keys.Length)];
        ButtonImage.sprite = Resources.Load<Sprite>("UIKeys/" + Key);
    }

    private void Update() {
        if (!Active)
            return;

        if (Time.time >= Timeout) {
            CancelMinigame();
            return;
        }

        for (var i = 0; i < Keys.Length; i++) {
            if (Keys[i] != Key && Input.GetButtonDown(Keys[i])) {
                CancelMinigame();
                return;
            }
        }

        if (Input.GetButtonDown(Key)) {
            NextKey();
        }

        if (Times <= 0) {
            Times = 0;
            EndMinigame();
            return;
        }
    }

    protected override void _EndMinigame() {
        if (!Active)
            return;

        Win = Times <= 0f;
    }

}

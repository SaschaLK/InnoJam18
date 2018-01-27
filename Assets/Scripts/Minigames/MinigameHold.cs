using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHold : MinigameBase {

    protected override string UIPath {
        get {
            return "Hold"; // Assets/Resources/Minigames/<PrefabName>
        }
    }

    protected readonly static string[] Keys = {
        "Fire1",
        "Fire2",
        "Fire3",
        "Fire4"
    };

    // UI
    protected Text TimerText;
    protected Image ButtonImage;

    // Variablen
    protected string Key;
    protected float TimeHeldDown;
    protected bool Holding;
    protected float Timeout;

    protected override void Awake() {
        base.Awake(); // Wichtig!

    }

    protected override void _StartMinigame() {
        // Bereite hier alles vor.
        // Setze alles zurück, bereite UI vor, ...

        // Finde alles aus dem UI-Baum.
        TimerText = UITree.Find("TimerText").GetComponent<Text>();
        ButtonImage = UITree.Find("ButtonImage").GetComponent<Image>();

        // Wir generieren hier z.B. die Zeit, wie lange der Spieler A gedrückt halten muss.
        Key = Keys[Random.Range(0, Keys.Length)];
        TimeHeldDown = Random.Range(1f, 3f);
        // Wir wollen nicht, dass der Spieler sofort failt -
        // stattdessen wollen wir warten, bis er anfängt, A zu drücken.
        Holding = false;

        // Irgendeine Zeit in der Zukunft, damit diese Fail-Condition nicht aus Versehen ausgelöst wird.
        Timeout = Time.time + 2f;

        ButtonImage.sprite = Resources.Load<Sprite>("UIKeys/" + Key);
    }

    private void Update() {
        if (!Active)
            return;

        Holding |= Input.GetButtonDown(Key); // Der Spieler hat angefangen, A zu halten.
        if (!Holding && Input.GetButtonUp(Key)) {
            // Der Spieler hat Fire1 sofort losgelassen.
            // Fail nach einer kurzen Auszeit.
            Timeout = Time.time + 0.3f;
        }

        if (!Holding && Time.time >= Timeout) {
            // Wenn der Player 0.3 Sekunden nach dem sofortigen Loslassen nicht drückt,
            // failen wir sofort.
            CancelMinigame();
            return;
        }

        if (Input.GetButton(Key)) {
            TimeHeldDown -= Time.deltaTime;
        } else if (Holding) {
            // Der Spieler hält A nicht mehr gedrückt, hat es aber mal gehalten. Fail!
            CancelMinigame();
            return;
        }

        // Wir vermeiden < 0 bugs.
        if (TimeHeldDown < 0f)
            TimeHeldDown = 0f;
        TimerText.text = TimeHeldDown.ToString("N2").Replace(',', '.');

        if (TimeHeldDown <= 0f) {
            // Wir haben die Zielzeit erreicht - Beende das Minigame.
            // Achtung! Wir beenden das Minigame via EndMinigame,
            // aber implementieren alles wichtige in _EndMinigame.
            EndMinigame();
        }
    }

    protected override void _EndMinigame() {
        if (!Active)
            return; // Wir haben das Minigame bereits beendet.

        // Wir bestimmen nun, ob der Player das Minigame gewonnen hat.
        // Theoretisch können wir es auch in Update bestimmen - who cares.
        Win = TimeHeldDown <= 0f;
    }

}

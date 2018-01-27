using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTest : MinigameBase {

    protected override string UIPath {
        get {
            return "Test"; // Assets/Resources/Minigames/<PrefabNameHier>
        }
    }

    // UI
    protected Text TimerText;

    // Variablen
    protected float TimeHeldDown;
    protected bool Holding;
    protected float PressedUpTime;

    protected override void Awake() {
        base.Awake(); // Wichtig!

    }

    protected override void _StartMinigame() {
        // Bereite hier alles vor.
        // Setze alles zurück, bereite UI vor, ...

        // Finde alles aus dem UI-Baum.
        TimerText = UITree.Find("TimerText").GetComponent<Text>();

        // Wir generieren hier z.B. die Zeit, wie lange der Spieler A gedrückt halten muss.
        TimeHeldDown = Random.Range(3f, 5f);
        // Wir wollen nicht, dass der Spieler sofort failt -
        // stattdessen wollen wir warten, bis er anfängt, A zu drücken.
        Holding = false;

        // Irgendeine Zeit in der Zukunft, damit diese Fail-Condition nicht aus Versehen ausgelöst wird.
        PressedUpTime = Time.time + TimeHeldDown + 1000f;
    }

    private void Update() {
        if (!Active)
            return;

        Holding |= Input.GetButtonDown("Fire1"); // Der Spieler hat angefangen, A zu halten.
        if (!Holding && Input.GetButtonUp("Fire1")) {
            // Der Spieler hat Fire1 sofort losgelassen.
            // Fail nach einer kurzen Auszeit.
            PressedUpTime = Time.time;
        }

        if (!Holding && Time.time >= PressedUpTime + 0.3f) {
            // Wenn der Player 0.3 Sekunden nach dem sofortigen Loslassen nicht drückt,
            // failen wir sofort.
            EndMinigame();
            return;
        }

        if (Input.GetButton("Fire1")) {
            TimeHeldDown -= Time.deltaTime;
        } else if (Holding) {
            // Der Spieler hält A nicht mehr gedrückt, hat es aber mal gehalten. Fail!
            EndMinigame();
            return;
        }

        // Wir vermeiden < 0 bugs.
        if (TimeHeldDown <= 0f) {
            TimeHeldDown = 0f;
        }
        TimerText.text = TimeHeldDown.ToString("N1").Replace(',', '.');

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

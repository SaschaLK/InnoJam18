using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

    public AirplaneFall(ControlLamp lamp, float shiftDistance)
    {
        Debug.Log("airplane is falling");

        AirplaneFallCameraScript camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AirplaneFallCameraScript>();
        camScript.currentLerpTime = 0f;

        lamp.OnActivation.Invoke();
    }
}

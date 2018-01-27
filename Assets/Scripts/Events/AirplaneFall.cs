﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

	public AirplaneFall(ControlLamp lamp)
    {
        if (lamp == null) return;
        Debug.Log("airplane is falling");

        lamp.OnActivation.Invoke();
    }
}

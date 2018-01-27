using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

	public AirplaneFall(ControlLamp lamp)
    {
        if (lamp == null) return;
        lamp.OnActivation.Invoke();
    }
}

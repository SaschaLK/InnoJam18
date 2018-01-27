using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFall : GameEvent {

	public AirplaneFall(ControlLamp lamp)
    {
        lamp.OnActivation.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLamp : MonoBehaviour {

    /// <summary>
    /// Invoked by GameButton.PressButton
    /// </summary>
    public LampActivationEvent OnActivation = new LampActivationEvent();
}

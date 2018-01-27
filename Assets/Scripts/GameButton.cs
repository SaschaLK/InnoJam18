using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour {

    [SerializeField]
    ControlLamp lamp;

	public void PressButton()
    {
        Debug.Log(lamp + "activated");
        lamp.OnActivation.Invoke();
    }
}

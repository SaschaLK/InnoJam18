using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveComponent : MonoBehaviour {

    /// <summary>
    /// Invoked by PlayerController.Interact
    /// </summary>
    public UnityEvent OnInteract = new UnityEvent();

}

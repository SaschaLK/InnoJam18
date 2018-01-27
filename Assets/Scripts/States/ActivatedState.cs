using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedState : StationState
{

    InteractiveComponent interactive;

    private void Awake()
    {
        interactive = GetComponentInParent<InteractiveComponent>();
    }
}

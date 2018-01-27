using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent {
   
    public UnityEvent OnEventStart = new UnityEvent();

    public UnityEvent OnFailed = new UnityEvent();

    public UnityEvent OnSuccess = new UnityEvent();
    
}

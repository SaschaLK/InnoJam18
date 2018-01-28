﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SceneController : NetworkBehaviour {

    public static SceneController instance;

    private Camera gameCamera;

    public Airplane airplane;
    
    [SerializeField]
    ControlLamp sinkingLamp;

    [SerializeField]
    ControlLamp evadeLeftLamp;

    [SerializeField]
    ControlLamp evadeRightLamp;

    private float minEventTime = 5f;
    private float maxEventTime = 12f;

    private float timer;
    private float timeEvent;
    

    public bool evade = true;

    public List<GameEvent> currentEvents;

    private void Awake()
    {
        instance = this;
        this.gameCamera = GetComponent<Camera>();
        currentEvents = new List<GameEvent>();
    }

    void Start(){
        if (!isServer) return;
        Debug.Log("START");
        timeEvent = Random.Range(minEventTime, maxEventTime);
    }

    private void Update()
    {
        if (!isServer) return;

        timer += Time.deltaTime;
        Debug.Log("UPDATE");
        float difficultyIncrease = 0;

        // increase difficulty every 10 seconds
        if (Mathf.FloorToInt(Time.timeSinceLevelLoad) % 10 == 0) {
            difficultyIncrease++;
        }

        if (timer >= timeEvent) {
            timer = 0;
            timeEvent = Random.Range(Mathf.Min(minEventTime - difficultyIncrease/2 , maxEventTime - difficultyIncrease) , Mathf.Max(minEventTime - difficultyIncrease / 2, maxEventTime - difficultyIncrease));
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()   {
        if (!isServer) return;
        Debug.Log("TRIGGER RANDOM");
        float rand = Random.Range(0, 100);
        if(rand <= 10) {
            CmdTriggerTurbulenceEvent();

        } else if(rand <= 30) {
            //DO NOTHING FOR NOW

        } else if (rand <= 50) {
            CmdTriggerEnemyApproach();

        } else if(rand <= 70) {
            CmdTriggerAirplaneFall();

        } else {
            if(Random.value <= 0.5f) {
                CmdTriggerEvadeRight();
            } else {
                CmdTriggerEvadeLeft();

            }
        }
    }

    [Command]
    public void CmdTriggerTurbulenceEvent()
    {
        RpcTriggerTurbulenceEvent();
    }

    [ClientRpc]
    public void RpcTriggerTurbulenceEvent()
    {
        GameEvent evt = new TurbulenceEvent(this.GetComponent<TurbulenceEventHandler>());
        if (isServer) currentEvents.Add(evt);
        evt.OnEventStart.Invoke();
    }

    [Command]
    public void CmdTriggerAirplaneFall()
    {
        RpcTriggerAirplaneFall();
    }

    [ClientRpc]
    public void RpcTriggerAirplaneFall()
    {
        GameEvent evt = new AirplaneFall(this.GetComponent<AirplaneFallHandler>());
        if (isServer) currentEvents.Add(evt);
        evt.OnEventStart.Invoke();
    }

    [Command]
    public void CmdTriggerEvadeLeft()
    {
        RpcTriggerEvadeLeft();
    }

    [ClientRpc]
    public void RpcTriggerEvadeLeft()
    {
        GameEvent evt = new EvadeLeft(this.GetComponent<EvadeLeftHandler>());
        if (isServer) currentEvents.Add(evt);
        evt.OnEventStart.Invoke();
    }

    [Command]
    public void CmdTriggerEvadeRight()
    {
        RpcTriggerEvadeRight();
    }

    [ClientRpc]
    public void RpcTriggerEvadeRight()
    {
        GameEvent evt = new EvadeRight(this.GetComponent<EvadeRightHandler>());
        if (isServer) currentEvents.Add(evt);
        evt.OnEventStart.Invoke();
    }

    [Command]
    public void CmdTriggerEnemyApproach()
    {
        RpcTriggerEnemyApproach();
    }

    [ClientRpc]
    public void RpcTriggerEnemyApproach()
    {
        GameEvent evt = new EnemyApproach(this.GetComponent<EnemyApproachHandler>());
        if(isServer) currentEvents.Add(evt);
        evt.OnEventStart.Invoke();
    }

    #region EvadeFunctions
    public void EvadeFunction()
    {
        evade = true;
    }

    public void StartHitCountdown(float counter)
    {
        evade = false;
        StartCoroutine(EvadeCheck(counter));
    }

    private IEnumerator EvadeCheck(float counter)
    {
        yield return new WaitForSeconds(counter);
        if(evade == false)
        {
            //TODO: Hit the thing
            Debug.Log("Evade too late... CRASH!!");
        }
    }

    #endregion
}

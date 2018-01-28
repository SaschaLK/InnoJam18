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

    // this is ugly and dirty code and nobody will ever find it brua!
    // working for two players pressing evade buttons within 3 secs
    private int playersPressingNow = 0;
    public void TriggerEvadeButtonPressOn()
    {
        playersPressingNow++;
        if(playersPressingNow >= 2) {
            // check if there is an open evade event
            foreach(GameEvent ev in currentEvents) {
                if (ev is Evade) {
                    Evade evad = (Evade)ev;
                    evad.TriggerOnComplete();
                }
            }
        }
        Invoke("TriggerEvadeButtonPressOff", BalancingConstant.EVADE_BUTTONOVERLAPTIME);
    }
    private void TriggerEvadeButtonPressOff() { playersPressingNow--; }

    void Start(){
        if (!isServer) return;
        Debug.Log("START");
        timeEvent = Random.Range(minEventTime, maxEventTime);
    }

    private void Update()
    {
        if (!isServer) return;

        timer += Time.deltaTime;
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
        CmdTriggerEvade();
        return;

        //TODO: DEBUG
        float rand = Random.Range(0, 100);
        if (rand <= 10) {
            CmdTriggerTurbulenceEvent();
            return;

        } else if(rand <= 30) {
            //DO NOTHING FOR NOW
            return;
        } else if (rand <= 50) {
            CmdTriggerEnemyApproach();
            return;
        } else if(rand <= 70) {
            CmdTriggerAirplaneFall();
            return;
        } else {
            CmdTriggerEvade();
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
    public void CmdTriggerEvade()
    {
        RpcTriggerEvade();
    }

    [ClientRpc]
    public void RpcTriggerEvade()
    {
        GameEvent ev = new Evade(this.GetComponent<EvadeHandler>());
        if (isServer) currentEvents.Add(ev);
        ev.OnEventStart.Invoke();
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
}

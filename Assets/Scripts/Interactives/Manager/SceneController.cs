﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SceneController : NetworkBehaviour {

    public static SceneController instance;

    private Camera gameCamera;

    [SerializeField]
	Airplane airplane;

    [SerializeField]
    ControlLamp sinkingLamp;

    [SerializeField]
    ControlLamp evadeLeftLamp;

    [SerializeField]
    ControlLamp evadeRightLamp;

    private float minEventTime = 10f;
    private float maxEventTime = 30f;

    private float timer;
    private float timeEvent;

    public bool evade = true;

    private void Awake()
    {
        if (!isServer) return;
        instance = this;
        this.gameCamera = GetComponent<Camera>();
    }

    void Start(){
        if (!isServer) return;

        timeEvent = Random.Range(minEventTime, maxEventTime);
        EnemyApproach ae = new EnemyApproach(this.GetComponent<EnemyApproachHandler>());
        ae.OnEventStart.Invoke();
    }

    private void Update()
    {
        if (!isServer) return;

        timer += Time.deltaTime;

        float difficultyIncrease = 0;

        // increase difficulty every 10 seconds
        if(Mathf.FloorToInt(Time.timeSinceLevelLoad) % 10 == 0)
        {
            difficultyIncrease++;
        }

        if (timer >= timeEvent)
        {
            timer = 0;
            timeEvent = Random.Range(Mathf.Min(minEventTime - difficultyIncrease/2 , maxEventTime - difficultyIncrease) , Mathf.Max(minEventTime - difficultyIncrease / 2, maxEventTime - difficultyIncrease));
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()   {
        if (!isServer) return;
        float rand = Random.Range(0, 100);

        if(rand <= 10)
        {
            CmdTriggerGameEvent(new TurbulenceEvent(airplane));
            return;
        }
        else if(rand <= 30)
        {
            CmdTriggerGameEvent(new EnemyAttack(airplane, gameCamera));
            return;
        }
        else if (rand <= 50)
        {
            CmdTriggerGameEvent(new EnemyApproach(this.GetComponent<EnemyApproachHandler>()));
            ae.OnEventStart.Invoke();
            return;
        }
        else if(rand <= 70)
        {
            CmdTriggerGameEvent(new AirplaneFall(sinkingLamp));
            return;
        }
        else
        {
            rand = Random.value;
            if(rand <= 0.5f)
            {
                CmdTriggerGameEvent(new EvadeLeft(evadeLeftLamp, 10f));
            }
            else
            {
                CmdTriggerGameEvent(new EvadeRight(evadeRightLamp, 10f));
            }
        }
    }

    [Command]
    public void CmdTriggerGameEvent(GameEvent evt)
    {
        RpcTriggerGameEvent(evt);
    }

    [ClientRpc]
    public void RpcTriggerGameEvent(GameEvent evt)
    {
        evt.triggerStart();
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

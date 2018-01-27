﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    public static SceneController instance;

    private Camera gameCamera;

    public Airplane airplane;

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

    public List<GameEvent> currentEvents;

    private void Awake()
    {
        instance = this;
        this.gameCamera = GetComponent<Camera>();
    }

    void Start(){
        timeEvent = Random.Range(minEventTime, maxEventTime);
    }

    private void Update()
    {
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

    private void TriggerRandomEvent()
    {
        float rand = Random.Range(0, 100);

        if(rand <= 10)
        {
            TurbulenceEvent te = new TurbulenceEvent(this.GetComponent<TurbulenceEventHandler>());
            te.OnEventStart.Invoke();
            //TurbulenceEvent te = new TurbulenceEvent(airplane);
            return;
        }
        else if(rand <= 30)
        {
            //DO NOTHING FOR NOW
            return;
        }
        else if (rand <= 50)
        {
            EnemyApproach ae = new EnemyApproach(this.GetComponent<EnemyApproachHandler>());
            ae.OnEventStart.Invoke();
            return;
        }
        else if(rand <= 70)
        {
            AirplaneFall af = new AirplaneFall(this.GetComponent<AirplaneFallHandler>());
            af.OnEventStart.Invoke();
            return;
        }
        else
        {
            rand = Random.value;
            if(rand <= 0.5f)
            {
                EvadeLeft el = new EvadeLeft(this.GetComponent<EvadeLeftHandler>());
                el.OnEventStart.Invoke();
                //EvadeLeft el = new EvadeLeft(evadeLeftLamp, 10f);
            }
            else
            {
                EvadeRight er = new EvadeRight(this.GetComponent<EvadeRightHandler>());
                er.OnEventStart.Invoke();
                //EvadeRight er = new EvadeRight(evadeRightLamp, 10f);
            }
        }
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

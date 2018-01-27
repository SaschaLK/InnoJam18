﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

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


    private float timer;
    private float timeEvent;

    public bool evade = true;

    private void Awake()
    {
        instance = this;
        this.gameCamera = GetComponent<Camera>();
    }

    void Start(){
        timeEvent = Random.Range(5f, 20f);
        ScreenShakeController.Instance.Trigger(5f, 0.2f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeEvent)
        {
            timer = 0;
            timeEvent = Random.Range(5f, 20f);
            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        float rand = Random.Range(0, 100);

        if(rand <= 10)
        {
            TurbulenceEvent te = new TurbulenceEvent(airplane);
            return;
        }
        else if(rand <= 30)
        {
            EnemyAttack ea = new EnemyAttack(airplane, gameCamera);
            return;
        }
        else if (rand <= 50)
        {
            EnemyApproach enemyApproach = new EnemyApproach();
            return;
        }
        else if(rand <= 70)
        {
            AirplaneFall af = new AirplaneFall(sinkingLamp);
            return;
        }
        else
        {
            rand = Random.value;
            if(rand <= 0.5f)
            {
                EvadeLeft el = new EvadeLeft(evadeLeftLamp, 10f);
            }
            else
            {
                EvadeRight er = new EvadeRight(evadeRightLamp, 10f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    [SerializeField]
    Camera camera;

    [SerializeField]
	Airplane airplane;

	[SerializeField]
	GameObject enemyAttackLamp;

    private float timer;
    private float timeEvent;

	void Start(){
        timeEvent = Random.Range(5f, 20f);
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
        int rand = Random.Range(0, 100);

        if(rand <= 10)
        {
            TurbulenceEvent te = new TurbulenceEvent(airplane);
            return;
        }
        else if(rand <= 30)
        {
            EnemyAttack ea = new EnemyAttack(airplane, camera);
            return;
        }
        else if (rand <= 50)
        {
            EnemyApproach enemyApproach = new EnemyApproach(enemyAttackLamp);
            return;
        }
        else if(rand <= 70)
        {
            AirplaneFall af = new AirplaneFall();
        }
    } 
}

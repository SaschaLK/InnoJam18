using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyApproachHandler : NetworkBehaviour {

    Airplane airplane;
    public float timeToFail = BalancingConstant.ENEMYAPPROACH_TIME;

    private int invokeIterations = 0;
    private bool success = false;

    public EnemyApproach ea;

    public AudioSource sirenAudio;

    public void BindEvent(EnemyApproach ea)
    {
        this.ea = ea;
        airplane = SceneController.instance.airplane;
    }
    
    public void OnEventStart()
    {
        if (airplane == null) return;
        Debug.Log("enemy is approaching");
        
        if (isServer) {
            success = false;
            invokeIterations = BalancingConstant.ENEMYAPPROACH_ATTACKS - 1;
            Invoke("CmdAttackShip", timeToFail / BalancingConstant.ENEMYAPPROACH_ATTACKS);
        }

        sirenAudio.volume = 1;
        airplane.StartEnemyLamps();
    }

    [Command]
    private void CmdAttackShip() {
        if (success) return; // dont do damage after the players were successful

        List<InteractiveComponent> stations = airplane.stations;

        if (stations.Count == 0) return;

        int rand = Random.Range(0, stations.Count);

        // only kill stations that are not dead already
        for(int i = 0; i < stations.Count; i++) {
            if(!stations[rand].HealthLargerThan(0)) {
                rand = (rand+1) % stations.Count;
            } else {
                stations[rand].TakeFatalDamage();
                break;
            }
        }
         
        invokeIterations--;
        if (invokeIterations > 0) {
            Invoke("CmdAttackShip", timeToFail / BalancingConstant.ENEMYAPPROACH_ATTACKS);
        } else {
            ea.OnFailed.Invoke();
        }
    }

    private void stopAttackVisuals()
    {
        success = true;
        SceneController.instance.currentEvents.Remove(ea);
        sirenAudio.volume = 0;
        airplane.StopEnemyLamps();
    }
    
	public void EnemyEventFailed() {
        Debug.Log("Enemy Approach FAILED!");
        stopAttackVisuals();
    }

    public void EnemyEventSuccess()
    {
        Debug.Log("Enemy Approach SUCCESSFUL!");
        stopAttackVisuals();
    }
}

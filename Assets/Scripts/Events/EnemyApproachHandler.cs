using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyApproachHandler : NetworkBehaviour {

    Airplane airplane;
    public float timeToFail = 20f;

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
            invokeIterations = 3;
            Invoke("CmdAttackShip", timeToFail / 4);
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
        stations[rand].TakeFatalDamage();

        invokeIterations--;
        if(invokeIterations > 0)
            Invoke("CmdAttackShip", timeToFail / 4);
    }
    
	public void EnemyEventFailed()
    {
        SceneController.instance.currentEvents.Remove(ea);
        sirenAudio.volume = 0;
        airplane.StopEnemyLamps();
    }

    public void EnemyEventSuccess()
    {
        sirenAudio.volume = 0;
        airplane.StopEnemyLamps();
        success = true;
        SceneController.instance.currentEvents.Remove(ea);
    }
}

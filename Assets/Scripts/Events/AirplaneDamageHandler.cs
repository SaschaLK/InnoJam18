using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AirplaneDamageHandler : NetworkBehaviour {

    Airplane airplane;

    private void Awake()
    {
        airplane = GetComponent<Airplane>();
    }

    public void DoDamage(float damage)
    {
        ScreenShakeController.Instance.Trigger(Camera.main.transform, 0.5f, damage);

        if (isServer) { 
            airplane.hitPoints -= damage;
            if (airplane.hitPoints <= 0)
                GameManager.instance.EndGame();
        }
    }
}

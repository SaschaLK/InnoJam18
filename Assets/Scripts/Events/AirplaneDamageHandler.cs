using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneDamageHandler : MonoBehaviour {

    Airplane airplane;

    private void Awake()
    {
        airplane = GetComponent<Airplane>();
    }

    public void DoDamage(float damage)
    {
        airplane.hitPoints -= damage;

        ScreenShakeController.Instance.Trigger(Camera.main.transform, 0.5f, damage);

        if (airplane.hitPoints <= 0)
        {
            GameManager.instance.EndGame();
        }
    }
}

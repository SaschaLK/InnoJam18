using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLampHandler : MonoBehaviour
{
    private Light[] lights;

    private float speed = 150f;

    float lerpTime = 3f;
    float currentLerpTime;

    float minValue = 0f;
    float maxValue = 5.5f;

    bool increase = true;

    bool alarm = false;

    private void Awake()
    {
        lights = GetComponentsInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alarm)
        {
            Turn();
        }
    }

    public void StartAlarm()
    {
        alarm = true;

        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    public void StopAlarm()
    {
        alarm = false;

        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }

    private void Turn()
    {
        foreach (Light light in lights)
        {
            //light.transform.rotation.y += speed * Time.deltaTime;
            Vector3 lightRotation = light.transform.rotation.eulerAngles + new Vector3(0, speed * Time.deltaTime, 0);
            light.transform.eulerAngles = lightRotation;

            float perc;

            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = 0;
                perc = 0;
                increase = !increase;
            }

            //lerp!
            perc = currentLerpTime / lerpTime;

            if (increase)
            {
                light.intensity = Mathf.Lerp(minValue, maxValue, perc);
            }
            else
            {
                light.intensity = Mathf.Lerp(maxValue, minValue, perc);
            }
        }

    }
}

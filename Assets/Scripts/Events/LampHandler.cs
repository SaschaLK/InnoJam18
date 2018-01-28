using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampHandler : MonoBehaviour {

    private Light[] lamps;

    private void Awake()
    {
        lamps = GetComponentsInChildren<Light>();
        foreach(Light light in lamps)
        {
            light.enabled = false;
        }
    }

    public void ActivateLamp()
    {
        StartCoroutine(LampLight());
    }

    private IEnumerator LampLight()
    {
        foreach (Light light in lamps)
        {
            light.enabled = true;
        }
        yield return new WaitForSeconds(2f);
        foreach (Light light in lamps)
        {
            light.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampHandler : MonoBehaviour {

    private Light lamp;

    private void Awake()
    {
        lamp = GetComponentInChildren<Light>();
        lamp.enabled = false;
    }

    public void ActivateLamp()
    {
        StartCoroutine(LampLight());
    }

    private IEnumerator LampLight()
    {
        lamp.enabled = true;
        yield return new WaitForSeconds(2f);
        lamp.enabled = false;
    }
}

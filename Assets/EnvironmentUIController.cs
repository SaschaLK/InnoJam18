using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentUIController : MonoBehaviour {

    public GameObject blitz;

    public bool startBlitz = false;

    float timeToStart = 0f;

    bool startNextBlitz = true;

    public void toggleBlitz()
    {
        startBlitz = !startBlitz;
        blitz.SetActive(false);
    }

    private void Update()
    {
        if(startBlitz == true && blitz.activeSelf == false && startNextBlitz == true)
        {
            startNextBlitz = false;
            blitz.SetActive(true);
            StartCoroutine(SwitchBlitzOff());
        }
    }

    IEnumerator SwitchBlitzOff()
    {
        float value = Random.value;
        value = Mathf.Clamp(value, 0.1f, 0.3f);
        yield return new WaitForSeconds(value);
        blitz.SetActive(false);
        yield return new WaitForSeconds(timeToStart);
        timeToStart = Random.value * 5;
        startNextBlitz = true;
    }
}

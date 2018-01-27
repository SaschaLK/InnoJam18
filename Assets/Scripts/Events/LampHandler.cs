using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampHandler : MonoBehaviour {

    public Material matOn;
    public Material matOff;

    public void ActivateLamp()
    {
        StartCoroutine(LampLight());
    }

    private IEnumerator LampLight()
    {
        Debug.Log("start lamp");
        int count = 3;
        this.GetComponent<AudioSource>().Play();

        for (int i=0; i< count; i++)
        {
            this.GetComponent<Renderer>().material = matOn;
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<Renderer>().material = matOff;
            yield return new WaitForSeconds(0.5f);
        }
        this.GetComponent<AudioSource>().Stop();
        Debug.Log("stop lamp");
    }
}

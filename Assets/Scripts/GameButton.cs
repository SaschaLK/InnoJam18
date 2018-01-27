using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameButton : MonoBehaviour {

    [SerializeField]
    ControlLamp lamp;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PressButton()
    {
        Debug.Log(this.name + " pressed");
        lamp.OnActivation.Invoke();
        audioSource.Play();
    }
}

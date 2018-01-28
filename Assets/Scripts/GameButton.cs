using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameButton : MonoBehaviour {

    [SerializeField]
    ControlLamp lamp;

    public bool isForEvade = false;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PressButton()
    {
        Debug.Log(this.name + " pressed");

        if(isForEvade)  {
            SceneController.instance.TriggerEvadeButtonPressOn();
        } else
        {
            lamp.OnActivation.Invoke();
            audioSource.Play();
        }
    }

}

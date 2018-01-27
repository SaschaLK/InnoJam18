using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkPlayerController : PlayerController
{

    private PlayerController pc;

    // Use this for initialization
    void Start()
    {
        pc = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}

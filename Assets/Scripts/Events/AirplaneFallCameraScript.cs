using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFallCameraScript : MonoBehaviour {

    public float ShiftSpeed = 0.3f;
    public float ShiftDistance;
    public Transform LevelCenterPoint;

    Vector3 OuterPosition;
    Vector3 NeutralPosition;

    GameObject mainCam;

    float lerpTime = 1f;
    public float currentLerpTime = 1f;

    private void Awake()
    {
        mainCam = gameObject;

        NeutralPosition = mainCam.transform.position;
        OuterPosition = NeutralPosition + new Vector3(0, 0, 1) * ShiftDistance;
    }

    private void Update()
    {
        TiltPlaneDown();
    }

    private void TiltPlaneDown()
    {
        if (currentLerpTime > lerpTime)
        {
            return;
        }

        //increment timer once per frame
        currentLerpTime += Time.deltaTime * ShiftSpeed;


        //lerp!
        float t = currentLerpTime / lerpTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        mainCam.transform.position = Vector3.Lerp(NeutralPosition, OuterPosition, t);

        mainCam.transform.LookAt(LevelCenterPoint.position);
    }
}

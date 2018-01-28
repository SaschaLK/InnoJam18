using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneFallCameraScript : MonoBehaviour
{
    private float startPos;
    private float endPos;

    float lerpTime = 5f;
    private float currentLerpTime = 0f;

    bool fall = false;

    private void Update()
    {
        if (fall == true)
        {
            currentLerpTime += Time.deltaTime;
               
            float t = currentLerpTime / lerpTime;
            t = t * t;

            if(t >= 1)
            {
                t = 1;
                fall = false;
            }
            float rot = Mathf.Lerp(startPos, endPos, t);

            Vector3 newRot = new Vector3(rot, 0, 0);
            Debug.Log(newRot);
            this.transform.eulerAngles = newRot;
            //this.transform.RotateAround(LevelCenterPoint.position, Vector3.right, Time.deltaTime * ShiftSpeed);
            //TiltPlaneDown();
        }
    }

    public void StartTiltUp()
    {
        startPos = Camera.main.transform.eulerAngles.x;
        endPos = startPos - 20;
        Debug.Log(startPos + " / " + endPos);
        fall = true;
    }

    /*private void TiltPlaneDown()
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
    }*/
}
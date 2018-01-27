using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class MenuController : MonoBehaviour {

    public Transform JoinedPlayerPosition;
    public Transform HostPlayerPosition;
    public float ShiftCamPositionSpeed = 1;

    Vector3 shiftCamtarget;
    float lerpCamPosTime = 1f;
    float currentLerpCamPosTime = 1.1f;
    Vector3 originCamPos;

    public GameObject Canvas;
    public float ShiftFocusSpeed = 0.1f;
    public float FinalFocusDistance = 25f;
    public float StartFocusDistance = 0.3f;


    float lerpTime = 1f;
    float currentLerpTime = 1.1f;
    PostProcessingProfile ppProfile;
    DepthOfFieldModel.Settings settings;

    void Start()
    {
        originCamPos = transform.position;

        ppProfile = GetComponent<PostProcessingBehaviour>().profile;
        settings = ppProfile.depthOfField.settings;
        settings.focusDistance = StartFocusDistance;
        ppProfile.depthOfField.settings = settings;
        settings = ppProfile.depthOfField.settings;
    }

    void Update()
    {
        FocusGame();
        ShiftCamPos();
    }

    public void StartFocusShift()
    {
        currentLerpTime = 0f;
        Canvas.SetActive(false);
    }

    public void FocusHostPlayer()
    {
        currentLerpCamPosTime = 0f;
        shiftCamtarget = HostPlayerPosition.position;
    }

    public void FocusJoinedPlayer()
    {
        currentLerpCamPosTime = 0f;
        shiftCamtarget = JoinedPlayerPosition.position;
    }

    void ShiftCamPos()
    {
        if(currentLerpCamPosTime > lerpCamPosTime)
        {
            return;
        }

        //increment timer once per frame
        currentLerpCamPosTime += Time.deltaTime * ShiftCamPositionSpeed;

        //lerp!
        float t = currentLerpCamPosTime / lerpCamPosTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        transform.position = Vector3.Lerp(originCamPos, shiftCamtarget, t);
    }

    void FocusGame()
    {
        if (currentLerpTime > lerpTime)
        {
            return;
        }

        //increment timer once per frame
        currentLerpTime += Time.deltaTime * ShiftFocusSpeed;

        //lerp!
        float t = currentLerpTime / lerpTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        settings.focusDistance = Mathf.Lerp(StartFocusDistance, FinalFocusDistance, t);
        ppProfile.depthOfField.settings = settings;
        Debug.Log(Mathf.Lerp(StartFocusDistance, FinalFocusDistance, t));
    }
}

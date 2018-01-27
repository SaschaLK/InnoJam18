﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenShakeController : MonoBehaviour {

    public static ScreenShakeController Instance;

    void Awake() {
        Instance = this;
    }

    public void Trigger() {
        StartCoroutine(Shake());
    }

    IEnumerator Shake() {
        float time = Time.timeScale;
        Camera cam = Camera.main;

        const float dur = 0.4f;
        Vector3 camShakePrev = Vector3.zero;
        for (float start = Time.unscaledTime, end = start + dur, t = 0; t < 1; t = (Time.unscaledTime - start) / dur) {
            float st = t;
            st = 1f - Mathf.Pow(1f - Mathf.Sin(Mathf.PI * st * 0.5f), 2);

            float max = (1f - st) * 2f;

            cam.transform.Translate(-camShakePrev);
            cam.transform.Translate(camShakePrev = new Vector3(
                3f * (Random.Range(0f, max) - (max) * 0.5f),
                3f * (Random.Range(0f, max) - (max) * 0.5f),
                0f
            ));

            yield return null;
        }

    }

}
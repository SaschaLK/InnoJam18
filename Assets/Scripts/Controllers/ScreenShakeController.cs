using System.Collections;
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

    public void Trigger(Transform target, float duration , float intensity) {
        StartCoroutine(Shake(target, duration, intensity));
    }

    IEnumerator Shake(Transform target, float duration , float intensity) {
        float time = Time.timeScale;

        Collider[] colliders = target.GetComponentsInChildren<Collider>();
        bool[] enabledMap = new bool[colliders.Length];
        for (int i = 0; i < colliders.Length; i++) {
            enabledMap[i] = colliders[i].enabled;
            colliders[i].enabled = false;
        }

        Vector3 camShakePrev = Vector3.zero;
        for (float start = Time.time, end = start + duration, t = 0; t < 1; t = (Time.time - start) / duration) {
            float st = t;
            st = 1f - Mathf.Pow(1f - Mathf.Sin(Mathf.PI * st * 0.5f), 2);

            float max = (1f - st) * 2f * intensity;

            target.Translate(-camShakePrev);
            target.Translate(camShakePrev = new Vector3(
                3f * (Random.Range(0f, max) - (max) * 0.5f),
                3f * (Random.Range(0f, max) - (max) * 0.5f),
                0f
            ));

            yield return null;
        }

        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = enabledMap[i];
        }

    }

}

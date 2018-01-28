using System.Collections;
using UnityEngine;

public class CameraTurner : MonoBehaviour {

    public float speed = 1.0f;
    public float amount = 1.0f;

    Vector3 prev;

    void Update() {
        float x = (Mathf.PerlinNoise(Time.time * speed + 1024f, 0f) * 2.0f - 1.0f) * amount;
        float y = (Mathf.PerlinNoise(0f, Time.time * speed + 2048f) * 2.0f - 1.0f) * amount;
        float z = (Mathf.PerlinNoise(Time.time * speed, Time.time * speed - 2048f) * 2.0f - 1.0f) * amount;

        transform.Translate(-prev);
        transform.Translate(prev = new Vector3(x, y, z));
    }
}

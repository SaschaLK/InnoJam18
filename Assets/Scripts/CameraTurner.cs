using System.Collections;
using UnityEngine;

public class CameraTurner : MonoBehaviour {

    public float speed = 1.0f;
    public float amount = 1.0f;
    Transform myTransform;

    Vector3 origPos;
    Vector3 ourPos;

    void Start() {
        myTransform = transform;
    }

    void Update() {
        if (myTransform.localPosition != ourPos) {
            origPos = myTransform.localPosition;
        }

        float x = (Mathf.PerlinNoise(Time.time * speed + 1024f, 0f) * 2.0f - 1.0f) * amount;
        float y = (Mathf.PerlinNoise(0f, Time.time * speed + 2048f) * 2.0f - 1.0f) * amount;
        float z = (Mathf.PerlinNoise(Time.time * speed, Time.time * speed - 2048f) * 2.0f - 1.0f) * amount;

        myTransform.localPosition = ourPos = origPos + new Vector3(x, y, z);
    }
}

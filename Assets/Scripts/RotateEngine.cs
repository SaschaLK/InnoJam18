using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEngine : MonoBehaviour {

    public float speed = 10f;

    private void Update()
    {
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * speed * 10f);
    }
}

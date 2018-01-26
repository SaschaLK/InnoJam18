using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float MovementSpeed = 10f;

    Collider collider;
    Rigidbody rigidbody;

	void Awake() {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update() {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rigidbody.velocity = new Vector3(
            x * MovementSpeed,
            rigidbody.velocity.y,
            y * MovementSpeed
        );

	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float MovementSpeed = 10f;
    public float UsageRadius = 5f;

    float closestDist;
    InteractiveComponent closest;

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

        InteractiveComponent[] interactives = FindObjectsOfType<InteractiveComponent>();
        Vector3 pos = transform.position;
        closestDist = UsageRadius * UsageRadius;
        closest = null;
        for (int i = 0; i < interactives.Length; i++) {
            InteractiveComponent interactive = interactives[i];

            float dist = (pos - interactive.transform.position).sqrMagnitude;
            bool canUse = true; // TODO: Dependencies

            if (!canUse || dist > closestDist) {
                continue;
            }

            closestDist = dist;
            closest = interactive;
        }

        if (closest != null) {
            closestDist = Mathf.Pow(closestDist, 0.5f);
            if (Input.GetButtonDown("Fire1")) {
                closest.Use.Invoke();
            }
        }

	}

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, UsageRadius);
        if (closest != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, closestDist);
        }
    }

}

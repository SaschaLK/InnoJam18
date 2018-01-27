using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDamageHandler : MonoBehaviour {

    InteractiveComponent interactive;
    GameObject fire;

    private void Awake()
    {
        interactive = GetComponent<InteractiveComponent>();
    }

    public void FireOutbreak()
    {
        int rand = Random.Range(0, 100);

        if (rand <= 99 && fire == null)
        {
            Debug.Log("FIRE!!!");
            fire = Instantiate(Resources.Load<GameObject>("Fire"), this.transform.position, Quaternion.identity, this.transform);
        }
    }
}

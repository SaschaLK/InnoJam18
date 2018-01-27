using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEngine : MonoBehaviour {

    public float speed = 10f;

    private void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * speed * 10f);
        /*Debug.Log(transform.localEulerAngles);
        Vector3 rotation = transform.eulerAngles + new Vector3(speed * Time.deltaTime, 0, 0) ;
        //transform.Rotate(rotation);


        transform.localEulerAngles = rotation;*/
    }
}

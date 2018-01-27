using System.Collections;
using UnityEngine;

    public class CameraTurner : MonoBehaviour {

		public float speed = 1.0f;
		public float amount =1.0f;
		Transform myTransform;

		void Start () {
			myTransform = transform;
		}
			
		void Update () {
			float x = (Mathf.PerlinNoise(Time.time * speed, 0f) * 2.0f -1.0f) * amount;
			float y = (Mathf.PerlinNoise(0f, Time.time * speed) * 2.0f - 1.0f) * amount;

			myTransform.localPosition = new Vector3(x,y,0f);
		}
	}

using System.Collections;
using UnityEngine;

namespace CaLey17.Crisis.Scripts {
    public class CameraShaker : MonoBehaviour
    {
        public float DefaultMagnitude = .032f;
        public float DefaultDuration = .3f;
        public float DefaultShakeSpeed = 10f;
        public AnimationCurve DefaultDamper = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f));
		public float RotationAmount = 100f;

		Vector3 originalShakerPos = new Vector3(0,0,0);
		Quaternion originalShakerRot = Quaternion.Euler (new Vector3(0,0,0));

        private void Update()
        {
            // TODO it's a DEBUG method... disablen...
			if (Input.GetKeyDown(KeyCode.S)) Shake();
        }

        public void Shake(float magnitude = -1f, float duration = -1f, float shakeSpeed = -1f, AnimationCurve damper = null)
        {
            // take default values if there is no external override
            if (magnitude.Equals(-1f)) magnitude = DefaultMagnitude;
            if (duration.Equals(-1f)) duration = DefaultDuration;
            if (shakeSpeed.Equals(-1f)) shakeSpeed = DefaultShakeSpeed;
            if (damper == null) damper = DefaultDamper;

            StartCoroutine(Shaking(magnitude, duration, shakeSpeed, damper));
        }

        // ---------------------------------------------------------------
        // Another Solution:
        // http://unitytipsandtricks.blogspot.de/2013/05/camera-shake.html
        // ---------------------------------------------------------------
        private IEnumerator Shaking(float magnitude, float duration, float shakeSpeed, AnimationCurve damper)
        {
            var elapsed = 0.0f;

			while (elapsed < duration) 
			{
				elapsed += Time.deltaTime;			
				var dampedMagnitude = (damper != null) ? (damper.Evaluate(elapsed / duration) * magnitude) : magnitude;
				var x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * dampedMagnitude) - (dampedMagnitude / 2f);
				var y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * dampedMagnitude) - (dampedMagnitude / 2f);
				var z = (Mathf.PerlinNoise(0.5f, Time.time * shakeSpeed * 0.5f) * dampedMagnitude) - (dampedMagnitude / 2f);
				transform.localPosition = new Vector3(originalShakerPos.x + x, originalShakerPos.y + y, originalShakerPos.z);
				transform.localRotation = Quaternion.Euler(new Vector3(originalShakerRot.x, originalShakerRot.y, originalShakerRot.z + (z * RotationAmount)));
				yield return null;
			}
			transform.localPosition = originalShakerPos;
        }
    }
}

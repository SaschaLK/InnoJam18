using System.Collections;
using UnityEngine;

public class CameraZoomer : MonoBehaviour {

    public Transform Player1Pos;
    public Transform Player2Pos;

    void Start() {
        // TODO: Don't zoom on scene start, but zoom on round start!
       
    }
    private bool hasStarted = false;
    public void TriggerZoom()
    {
        if (hasStarted) return;
        Zoom();
        hasStarted = true;
    }

    public void Zoom(float delay = 5f, float duration = 5f) {
        StartCoroutine(_Zoom(delay, duration));
    }
    IEnumerator _Zoom(float delay, float duration) {
        Vector3 from = transform.position;

        yield return new WaitForSeconds(delay);

        Vector3 to = PlayerController.LocalPlayer.PlayerNumber == 1 ? Player1Pos.position : Player2Pos.position;

        for (float start = Time.time, end = start + duration, t = 0; t < 1; t = (Time.time - start) / duration) {
            float st = t;
            st = 1f - Mathf.Pow(1f - Mathf.Sin(Mathf.PI * st * 0.5f), 2);

            transform.position = Vector3.Lerp(from, to, st);

            yield return null;
        }

        transform.position = to;
    }

}

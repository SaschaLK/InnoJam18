using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {

    public int PlayerNumber;

    public Material Material;

    public GameObject Glass;

	void Awake() {
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();

        Material = Instantiate(Material);
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].sharedMaterial = Material;
        }

        // TODO: Don't fade on scene start, but fade on round start!
        Fade();
	}

    public void Fade(float delay = 5f, float duration = 5f) {
        StartCoroutine(_Fade(delay, duration));
    }
    IEnumerator _Fade(float delay, float duration) {
        Material.color = new Color(0f, 0f, 0f, 0f);

        yield return new WaitForSeconds(delay);

        if (PlayerController.LocalPlayer != null && PlayerController.LocalPlayer.PlayerNumber == PlayerNumber) {
            yield break;
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraZoomer>().TriggerZoom();

        if (Glass != null)
            Destroy(Glass);

        for (float start = Time.time, end = start + duration, t = 0; t < 1; t = (Time.time - start) / duration) {
            float st = t;
            st = 1f - Mathf.Pow(1f - Mathf.Sin(Mathf.PI * st * 0.5f), 2);

            Material.color = new Color(0f, 0f, 0f, st);

            yield return null;
        }

        Material.color = new Color(0f, 0f, 0f, 1f);
    }

    public void ResetFade() {
        Material.color = new Color(0f, 0f, 0f, 0f);

    }

}

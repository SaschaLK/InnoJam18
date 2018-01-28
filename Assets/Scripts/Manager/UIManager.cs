using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject CanvasPrefab;
    public float OffsetVertical = 0f;
    public float OffsetHorizontal = 0f;

    public Sprite Crystal;
    public Sprite FireExt;
    public Sprite Grease;
    public Sprite Rocket;
    public Sprite Stick;
    public Sprite Hammer;
    public Sprite Bucket;
    public Sprite LauncherLoaded;
    public Sprite LoadEngine;
    public Sprite EnigineReady;
    public Sprite Left;
    public Sprite Right;
    public Sprite Altitude;

    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
            
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowUI(Sprite icon, Transform position)
    {
        GameObject newCanvas;
        newCanvas = Instantiate(CanvasPrefab, position);
        newCanvas.transform.localPosition = new Vector3(OffsetHorizontal, OffsetVertical, 0);
        newCanvas.transform.GetChild(0).GetComponent<Image>().sprite = icon;
    }
}

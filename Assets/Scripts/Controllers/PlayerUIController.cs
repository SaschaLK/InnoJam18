using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    public float Padding = 10f;

    private RectTransform Root;
    private CanvasGroup Group;
    private RectTransform Panel;
    private Text TooltipText;

    private Transform parent;
    private Quaternion rotation;
    private Vector3 rotationEuler;
    private Vector3 offset;

    private bool visibleText;
    private bool visibleMinigame;

    private void Awake() {
        if (Root == null)
            Root = GetComponent<RectTransform>();
        if (Group == null)
            Group = GetComponent<CanvasGroup>();
        Group.alpha = 0;
        if (Panel == null)
            Panel = transform.Find("Panel").GetComponent<RectTransform>();
        if (TooltipText == null)
            TooltipText = transform.Find("TooltipText").GetComponent<Text>();

        parent = transform.parent;
        rotation = transform.rotation;
        rotationEuler = rotation.eulerAngles;
        offset = transform.position - parent.position;
    }

    private void LateUpdate() {
        if (visibleText || visibleMinigame) {
            Group.alpha = Mathf.Clamp01(Group.alpha + Time.deltaTime / 0.1f);
        } else {
            Group.alpha = Mathf.Clamp01(Group.alpha - Time.deltaTime / 0.1f);
        }
        visibleText = false;

        // transform.rotation = rotation;
        transform.eulerAngles = new Vector3(
            Mathf.Lerp(0f, rotation.eulerAngles.x, Group.alpha),
            rotation.eulerAngles.y,
            rotation.eulerAngles.z
        );
        transform.position = parent.position + offset;
    }

    public void ShowText(string text) {
        TooltipText.enabled = true;
        TooltipText.text = text;

        Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipText.preferredWidth + Padding);
        Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TooltipText.preferredHeight + Padding);

        visibleText = true;
    }

    public void ShowMinigame() {
        TooltipText.text = "";
        TooltipText.enabled = false;

        Panel.anchorMin = Vector2.zero;
        Panel.anchorMax = Vector2.one;
        Panel.sizeDelta = Vector2.zero;

        visibleMinigame = true;
    }

    public void HideMinigame() {
        visibleMinigame = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTooltipController : MonoBehaviour {

    public float Padding = 10f;

    private CanvasGroup Group;
    private RectTransform Panel;
    private Text TooltipText;
    private Quaternion rotation;
    private Transform parent;
    private Vector3 offset;
    private bool textVisible;

    private void Awake() {
        if (Group == null)
            Group = GetComponent<CanvasGroup>();
        if (Panel == null)
            Panel = transform.Find("Panel").GetComponent<RectTransform>();
        if (TooltipText == null)
            TooltipText = transform.Find("TooltipText").GetComponent<Text>();

        rotation = transform.rotation;
        parent = transform.parent;
        offset = transform.position - parent.position;
    }

    private void LateUpdate() {
        transform.rotation = rotation;
        transform.position = parent.position + offset;

        if (!textVisible)
            Group.alpha = Mathf.Clamp01(Group.alpha - Time.deltaTime / 0.1f);
        textVisible = false;
    }

    public void ShowText(string text) {
        TooltipText.text = text;

        Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TooltipText.preferredWidth + Padding);
        Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TooltipText.preferredHeight + Padding);

        Group.alpha = Mathf.Clamp01(Group.alpha + Time.deltaTime / 0.1f);
        textVisible = true;
    }

}

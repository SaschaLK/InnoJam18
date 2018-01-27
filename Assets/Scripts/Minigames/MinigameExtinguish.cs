using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameExtinguish : MinigameBase {

    protected override string UIPath {
        get {
            return "Extinguish";
        }
    }

    protected Text CountdownText;
    protected RectTransform LeftImage;
    protected RectTransform RightImage;
    protected RectTransform Table;
    protected RectTransform Cell;

    protected float TimeLeft;
    protected Vector2 LeftSpray;
    protected Vector2 RightSpray;

    protected RectTransform[,] Cells;
    protected Image[,] CellImages;

    protected override void Awake() {
        base.Awake();

    }

    protected override void _StartMinigame() {
        CountdownText = UITree.Find("CountdownText").GetComponent<Text>();
        LeftImage = UITree.Find("ImageLeft").GetComponent<RectTransform>();
        RightImage = UITree.Find("ImageRight").GetComponent<RectTransform>();

        Table = UITree.Find("Table").GetComponent<RectTransform>();
        Cell = Table.Find("Cell").GetComponent<RectTransform>();
        Vector3 cellPos = Cell.localPosition;

        Cells = new RectTransform[10, 20];
        CellImages = new Image[10, 20];
        for (int y = 0; y < 10; y++) {
            for (int x = 0; x < 20; x++) {
                RectTransform cell = Cell;
                if (x != 0 || y != 0) {
                    cell = Instantiate(Cell, Table);
                    cell.localPosition = new Vector3(
                        cellPos.x + x * 50f,
                        cellPos.y - y * 50f,
                        cellPos.z
                    );
                }
                Cells[y, x] = cell;
                CellImages[y, x] = cell.GetComponent<Image>();
                CellImages[y, x].color = new Color(
                    Random.Range(0.7f, 0.9f),
                    Random.Range(0.4f, 0.6f),
                    Random.Range(0.1f, 0.3f)
                );
            }
        }

        TimeLeft = 5f;
        LeftSpray = new Vector2(0f, 0.5f);
        RightSpray = new Vector2(1f, 0.5f);

        UpdateSprites();
    }

    private void UpdateSprites() {
        float width = UITree.rect.width;
        float height = UITree.rect.height;

        LeftImage.anchoredPosition = new Vector2(
            LeftSpray.x * width,
            (LeftSpray.y - 0.5f) * height
        );

        RightImage.anchoredPosition = new Vector2(
            (RightSpray.x - 1f) * width,
            (RightSpray.y - 0.5f) * height
        );

        int x, y;
        RectTransform cell;

        x = Mathf.Clamp(Mathf.FloorToInt(LeftSpray.x * 19f), 0, 19);
        y = Mathf.Clamp(Mathf.FloorToInt((1f - LeftSpray.y) * 9f), 0, 9);
        cell = Cells[y, x];
        if (cell != null) {
            Destroy(cell.gameObject);
            Cells[y, x] = null;
            CellImages[y, x] = null;
        }

        x = Mathf.Clamp(Mathf.FloorToInt(RightSpray.x * 19f), 0, 19);
        y = Mathf.Clamp(Mathf.FloorToInt((1f - RightSpray.y) * 9f), 0, 9);
        cell = Cells[y, x];
        if (cell != null) {
            Destroy(cell.gameObject);
            Cells[y, x] = null;
            CellImages[y, x] = null;
        }

        for (y = 0; y < 10; y++) {
            for (x = 0; x < 20; x++) {
                Image cellImage = CellImages[y, x];
                if (cellImage == null)
                    continue;
                Color c = cellImage.color;
                cellImage.color = new Color(
                    Mathf.Lerp(c.r, Random.Range(0.7f, 0.9f), 0.1f),
                    Mathf.Lerp(c.g, Random.Range(0.4f, 0.6f), 0.1f),
                    Mathf.Lerp(c.b, Random.Range(0.1f, 0.3f), 0.1f)
                );
            }
        }
    }

    private void Update() {
        if (!Active)
            return;

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
            EndMinigame();
            return;
        }

        LeftSpray += new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        ) * 0.05f;
        LeftSpray = new Vector2(
            Mathf.Clamp01(LeftSpray.x),
            Mathf.Clamp01(LeftSpray.y)
        );

        RightSpray += new Vector2(
            Input.GetAxis("Horizontal Look"),
            -Input.GetAxis("Vertical Look")
        ) * 0.05f;
        RightSpray = new Vector2(
            Mathf.Clamp01(RightSpray.x),
            Mathf.Clamp01(RightSpray.y)
        );

        UpdateSprites();

        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0f)
            TimeLeft = 0f;
        CountdownText.text = TimeLeft.ToString("N2").Replace(',', '.');

        if (TimeLeft <= 0) {
            TimeLeft = 0;
            EndMinigame();
            return;
        }

    }

    protected override void _EndMinigame() {
        if (!Active)
            return;

        int cleared = 0;
        for (int y = 0; y < 10; y++) {
            for (int x = 0; x < 20; x++) {
                if (Cells[y, x] == null) {
                    cleared++;
                }
            }
        }
        Win = cleared >= (10 * 20) * 0.6;
    }

}

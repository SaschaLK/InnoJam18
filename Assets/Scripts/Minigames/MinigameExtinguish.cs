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

    protected readonly static int TableWidth = 20;
    protected readonly static int TableHeight = 10;
    protected readonly static float CellWidth = 50f;
    protected readonly static float CellHeight = 50f;

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

        Cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CellWidth);
        Cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CellHeight);

        Cells = new RectTransform[TableHeight, TableWidth];
        CellImages = new Image[TableHeight, TableWidth];
        for (int y = 0; y < TableHeight; y++) {
            for (int x = 0; x < TableWidth; x++) {
                RectTransform cell = Cell;
                if (x != 0 || y != 0) {
                    cell = Instantiate(Cell, Table);
                    cell.localPosition = new Vector3(
                        cellPos.x + x * CellWidth,
                        cellPos.y - y * CellHeight,
                        cellPos.z
                    );
                }
                Cells[y, x] = cell;
                CellImages[y, x] = cell.GetComponent<Image>();
                CellImages[y, x].color = new Color(
                    Random.Range(0.7f, 0.9f),
                    Random.Range(0.3f, 0.5f),
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

        x = Mathf.Clamp(Mathf.FloorToInt(LeftSpray.x * (TableWidth - 1f)), 0, TableWidth - 1);
        y = Mathf.Clamp(Mathf.FloorToInt((1f - LeftSpray.y) * (TableHeight - 1f)), 0, TableHeight - 1);
        cell = Cells[y, x];
        if (cell != null) {
            Destroy(cell.gameObject);
            Cells[y, x] = null;
            CellImages[y, x] = null;
        }

        x = Mathf.Clamp(Mathf.FloorToInt(RightSpray.x * (TableWidth - 1f)), 0, TableWidth - 1);
        y = Mathf.Clamp(Mathf.FloorToInt((1f - RightSpray.y) * (TableHeight - 1f)), 0, TableHeight - 1);
        cell = Cells[y, x];
        if (cell != null) {
            Destroy(cell.gameObject);
            Cells[y, x] = null;
            CellImages[y, x] = null;
        }

        for (y = 0; y < TableHeight; y++) {
            for (x = 0; x < TableWidth; x++) {
                Image cellImage = CellImages[y, x];
                if (cellImage == null)
                    continue;
                Color c = cellImage.color;
                cellImage.color = new Color(
                    Mathf.Lerp(c.r, Random.Range(0.7f, 0.9f), 0.1f),
                    Mathf.Lerp(c.g, Random.Range(0.3f, 0.5f), 0.1f),
                    Mathf.Lerp(c.b, Random.Range(0.1f, 0.3f), 0.1f)
                );
            }
        }
    }

    private void Update() {
        if (!Active)
            return;

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

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
            EndMinigame();
            return;
        }

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

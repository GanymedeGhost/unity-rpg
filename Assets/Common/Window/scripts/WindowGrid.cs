using UnityEngine;

public class WindowGrid : MonoBehaviour {

    public GameObject labelPrefab;
    public Transform cursorObj;

    private WindowManager windowManager;
    private Window window;

    private Vector2 size;
    private float margins = 12f;

    private int cols = 1;
    private int rows = 1;

    private bool isActive = false;

    private GameObject[,] cells;

    private float[,] cellW;
    private float[,] cellH;
    private Vector3[,] cellPos;
    private Vector3[,] cellAnchor;

    private int cursorX = 0;
    private int cursorY = 0;

    private float cursorDelay = 0.2f;
    private float lastCursorTime = 0f;
    private float nextCursorTime = 0.2f;

    public void Initialize(Window _window, int w, int h, bool _isActive = true, bool _cursorEnabled = false)
    {
        this.windowManager = GetComponentInParent<WindowManager>();
        this.window = _window;
        this.isActive = _isActive;

        this.size = new Vector2(_window.Size().x - margins * 2, _window.Size().y - margins * 2);

        this.cols = w;
        this.rows = h;

        this.cells = new GameObject[w, h];
        this.cellW = new float[w, h];
        this.cellH = new float[w, h];
        this.cellPos = new Vector3[w, h];
        this.cellAnchor = new Vector3[w, h];

        float cursorWidth;
        if (_cursorEnabled)
        {
            cursorWidth = cursorObj.GetComponent<RectTransform>().rect.size.x;
        } else
        {
            cursorWidth = 0f;
        }

        for (int x=0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                cellW[x, y] = size.x / cols - cursorWidth;
                cellH[x, y] = size.y / rows;

                GameObject newLabel = Instantiate(labelPrefab, transform) as GameObject;

                RectTransform labelRect = newLabel.GetComponent<RectTransform>();
                labelRect.sizeDelta = new Vector2(cellW[x, y], cellH[x, y]);

                cellAnchor[x, y] = labelRect.position + new Vector3(cellW[x, y] * x + cursorWidth * (x + 1), cellH[x, y] * -y + cellH[x, y] * -0.5f);
                cellPos[x, y] = labelRect.position + new Vector3(cellW[x, y] * x + cursorWidth * (x + 1), cellH[x, y] * -y);

                labelRect.position = cellPos[x, y];
            }
        }

        if (_cursorEnabled)
        {
            ToggleCursor();
            UpdateCursorPosition();
        }
    }

    private void Update()
    {
        if (isActive && CursorEnabled())
        {
            ProcessInput();
        }
    }

    /// <summary>
    /// Reset time until next character is typed
    /// </summary>
    private void RefreshCursorTimer()
    {
        lastCursorTime = Time.time;
        nextCursorTime = lastCursorTime + cursorDelay;
    }

    /// <summary>
    /// Check if it's time to print the next character
    /// </summary>
    /// <returns>True if ready, false otherwise</returns>
    private bool CheckCursorTimer()
    {
        return Time.time >= nextCursorTime;
    }

    private void ProcessInput()
    {
        if (CheckCursorTimer())
        {
            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                RefreshCursorTimer();
                cursorX += 1;
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                RefreshCursorTimer();
                cursorX -= 1;
            }
            else if (Input.GetAxisRaw("Vertical") > 0.1f)
            {
                RefreshCursorTimer();
                cursorY -= 1;
            }
            else if (Input.GetAxis("Vertical") < -0.1f)
            {
                RefreshCursorTimer();
                cursorY += 1;
            }
            LimitCursor();
            UpdateCursorPosition();
        }

        if (Input.GetButtonDown("Confirm"))
        {
            Debug.Log("Cursor X: " + cursorX);
            Debug.Log("Cursor Y: " + cursorY);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            windowManager.DestroyWindow(window.gameObject);
        }
    }

    private void LimitCursor()
    {
        if (cursorX < 0)
        {
            cursorX = cols-1;
        }
        else if (cursorX == cols)
        {
            cursorX = 0;
        }

        if (cursorY < 0)
        {
            cursorY = rows - 1;
        }
        else if (cursorY == rows)
        {
            cursorY = 0;
        }
    }

    private void UpdateCursorPosition()
    {
        cursorObj.position = cellAnchor[cursorX, cursorY];
    }

    public bool CursorEnabled()
    {
        return cursorObj.gameObject.activeSelf;
    }

    public bool ToggleCursor()
    {
        cursorObj.gameObject.SetActive(!CursorEnabled());
        return CursorEnabled();
    }
}

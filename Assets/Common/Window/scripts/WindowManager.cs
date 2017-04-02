using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class WindowManager : MonoBehaviour {

	public GameObject windowPrefab;
	public GameObject windowTextPrefab;
    public GameObject windowGridPrefab;

    public RectTransform myRect;

	private string lorem = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.";

	[SerializeField]
	private List <GameObject> windows = new List<GameObject>();

	private int activeWindow = -1;

    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Create a new window and add it to the window list
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public GameObject CreateWindow (Vector2 pos, Vector2 size) {
		GameObject window = Instantiate (windowPrefab) as GameObject;
		window.transform.SetParent (transform, false);

        window.GetComponent<Window>().Initialize(pos, size, true);
	
		windows.Add (window);
		activeWindow = windows.Count - 1;

		return window;
	}

    public bool DestroyWindow(GameObject window)
    {
        windows.Remove(window);
        Destroy(window);
        return true;
    }

    public void CreateMessageWindow(Vector2 pos, Vector2 size, string message)
    {
        Window window = CreateWindow(pos, size).GetComponent<Window>();
        WindowText newText = window.AddToContents(windowTextPrefab).GetComponent<WindowText>();
        newText.Initialize(window, message, window.Size(), true);
    }

	private void Start() {
        Vector2 size = new Vector2(280f, 82f);
        Vector2 pos = new Vector2(myRect.sizeDelta.x * 0.5f, size.y - 24f);

        //CreateMessageWindow(pos, size, "How about this textbox, huh? Pretty nice, right? I think it's pretty cool. Sure as hell took long enough to get it working correctly. Fo shizzle, y'all. This is the end of the line or something. For all of this, I'm better off without you.");

        Window window = CreateWindow(new Vector2(150f, 100f), new Vector2(200f, 200f)).GetComponent<Window>();
        //WindowText newText = window.AddToContents(windowTextPrefab).GetComponent<WindowText>();
        //newText.Initialize(window, lorem, window.Size(), true);
        WindowGrid newGrid = window.AddToContents(windowGridPrefab).GetComponent<WindowGrid>();
        newGrid.Initialize(window, 1, 8, true, true);
    }

}

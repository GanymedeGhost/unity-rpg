using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {

	public GameObject windowPrefab = null;
	public GameObject windowTextPrefab = null;

	private string lorem = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";

	[SerializeField]
	private List <GameObject> windows = new List<GameObject>();

	private int activeWindow = -1;

	public GameObject CreateWindow (Vector2 pos, Vector2 size) {
		GameObject window = Instantiate (windowPrefab) as GameObject;
		window.transform.SetParent (transform, false);
		RectTransform rect = window.GetComponent<RectTransform> ();
		rect.position = pos;
		rect.sizeDelta = size;
	
		windows.Add (window);
		activeWindow = windows.Count - 1;

		return window;
	}

	private void Start() {
		Vector2 size = new Vector2 (280f, 100f);
		Window window = CreateWindow (new Vector2 (150f, 100f), size).GetComponent<Window>();
		WindowText newText = window.AddToContents (windowTextPrefab).GetComponent<WindowText>();
		newText.Initialize(lorem, size, true);
	}

}

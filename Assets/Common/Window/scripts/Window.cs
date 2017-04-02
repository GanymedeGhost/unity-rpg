using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

	public GameObject back = null;
	public GameObject border = null;
	public GameObject container = null;

	private bool isActive = true;

	private List<GameObject> contents = new List<GameObject>();

    private RectTransform myRect;

    public void Initialize(Vector2 pos, Vector2 size, bool _isActive)
    {
        this.SetPosition(pos);
        this.SetSize(size);
        this.isActive = _isActive;
    }

	public GameObject AddToContents(GameObject obj) {
		GameObject newObj = Instantiate (obj) as GameObject;
		newObj.transform.SetParent (container.transform, false);
		contents.Add (newObj);

		return newObj;
	}

    public void SetPosition(Vector2 pos)
    {
        myRect.position = pos;
    }

    public void SetSize(Vector2 size)
    {
        myRect.sizeDelta = size;
    }

    public Vector2 Size()
    {
        Rect rect = GetComponent<RectTransform>().rect;
        return rect.size;
    }

    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }
}

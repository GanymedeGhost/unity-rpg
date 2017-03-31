using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

	public GameObject back = null;
	public GameObject border = null;
	public GameObject container = null;

	private bool isActive = true;

	private List<GameObject> contents = new List<GameObject>();

	public GameObject AddToContents(GameObject obj) {
		GameObject newObj = Instantiate (obj) as GameObject;
		newObj.transform.SetParent (container.transform, false);
		contents.Add (newObj);

		return newObj;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSize : MonoBehaviour {

	Font myFont;
    CharacterInfo charInfo;

    // Use this for initialization
    void Awake () {
		myFont = GetComponent<Text> ().font;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Fire1") > 0.5f) {
			Canvas.ForceUpdateCanvases ();
			if (myFont.GetCharacterInfo ('N', out charInfo, myFont.fontSize, FontStyle.Normal)) {
				print (charInfo.advance);
			}
		}
	}
}

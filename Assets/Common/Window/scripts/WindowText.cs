using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Text))]
public class WindowText : MonoBehaviour {

	private Vector2 size;

	private Text textObject;
	private Font myFont;

	private int maxLines = 0;

	private string text = "";
	private string displayedText = "";

	private bool previewed = false;
	private bool previewCleared = false;

	private bool typeText = false;
	private bool isWaiting = false;
	private bool isActive = true;

	private float typeDelay = 0.1f;
	private float lastCharTime = 0;
	private float nextCharTime = 0;


	public void Initialize(string _text, Vector2 _size, bool _typed = false, bool _isActive = true) {
		this.text = _text;
		this.size = _size;
		this.typeText = _typed;
		this.isActive = _isActive;

		if (!typeText) {
			textObject.text = text;
		} else {
			ParseText ();
		}
	}

	public void SetText(string _text){
		text = _text;
	}

	private void ParseText() {
		Canvas.ForceUpdateCanvases ();
        CharacterInfo charInfo;
		myFont.GetCharacterInfo (' ', out charInfo, myFont.fontSize);
		float spaceWidth = charInfo.advance;

		string[] words = text.Split (' ');
		float[] wordWidths = new float[words.Length];

		List<string> parsedLines = new List<string> ();
		List<float> lineWidths = new List<float> ();
		parsedLines.Add ("");
		lineWidths.Add (0f);

		int curWord = 0;
		int curLine = 0;

		foreach (string word in words) {
			char[] wordChars = word.ToCharArray ();
			foreach (char c in wordChars) {
				myFont.GetCharacterInfo (c, out charInfo, myFont.fontSize);
				wordWidths [curWord] += charInfo.advance;
			}
			wordWidths [curWord] += spaceWidth;

			if (lineWidths [curLine] + wordWidths [curWord] > size.x - 40) {
				parsedLines.Add ("");
				lineWidths.Add (0f);
				curLine += 1;
            }
			parsedLines [curLine] += words [curWord] + " ";
            lineWidths[curLine] += wordWidths[curWord];

			curWord += 1;
		}
        
        foreach (string str in parsedLines)
        {
            print(str);
        }
	}

	private void Awake() {
		textObject = GetComponent<Text> ();

		myFont = textObject.font;
		maxLines = Mathf.FloorToInt (size.y / myFont.fontSize);

		if (!typeText) {
			textObject.text = text;
		} else {
			textObject.text = "";
				
			lastCharTime = Time.time;
			nextCharTime = lastCharTime + typeDelay;
		}
	}

	private void Update() {
		if (!previewed) {
			textObject.text = text;
			previewed = true;
		} else if (!previewCleared) {
			textObject.text = "";
			previewCleared = true;
		}

		if (isActive && typeText) {
			if (Time.time > nextCharTime) {
				if (text != "") {
					lastCharTime = Time.time;
					nextCharTime = lastCharTime + typeDelay;

					string charToType = text.Substring (0, 1);
					text = text.Remove (0, 1);

					textObject.text += charToType;
				} else {
					isActive = false;
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Text))]
public class WindowText : MonoBehaviour {

    private WindowManager windowManager;
    private Window window;

	public Vector2 size;

	private Text textObject;
	private Font myFont;

	private int maxLines = 0;
    private int curTypedLine = 0;

	private string text = "";

	private bool previewed = false;
	private bool previewCleared = false;

	private bool typeText = false;
	private bool isWaiting = false;
	private bool isActive = true;

	private float typeDelay = 0.1f;
    private float curTypeDelay = 0.1f;
	private float lastCharTime = 0;
	private float nextCharTime = 0;

	public void Initialize(Window _window, string _text, Vector2 _size, bool _typed = false, bool _isActive = true) {

        this.windowManager = GetComponentInParent<WindowManager>();
        this.window = _window;

        this.text = " " + _text;
		this.size = new Vector2 (_size.x - myFont.fontSize, _size.y - myFont.fontSize);

        this.maxLines = Mathf.FloorToInt(size.y / myFont.fontSize) - 1;

        this.typeText = _typed;
		this.isActive = _isActive;

		if (!typeText) {
			textObject.text = text;
		}
	}

	public void SetText(string _text){
		text = _text;
	}

	private void ParseLines() {
        CharacterInfo charInfo;

        Canvas.ForceUpdateCanvases();
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
                Canvas.ForceUpdateCanvases();
                myFont.GetCharacterInfo (c, out charInfo, myFont.fontSize);
				wordWidths [curWord] += charInfo.advance;
			}

            words[curWord] += " ";
            wordWidths[curWord] += spaceWidth;
            
            if (lineWidths[curLine] + wordWidths[curWord] >= size.x)
            {
                parsedLines.Add("");
                lineWidths.Add(0f);
                curLine += 1;
            }

            lineWidths[curLine] += wordWidths[curWord];
			parsedLines[curLine] += words[curWord];

			curWord += 1;
		}

        //Rebuild the text with the linebreaks in place
        text = "";
        foreach (string str in parsedLines)
        {
            text += str + "\n";
        }
	}

	private void Awake() {
		textObject = GetComponent<Text> ();

		myFont = textObject.font;

		if (!typeText) {
			textObject.text = text;
		} else {
			textObject.text = "";
            RefreshTypeTimer();
		}
	}

	private void Update()
    {
        OneTimePreview();

        if (isActive && typeText)
        {
            TypeText();
            ProcessInput();
        }
    }

    /// <summary>
    /// Process input for typed text
    /// </summary>
    private void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isWaiting)
            {
                if (text == "")
                {
                    windowManager.DestroyWindow(window.gameObject);
                }

                textObject.text = "";
                isWaiting = false;
                curTypedLine = 0;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            curTypeDelay = typeDelay * 0.25f;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            curTypeDelay = typeDelay;
        }
    }

    /// <summary>
    /// Print the text character by character. 
    /// Pause if the max lines are exceeded and wait for input.
    /// </summary>
    private void TypeText()
    {
        if (CheckTypeTimer() && !isWaiting)
        {
            if (text != "")
            {
                RefreshTypeTimer();

                string charToType = text.Substring(0, 1);

                if (charToType == "\n")
                {
                    curTypedLine += 1;

                    if (curTypedLine == maxLines)
                    {
                        isWaiting = true;
                    }
                }

                text = text.Remove(0, 1);

                textObject.text += charToType;
            }
            else
            {
                isWaiting = true;
            }
        }
    }


    /// <summary>
    /// Reset time until next character is typed
    /// </summary>
    private void RefreshTypeTimer()
    {
        lastCharTime = Time.time;
        nextCharTime = lastCharTime + curTypeDelay;
    }

    /// <summary>
    /// Check if it's time to print the next character
    /// </summary>
    /// <returns>True if ready, false otherwise</returns>
    private bool CheckTypeTimer()
    {
        return Time.time >= nextCharTime;
    }

    /// <summary>
    /// Draw the full text during the first update cycle then clear it.
    /// Needed for CharacterInfo to get the proper sizes for font characters.
    /// </summary>
    private void OneTimePreview()
    {
        if (!previewed)
        {
            textObject.text = text;
            Canvas.ForceUpdateCanvases();
            previewed = true;
        }
        else if (!previewCleared)
        {
            textObject.text = "";
            Canvas.ForceUpdateCanvases();
            previewCleared = true;
            ParseLines();
        }
    }
}

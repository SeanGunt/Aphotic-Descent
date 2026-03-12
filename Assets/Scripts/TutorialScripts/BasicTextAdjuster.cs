using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicTextAdjuster : MonoBehaviour
{
	private Text basicText;

	private void Awake()
	{
		basicText = this.GetComponent<Text>();
	}

	//simple, can be called through events, and will handle disappearing via the text disabler separate from this script.
	public void ChangeBasicText(string newText)
	{
		basicText.text = newText;
	}
}

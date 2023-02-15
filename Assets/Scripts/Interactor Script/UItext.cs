using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class UItext : MonoBehaviour {

	public bool inTrigger;
	private PauseControls pauseControls;
	[Space(10)]
	[Header("Toggle for the gui on off")]
	public bool GuiOn;

	[Space(10)]
	public string Text = "E to interact";
	public GUIStyle guiStyle;
	public int fontSize;
	public Rect BoxSize = new Rect( 0, 0, 200, 100);

	[Space(10)]
	public GUISkin customSkin;

	private void Awake()
	{
		pauseControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseControls>();
	}
	void OnTriggerEnter(Collider other) 
	{
         if (other.gameObject.tag == "Player") 
         {
			inTrigger = true;
            GuiOn = true;
         }
		
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.gameObject.tag == "Player") 
         {
			inTrigger = false;
             GuiOn = false;
         }
	}

	void OnDisable()
	{
		inTrigger = false;
		GuiOn = false;
	}

	private void Update()
	{
		if(inTrigger && pauseControls.paused)
		{
			GuiOn = false;
		}
		else if (inTrigger && !pauseControls.paused)
		{
			GuiOn = true;
		}
	}

	void OnGUI()
	{

		if (customSkin != null)
		{
			GUI.skin = customSkin;
		}

		if (GuiOn == true)
		{
			// Make a group on the center of the screen
			GUI.BeginGroup (new Rect ((Screen.width - BoxSize.width) / 2, (Screen.height - BoxSize.height) / 2, BoxSize.width, BoxSize.height));
			// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
			guiStyle.fontSize = fontSize;
			GUI.Label(BoxSize, Text, guiStyle);

			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();

		}
	}
}
using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {
	
	public Rect side1;
	public Rect side2;
	public Rect topbar;
	public Rect botbar;
	
	public Rect but1;
	public Rect but2;
	
	
	public GUIStyle sides;
	public GUIStyle top;
	public GUIStyle bot;
	public GUIStyle but;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.Box(side1, "", sides);
		GUI.Box(side2, "", sides);
		GUI.Box (topbar, "", top);
		GUI.Box (botbar, "", bot);
		GUI.Button(but1, "", but);
		GUI.Button(but2, "", but);
	}
}

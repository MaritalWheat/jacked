using UnityEngine;
using System.Collections;

public class SkillUI : MonoBehaviour {
	
	//Used for sliding open and closed
	private const float openY = 100;
	private const float closeY = -500;
	private bool sliding;
	private float destination = closeY;
	
	public Rect window;
	public float slideSpeed;
	public bool open;
	public Texture lockedIcon;
	
	private bool isVisible;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (sliding == true) {
			if (window.y < destination) {
				window.y = Mathf.Min(window.y + (1 * slideSpeed) , openY);
			} else if (window.y > destination) {
				window.y = Mathf.Max(window.y - (1 * slideSpeed), closeY);
			}
			
			if (window.y == openY || window.y == closeY) {
				sliding = false;	
			}
		}
		
		if (window.y == closeY) {
			if (isVisible) {
				GameManager.PauseGame(false);	
			}
			isVisible = false;
		} else {
			if (!isVisible) {
				GameManager.PauseGame(true);
			}
			isVisible = true;	
		}
		
		
	}
	
	void OnGUI() {
		if (!isVisible) {
			return;	
		}

        GUI.depth = int.MinValue;   // draw on top of everything
		GUI.BeginGroup(window);
		Rect skillRect = new Rect(10, 10, 100, 100);
		int row = 0, col = 0;
		foreach (Skill s in SkillManager.getSkills()) {
			skillRect.x = 10 + (110 * col);
			skillRect.y = 10 + (110 * row);
			GUI.DrawTexture(skillRect, s.m_icon);
			if (SkillManager.GetSkillLevel(s.m_name) == 0) {
				GUI.DrawTexture(skillRect, lockedIcon);	
			}
			col ++;
			if (col > 3) {
				col = 0; 
				row ++;
			}
		}
		GUI.EndGroup();
	}
	
	public void slide(bool slideOpen) {
		if (slideOpen == open || sliding == true) {
			return;	
		}
		sliding = true;
		open = slideOpen;
		if (slideOpen) {
			destination = openY;
		} else {
			destination = closeY;	
		}
	}
}

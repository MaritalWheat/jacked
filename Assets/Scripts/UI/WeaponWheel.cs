using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponWheel : MonoBehaviour {
	
	public GUISkin m_skin;
	public Texture2D m_triShotSprite;
	public Texture2D m_circleBackground;
	public Texture2D m_blurOverlay;
	public float speed;
	private static WeaponWheel m_singleton;
	
	private List<float> m_itemContainerAngle; 
	private List<Rect> m_itemContainers;
	private List<Weapon> m_item;
	private const int k_numberOfItems = 8;
	private float butW = 75f;
	private float butH = 75f;	
	private float radius;
	private Vector2 origin;
	private float nextStopPoint = 0f;
	private bool m_display;
	private bool m_startedDisplay = false;
	//private bool m_finishedStartingDisplay = false;
	
	
	
	void Start () {
		if (m_singleton == null) {
			m_singleton = this;
		}
		m_itemContainerAngle = new List<float>();
		m_itemContainers = new List<Rect>();
		for (int i = 0; i < k_numberOfItems; i++) {
			m_itemContainerAngle.Add(i * 45);
			m_itemContainers.Add (new Rect(0, 0, butW, butH));
		}
		origin = new Vector2(Screen.width / 2 - butW / 2, Screen.height / 2.10f - butW / 2);	
	}
	
	void Update () {
		if ( m_itemContainerAngle[0] < nextStopPoint) {
			for (int i = 0; i < m_itemContainerAngle.Count; i++) {
				m_itemContainerAngle[i] += 5;
			}
		}	
		
		if (WeaponWheel.GetWeaponWheelDisplayStatus()) {
				GameManager.DisplayWeaponWheel(true);
			} else {
				GameManager.DisplayWeaponWheel(false);
		}
	}
	
	void OnGUI() {
		if (m_startedDisplay) {
			radius = 0;
			m_startedDisplay = false;
		}
		if (m_display) DrawWeaponSelect();		
	}
	
	private void DrawWeaponSelect() {
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", HudController.s_singleton.m_overlayStyle);
		if (radius < .3f * Screen.height) {
			radius += 7;
		} else {
			radius = .3f * Screen.height;
		}
		
		GUI.skin = m_skin;
		int index = 0;
		GUI.DrawTexture(new Rect(Screen.width / 2 - radius, Screen.height / 2.10f - radius, radius * 2, radius * 2), m_circleBackground);
		foreach (float property in m_itemContainerAngle) {
			Rect currentContainer = m_itemContainers[index];
			Vector2 pointToMoveTo = PointOnCircle(radius, property, origin);
			currentContainer.x = pointToMoveTo.x;
			currentContainer.y = pointToMoveTo.y;
			if (index < WeaponManager.m_singleton.m_ownedWeapons.Count) {
				if (GUI.Button(currentContainer, "", WeaponManager.m_singleton.m_ownedWeapons[index].m_style)) {
					PlayerCharacter.SetPlayerWeapon(WeaponManager.GetWeapon(WeaponManager.m_singleton.m_weapons[index].m_name));
					WeaponWheel.DisplayWeaponWheel(false);
					//nextStopPoint += 45f; //ROTATION IMPLEMENTATION
				}
			} else {
				GUI.Button(currentContainer, "", WeaponManager.m_singleton.m_noWeapon.m_style);
			}				
			index++;
		}
	}
	
	private Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
    {
        // Convert from degrees to radians via multiplication by PI/180        
        float x = (float)(radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180F)) + origin.x;
        float y = (float)(radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180F)) + origin.y;

        return new Vector2(x, y);
    }
	
	public static void DisplayWeaponWheel(bool display) {
		m_singleton.m_display = display;
		if (display == true) {
			m_singleton.m_startedDisplay = true;
		}
	}
	
	public static bool GetWeaponWheelDisplayStatus() {
		return m_singleton.m_display;	
	}
}

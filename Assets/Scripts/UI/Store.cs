using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Store : MonoBehaviour {
	private static Store m_singleton;
	private bool m_display;
	
	private bool m_storeClosed = true;
	
    public Rect m_storeBounds;
	public Rect m_headerBounds;
    public Rect m_bottomButtonLeftBounds;
    public Rect m_bottomButtonRightBounds;
	public Rect m_itemListBounds;
	public Rect m_selectedItemBounds;

    public GUIStyle m_boxStyle;
	public GUIStyle m_headerStyle;
	public GUIStyle m_backStyle;
	public GUIStyle m_windowStyle;
    public GUIStyle m_buttonStyle;
    public GUIStyle m_listStyle;

    private Vector2 m_scrollPos = Vector2.zero;
	private bool showRight;
	private float m_blurAmount;
	private Rect m_currStoreBounds;
    //private List<Weapon> m_storeWeapons;
	
	void Start() {
		if (m_singleton == null) {
			m_singleton = this;
		}
		m_currStoreBounds = new Rect (m_storeBounds.x, -m_storeBounds.height - 10, m_storeBounds.width, m_storeBounds.height);
	}
	
	void OnGUI() {
		if (m_display) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", HudController.s_singleton.m_overlayStyle);
            if (m_currStoreBounds.y < m_storeBounds.y) {
                m_currStoreBounds.y += Screen.height / 30;
            } else if (m_currStoreBounds.y >= m_storeBounds.y) {
                m_currStoreBounds.y = m_storeBounds.y;
            }
        } else {
            if (m_currStoreBounds.yMax > -10) {
                m_currStoreBounds.y -= Screen.height / 30;
            } else if (m_currStoreBounds.yMax <= -10) {
                m_currStoreBounds.y = -m_storeBounds.height - 10f; 
            }
        }
		GUI.Box(m_currStoreBounds, "", m_backStyle);
		GUI.BeginGroup(m_currStoreBounds);
        GUI.Box(m_itemListBounds, "", m_windowStyle);
		GUI.Box(m_selectedItemBounds, "", m_windowStyle);
		if (GUI.Button(m_bottomButtonLeftBounds, "Start Next", m_buttonStyle)) {
			m_storeClosed = true;
			m_display = false;
		}
        GUI.Button(m_bottomButtonRightBounds, "Money: " + GameManager.s_singleton.m_score, m_buttonStyle);

		GUI.Box(m_headerBounds, "STORE", m_headerStyle);
        
		GUI.BeginGroup(m_itemListBounds);

        int yValue = 100;
        foreach (Skill s in SkillManager.getSkills())
		{
            if (GUI.Button(new Rect(0, yValue, m_itemListBounds.width, 20), s.skillName, m_listStyle))
            {
                GameObject notification = (GameObject)Instantiate(HudController.s_singleton.ScrollingNotificationPrefab);

                if (SkillManager.PurchaseSkill(s.skillName))
                {
                    notification.GetComponent<ScrollingText>().Display("Purchased " + s.skillName + ".", 10.0f, HudController.s_singleton.m_largeFightingSpirit);
                }
                else
                {
                    notification.GetComponent<ScrollingText>().Display("You can't afford " + s.skillName + ".", 10.0f, HudController.s_singleton.m_largeFightingSpirit);
                }
            }
            yValue += 25;
        }

		for(int i = 0; i < WeaponManager.m_singleton.m_weapons.Count; i++) { 
			Weapon w = WeaponManager.m_singleton.m_weapons[i];
			if (GUI.Button(new Rect(0, yValue, m_itemListBounds.width, 20), w.m_name, m_listStyle) && GameManager.s_singleton.m_score >= 100) {
				WeaponManager.PurchaseWeapon(w.m_name);
                GameManager.s_singleton.m_score -= 100;
            }
			yValue += 25;
        }
		GUI.EndGroup();

        GUI.EndGroup();   
	}
	
	public static void DisplayStore() {
		Store.m_singleton.m_storeClosed = false;
		Store.m_singleton.m_display = true;
	}
	
	public static bool CheckStoreClosed() {
		return Store.m_singleton.m_storeClosed;
	}
		
	
	
}

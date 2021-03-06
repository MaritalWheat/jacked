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
	public GUIStyle m_descriptionStyle;

    private Vector2 m_scrollPos = Vector2.zero;
	private bool showRight;
	private float m_blurAmount;
	private Rect m_currStoreBounds;
	private Rect m_descriptionBounds;
    //private List<Weapon> m_storeWeapons;

	class DescriptionRect {
		public static bool m_valid;
		public static string m_text;
	}
	
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

		bool mouseOverDescription = false;
		Rect descriptionRect = new Rect(0, 0, 0 , 0);
		Skill hovered = null;

        foreach (Skill s in SkillManager.getSkills())
		{
			if (!s.GetPurchasedStatus()) {
				Rect tempRect = new Rect(0, yValue, m_itemListBounds.width, 20);

	            if (GUI.Button(tempRect, s.m_name, m_listStyle))
	            {
	                GameObject notification = (GameObject)Instantiate(HudController.s_singleton.ScrollingNotificationPrefab);

	                if (SkillManager.PurchaseSkill(s.m_name))
	                {
	                    notification.GetComponent<ScrollingText>().Display("Purchased " + s.m_name + ".", 10.0f, HudController.s_singleton.m_largeFightingSpirit);
	                }
	                else
	                {
	                    notification.GetComponent<ScrollingText>().Display("You can't afford " + s.m_name + ".", 10.0f, HudController.s_singleton.m_largeFightingSpirit);
	                }
	            }
	            yValue += 25;

				if (tempRect.Contains(Event.current.mousePosition)) {
					descriptionRect = tempRect;
					mouseOverDescription = true;
					hovered = s;
				}
			}
        }

		if (!mouseOverDescription) {
			DescriptionRect.m_valid = false;
		} else {
			DescriptionRect.m_valid = true;
			DescriptionRect.m_text = hovered.m_description;
		}

		for(int i = 0; i < WeaponManager.m_singleton.m_weapons.Count; i++) { 
			Weapon w = WeaponManager.m_singleton.m_weapons[i];
			if (GUI.Button(new Rect(0, yValue, m_itemListBounds.width, 20), w.m_name, m_listStyle) && GameManager.s_singleton.m_score >= 100) {
				WeaponManager.PurchaseWeapon(w.m_name);
                GameManager.s_singleton.m_score -= 100;
            }
			yValue += 25;
        }

		//Debug.Log(Hello + " is hovered over.");

		GUI.EndGroup();

        GUI.EndGroup(); 
		
		if (DescriptionRect.m_valid) {
			GUIContent content = new GUIContent(DescriptionRect.m_text);
			float height = m_descriptionStyle.CalcHeight(content, 200.0f);
			float left = descriptionRect.x + m_itemListBounds.x - 100;
			float top = descriptionRect.y + m_itemListBounds.y - height / 2;
			GUI.Box(new Rect(left, top, 200, height), DescriptionRect.m_text, m_descriptionStyle);
		}
	}
	
	public static void DisplayStore() {
		Store.m_singleton.m_storeClosed = false;
		Store.m_singleton.m_display = true;
	}
	
	public static bool CheckStoreClosed() {
		return Store.m_singleton.m_storeClosed;
	}
		
	
	
}

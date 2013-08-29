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
    //private List<Weapon> m_storeWeapons;
	
	void Start() {
		if (m_singleton == null) {
			m_singleton = this;
		}
        //m_storeWeapons = new Dictionary<Weapon,bool>();
        //foreach (Weapon selected in WeaponManager.m_singleton.m_weapons) {
        //    m_storeWeapons.Add(selected, false);
        //}
		/*m_storeBounds = new Rect(Screen.width * .10f, Screen.height * .10f, Screen.width * .8f, Screen.height * .8f);
        m_bottomButtonLeftBounds = new Rect(m_storeBounds.width * .01f, m_storeBounds.height - m_storeBounds.height * .01f - m_storeBounds.height * .1f, m_storeBounds.width * .97f / 2, m_storeBounds.height * .1f);
        m_bottomButtonRightBounds = new Rect(m_bottomButtonLeftBounds.xMax + m_storeBounds.width * .01f, m_bottomButtonLeftBounds.y, m_bottomButtonLeftBounds.width, m_bottomButtonLeftBounds.height);
		m_headerBounds = new Rect (m_storeBounds.width * .01f, m_storeBounds.height * .015f, m_storeBounds.width * .98f, m_storeBounds.height * .15f);
		m_itemListBounds = new Rect(m_bottomButtonLeftBounds.xMin, m_headerBounds.yMax + m_storeBounds.height * .01f, m_bottomButtonLeftBounds.width, m_bottomButtonLeftBounds.yMin -
			m_storeBounds.height * .01f - m_headerBounds.yMax - m_storeBounds.height * .01f);
		m_selectedItemBounds = new Rect(m_bottomButtonRightBounds.xMin, m_itemListBounds.yMin, m_itemListBounds.width, m_itemListBounds.height);*/
	}
	
	void OnGUI() {
		if (!m_display) return;
		GUI.Box(m_storeBounds, "", m_backStyle);
		GUI.BeginGroup(m_storeBounds);
        		GUI.Box(m_itemListBounds, "", m_windowStyle);
		GUI.Box(m_selectedItemBounds, "", m_windowStyle);
		if (GUI.Button(m_bottomButtonLeftBounds, "Start Next", m_buttonStyle)) {
			m_storeClosed = true;
			m_display = false;
		}
        GUI.Button(m_bottomButtonRightBounds, "Money: " + GameManager.s_singleton.m_score, m_buttonStyle);

		GUI.Box(m_headerBounds, "STORE", m_headerStyle);
        GUI.BeginGroup(m_itemListBounds);
/*
        int yValue = 10;
        foreach (Skill s in SkillManager.skills)
        {
            if (GUI.Button(new Rect(0, yValue, m_itemListBounds.width, 20), s.skillName, m_listStyle))
            {
                GameObject notification = (GameObject)Instantiate(HudController.s_singleton.ScrollingNotificationPrefab);

                if (SkillManager.PurchaseSkill(s))
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
*/

        float y = 10;
        foreach (Weapon selected in WeaponManager.m_singleton.m_weapons) {
            //bool value = m_storeWeapons.TryGetValue(selected, out value);
            selected.m_selectedInStore = GUI.Toggle(new Rect(0, y, m_itemListBounds.width, 20), selected.m_selectedInStore, selected.name, m_listStyle);
            Debug.Log("New Value: " + selected.m_selectedInStore);
            y += 30;
            
        }

        /*if (GUI.Button(new Rect(0, 10, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle)) {
			Debug.Log("Item Selected");
			showRight = true;
		}
        GUI.Button(new Rect(0, 40, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 70, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 100, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 130, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 160, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 190, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 220, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 250, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 280, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);
        GUI.Button(new Rect(0, 310, m_itemListBounds.width, 20), "A Really Sick Item", m_listStyle);*/
        GUI.EndGroup();

        foreach (Weapon selected in WeaponManager.m_singleton.m_weapons) {
            if (selected.m_selectedInStore) {
                if (GUI.Button(m_selectedItemBounds, "Buy", m_buttonStyle) && GameManager.s_singleton.m_score >= 100) {
                    WeaponManager.m_singleton.m_ownedWeapons.Add(WeaponManager.GetWeapon("TriShot"));
                    GameManager.s_singleton.m_score -= 100;
                }
            }
        }
		if (showRight) { 
			if (GUI.Button(m_selectedItemBounds, "Buy", m_buttonStyle) && GameManager.s_singleton.m_score >= 100) {
				WeaponManager.m_singleton.m_ownedWeapons.Add(WeaponManager.GetWeapon("TriShot"));
				GameManager.s_singleton.m_score -= 100;
			}
		}

        //m_scrollPos = GUI.BeginScrollView(new Rect(m_itemListBounds.left, m_itemListBounds.top, m_itemListBounds.width, m_itemListBounds.height + 500), m_scrollPos, m_itemListBounds);
        //GUI.Button(new Rect(0, 0, 100, 100), "", m_buttonStyle);
        //GUI.EndScrollView();
        
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

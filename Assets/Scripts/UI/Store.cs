using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	private static Store m_singleton;
	private bool m_display;
	
	private bool m_storeClosed = true;
	
    private Rect m_storeBounds;
    private Rect m_bottomButtonLeftBounds;
    private Rect m_bottomButtonRightBounds;

    public GUIStyle m_boxStyle;
    public GUIStyle m_buttonStyle;
	
	void Start() {
		if (m_singleton == null) {
			m_singleton = this;
		}
		
		m_storeBounds = new Rect(Screen.width * .10f, Screen.width * .10f, Screen.width * .8f, Screen.height * .8f);
        m_bottomButtonLeftBounds = new Rect(m_storeBounds.width * .01f, m_storeBounds.height - m_storeBounds.height * .01f - m_storeBounds.height * .1f, m_storeBounds.width * .97f / 2, m_storeBounds.height * .1f);
        m_bottomButtonRightBounds = new Rect(m_bottomButtonLeftBounds.xMax + m_storeBounds.width * .015f, m_bottomButtonLeftBounds.y, m_bottomButtonLeftBounds.width, m_bottomButtonLeftBounds.height);
	}
	
	void OnGUI() {
		if (!m_display) return;
		GUI.Box(m_storeBounds, "", m_boxStyle);
		GUI.BeginGroup(m_storeBounds);
        GUI.Box(new Rect (m_storeBounds.width * .01f, m_storeBounds.height * .015f, m_storeBounds.width * .98f, m_storeBounds.height * .15f), "STORE", m_boxStyle);
		if (GUI.Button(m_bottomButtonLeftBounds, "Start Next", m_buttonStyle)) {
			m_storeClosed = true;
			m_display = false;
		}
        GUI.Button(m_bottomButtonRightBounds, "Button", m_buttonStyle);
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

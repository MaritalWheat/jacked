using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	private static Store m_singleton;
	private bool m_display;
	
	private bool m_storeClosed = true;
	private Rect m_storeBounds;
	
	void Start() {
		if (m_singleton == null) {
			m_singleton = this;
		}
		
		m_storeBounds = new Rect(Screen.width * .10f, Screen.width * .10f, Screen.width * .8f, Screen.height * .8f);
	}
	
	void OnGUI() {
		if (!m_display) return;
		GUI.Box(m_storeBounds, "");
		GUI.BeginGroup(m_storeBounds);
		if (GUI.Button(new Rect (0, 0, 400, 400), "Start Next")) {
			m_storeClosed = true;
			m_display = false;
		}
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

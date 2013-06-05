using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public static GameOver s_singleton;
    private bool m_display = false;
    public GUIStyle m_buttonStyle;
    public GUIStyle m_textStyle;

    private Rect m_restartRect;
    private Rect m_statsRect;

	// Use this for initialization
	void Start () {
	    if (!s_singleton) {
            s_singleton = this;
        }

        m_restartRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 +100, 200, 50);
        m_statsRect = new Rect(Screen.width / 2 - 400, Screen.height / 2 - 100, 800, 150);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (m_display)
        {
            if (GUI.Button(m_restartRect, "Restart", m_buttonStyle))
            {
                HudController.s_singleton.Display(true);
                m_display = false; GameManager.s_singleton.Reset();
                GameManager.s_singleton.Enable(true);
                InputManager.Enable(true);
            }

            GUI.BeginGroup(m_statsRect);
            GUI.Box(new Rect(0, 0, m_statsRect.width, m_statsRect.width), "");
            GUI.Label(new Rect(0, 0, 800, 50), "You killed "+GameManager.s_singleton.m_creaturesKilled+ " furry creatures,", m_textStyle);
            GUI.Label(new Rect(0, 50, 800, 50), "and " + GameManager.s_singleton.m_catsKilled + " stupid cats.", m_textStyle);
            GUI.Label(new Rect(0, 100, 800, 50), "Final Score : " + GameManager.s_singleton.m_score,m_textStyle );

            GUI.EndGroup();
        }
    }

    public void Display()
    {
        m_restartRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 + 100, 200, 50);
        m_display = true;
    }
}

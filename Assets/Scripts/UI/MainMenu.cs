using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    private bool m_display = true;

    public GUIStyle m_titleStyle;
    public GUIStyle m_buttonStyle;
    private Rect m_titleRect;
    private Rect m_startRect;
    private float screenWidth, screenHeight;
    private HudController hudController;

	// Use this for initialization
	void Start () {
        hudController = this.gameObject.GetComponent<HudController>();
        m_startRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50);
        m_titleRect = new Rect(Screen.width / 2 - 200, 100, 400, 100);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            m_startRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50);
            m_titleRect = new Rect(Screen.width / 2 - 200, 100, 400, 100);
            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }
	}

    void OnGUI()
    {
        if (m_display)
        {
            GUI.Label(m_titleRect, "JACKED", m_titleStyle);
            if (GUI.Button(m_startRect, "START", m_buttonStyle))
            {
                //Start the game
                hudController.Display(true);
                InputManager.Enable(true);
                Display(false);
                //GameObject startNotification = (GameObject)Instantiate(hudController.ScrollingNotificationPrefab);
                //startNotification.GetComponent<ScrollingText>().Display("GET JACKED!", 15.0f, hudController.m_largeFightingSpirit);
                GameManager.s_singleton.Enable(true);
                //AudioManager.m_singleton.PlayTest();    
            }
        }

    }

    public void Display(bool display)
    {
        m_display = display;
    }
}

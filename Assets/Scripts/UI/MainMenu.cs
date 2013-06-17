using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    private bool m_display = true;

    public GUISkin skin;
    public GUIStyle m_titleStyle;
    public GUIStyle m_buttonStyle;

    private GUIStyleState state;
    private Rect m_titleRect;
    private Rect m_startRect;
    private float screenWidth, screenHeight;
    private HudController hudController;
    private bool playedIntroSound = false;

	void Start () {
        hudController = this.gameObject.GetComponent<HudController>();
        m_startRect = new Rect(Screen.width + 100, Screen.height / 2 - 25, 200, 50);
        m_titleRect = new Rect(0 - 400, 100, 400, 100);
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
            if (!playedIntroSound) {
                AudioManager.m_singleton.PlayIntroScreen();
                playedIntroSound = true;
            }
            SlideDisplay();



            GUI.skin = skin;
            GUI.Label(m_titleRect, "JACKED", m_titleStyle);
            if (m_startRect.Contains(Input.mousePosition)) {
                //Debug.Log("CONTAINS");
                //m_buttonStyle.hover = GUIStyleState.hover;
            }
            if (GUI.Button(m_startRect, "START", skin.GetStyle("Button")))
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

    void SlideDisplay() { //MAKE THIS MORE REUSABLE AND LESS SPECIFIC
        float middleX = Screen.width / 2 - m_titleRect.width / 2;
        m_titleRect.x += 25;
        m_startRect.x -= 25;
        if (middleX - m_titleRect.x <= 0) {
            m_titleRect.x = middleX;
        }      
        middleX = Screen.width / 2 - m_startRect.width / 2;
        if (m_startRect.x - middleX <= 0) {
            m_startRect.x = middleX;
        }
    }

    public void Display(bool display)
    {
        m_display = display;
    }
}

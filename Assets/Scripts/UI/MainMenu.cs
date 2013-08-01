using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    public static MainMenu m_singleton;
    public bool m_display = false;
    public bool m_displayTitleScreen = true;
    public GUISkin skin;
    public GUIStyle m_titleStyle;
    public GUIStyle m_buttonStyle;

    private GUIStyleState state;
    private Rect m_titleRect;
    private Rect m_startRect;
    private Rect m_resumeRect;
    private Rect m_optionsRect;
    private Rect m_button3Rect;
    private Rect m_button4Rect;

    private float m_positionButtonsGroupOne;
    private float m_positionButtonsGroupTwo;

    private float screenWidth, screenHeight;
    private HudController hudController;
    private bool playedIntroSound = false;
    private List<string> menuButtons = new List<string>() {"Resume", "Options", "Skills", "Quit"};
    private int currentButtonIndex = 0;
    private float m_menuButtonSwitchTimeBuffer = 0f;
    private const float k_menuButtonMinSwitchTime = 25;

    private bool m_isSelectButtonPressed;
    
	void Start () {
        if (m_singleton == null)
        {
            m_singleton = this;
        }
        hudController = this.gameObject.GetComponent<HudController>();
        m_startRect = new Rect(Screen.width + 100, Screen.height / 2 - 25, 200, 50);
        m_titleRect = new Rect(0 - 400, 100, 400, 100);

        m_resumeRect = new Rect(0, 0, 200, 50);
        m_optionsRect = new Rect(0, 50, 200, 50);
        m_button3Rect = new Rect(0, 100, 200, 50);
        m_button4Rect = new Rect(0, 150, 200, 50);
        
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        m_positionButtonsGroupOne = 0 - 200;
        m_positionButtonsGroupTwo = Screen.width;

	}
	
	// Update is called once per frame
	void Update () {
        if (screenHeight != Screen.height || screenWidth != Screen.width) {
            m_startRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50);
            m_titleRect = new Rect(Screen.width / 2 - 200, 100, 400, 100);
            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }
        if (Input.GetButtonDown("ButtonA") || Input.GetButtonDown("Start")) {
            m_isSelectButtonPressed = true;
        } else {
            m_isSelectButtonPressed = false;
        }
        if (menuButtons[currentButtonIndex].Equals("Resume") && Input.GetButtonDown("ButtonA")) {
            GameManager.Resume();
        }
	}

    void OnGUI()
    {
        if (m_displayTitleScreen)
        {
            DrawTitleScreen();
        } else {
            DrawMainMenu();
        }

    }

    void DrawTitleScreen() {
        
        if (!playedIntroSound) {
            AudioManager.m_singleton.PlayIntroScreen();
            playedIntroSound = true;
        }
        SlideDisplay();
        GUI.skin = skin;
        GUI.Label(m_titleRect, "JACKED", m_titleStyle);
        GUI.SetNextControlName("Start");
		if (PlayerCharacter.s_singleton.gamePad) {
        	GUI.FocusControl("Start");
		}
        if (GUI.Button(m_startRect, "START", skin.GetStyle("Button")) || (GUI.GetNameOfFocusedControl().Equals("Start") && m_isSelectButtonPressed)) {
            hudController.Display(true);
            InputManager.Enable(true);
            GameManager.s_singleton.Enable(true); 
            DisplayTitleScreen(false);
        }
    }

    void DrawMainMenu()
    {
        
        GUI.BeginGroup(new Rect(m_positionButtonsGroupOne, Screen.height / 3, 200, 400));
        GUI.SetNextControlName("Resume");
        if (GUI.Button(m_resumeRect, "RESUME", skin.GetStyle("Button"))) GameManager.Resume();
        GUI.SetNextControlName("Button1");
        GUI.Button(m_button3Rect, "BUTTON", skin.GetStyle("Button"));
        GUI.EndGroup();
        if (m_display) {
            if (m_positionButtonsGroupOne < Screen.width / 2 - 100) {
                m_positionButtonsGroupOne += Screen.width / 35;
            } else if (m_positionButtonsGroupOne >= Screen.width / 2 - 100) {
                m_positionButtonsGroupOne = Screen.width / 2 - 100;
            }
        } else {
            if (m_positionButtonsGroupOne > 0 - 200) {
                m_positionButtonsGroupOne -= Screen.width / 35;
            } else if (m_positionButtonsGroupOne <= 0 - 200) {
                m_positionButtonsGroupOne = 0 - 200; 
                GameManager.Pause(false);
            }
        }

        GUI.BeginGroup(new Rect(m_positionButtonsGroupTwo, Screen.height / 3, 200, 400));
        GUI.SetNextControlName("Options");
        GUI.Button(m_optionsRect, "OPTIONS", skin.GetStyle("Button"));
        GUI.SetNextControlName("Quit");
        if(GUI.Button(m_button4Rect, "QUIT", skin.GetStyle("Button"))) Application.Quit();
        GUI.EndGroup();
        if (m_display) {
            if (m_positionButtonsGroupTwo > Screen.width / 2 - 100) {
                m_positionButtonsGroupTwo -= Screen.width / 35;
            } else if (m_positionButtonsGroupTwo <= Screen.width / 2 - 100) {
                m_positionButtonsGroupTwo = Screen.width / 2 - 100;
            }
        } else {
            if (m_positionButtonsGroupTwo < Screen.width) {
                m_positionButtonsGroupTwo += Screen.width / 30;
            } else if (m_positionButtonsGroupTwo >= Screen.width) {
                m_positionButtonsGroupTwo = Screen.width;
                GameManager.Pause(false);
            }
        }
		
		if (PlayerCharacter.s_singleton.gamePad) {
	        GUI.FocusControl(menuButtons[currentButtonIndex]);
	        m_menuButtonSwitchTimeBuffer += 1;
	        if (Input.GetAxis("VerticalGamepad") == 1 && m_menuButtonSwitchTimeBuffer > k_menuButtonMinSwitchTime) {
	            if (currentButtonIndex == menuButtons.Count - 1) {
	                currentButtonIndex = 0;
	            } else {
	                currentButtonIndex++;
	            }
	            GUI.FocusControl(menuButtons[currentButtonIndex]);
	            m_menuButtonSwitchTimeBuffer = 0;
	        } else if (Input.GetAxis("VerticalGamepad") == -1 && m_menuButtonSwitchTimeBuffer > k_menuButtonMinSwitchTime) {
	            if (currentButtonIndex == 0) {
	                currentButtonIndex = menuButtons.Count - 1;
	            } else { 
	                currentButtonIndex--;
	            }
	            GUI.FocusControl(menuButtons[currentButtonIndex]);
	            m_menuButtonSwitchTimeBuffer = 0;
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

    public static void DisplayMainMenu(bool display)
    {
        MainMenu.m_singleton.m_display = display;
    }

    public static void DisplayTitleScreen(bool display) 
    {
        MainMenu.m_singleton.m_displayTitleScreen = display;
    }
}

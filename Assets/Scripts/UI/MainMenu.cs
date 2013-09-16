using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{

    public static MainMenu m_singleton;
    public bool m_display = false;
    public bool m_displayTitleScreen = true;
    public GUISkin skin;
    public GUIStyle m_titleStyle;
    public GUIStyle m_buttonStyle;

    private GUIStyleState state;
    public Rect m_menuBackgroundBounds;
    private Rect m_titleRect;
    private Rect m_startRect;
    private Rect m_resumeRect;
    private Rect m_optionsRect;
    private Rect m_button3Rect;
    private Rect m_button4Rect;
    private Rect m_backRect;

    private Rect m_upRect, m_leftRect, m_downRect, m_rightRect;
    private Rect m_upLabelRect, m_leftLabelRect, m_downLabelRect, m_rightLabelRect;

    private float m_positionButtonsGroupOne;
    private float m_positionButtonsGroupTwo;

    private float screenWidth, screenHeight;
    private HudController hudController;
    private bool playedIntroSound = false;
    private List<string> menuButtons = new List<string>() { "Resume", "Options", "Skills", "Quit" };
    private int currentButtonIndex = 0;
    private float m_menuButtonSwitchTimeBuffer = 0f;
    private const float k_menuButtonMinSwitchTime = 25;

    private bool m_isSelectButtonPressed;
    private bool m_displayOptions;

    private static readonly int OPTIONS_WIDTH = 80;
    private static readonly int OPTIONS_HEIGHT = 40;

    public GUIStyle m_menuBackgroundStyle;
    private Rect m_menuBounds;

    // Allowed keys for custom directional mapping
    private KeyCode[] whitelist = {
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.KeypadPeriod,
        KeyCode.KeypadDivide,
        KeyCode.KeypadMultiply,
        KeyCode.KeypadMinus,
        KeyCode.KeypadPlus,
        KeyCode.KeypadEnter,
        KeyCode.KeypadEquals,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.Insert,
        KeyCode.Home,
        KeyCode.End,
        KeyCode.PageUp,
        KeyCode.PageDown,
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        KeyCode.F11,
        KeyCode.F12,
        KeyCode.F13,
        KeyCode.F14,
        KeyCode.F15,
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Exclaim,
        KeyCode.DoubleQuote,
        KeyCode.Hash,
        KeyCode.Dollar,
        KeyCode.Ampersand,
        KeyCode.Quote,
        KeyCode.LeftParen,
        KeyCode.RightParen,
        KeyCode.Asterisk,
        KeyCode.Plus,
        KeyCode.Comma,
        KeyCode.Minus,
        KeyCode.Period,
        KeyCode.Slash,
        KeyCode.Colon,
        KeyCode.Semicolon,
        KeyCode.Less,
        KeyCode.Equals,
        KeyCode.Greater,
        KeyCode.Question,
        KeyCode.At,
        KeyCode.LeftBracket,
        KeyCode.Backslash,
        KeyCode.RightBracket,
        KeyCode.Caret,
        KeyCode.Underscore,
        KeyCode.BackQuote,
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z,
        KeyCode.Numlock,
        KeyCode.CapsLock,
        KeyCode.ScrollLock,
        KeyCode.RightShift,
        KeyCode.LeftShift,
        KeyCode.RightControl,
        KeyCode.LeftControl,
        KeyCode.RightAlt,
        KeyCode.LeftAlt,
        KeyCode.LeftCommand,
        KeyCode.LeftApple,
        KeyCode.LeftWindows,
        KeyCode.RightCommand,
        KeyCode.RightApple,
        KeyCode.RightWindows
    };
    private Dictionary<bool, Direction> waiting = new Dictionary<bool, Direction>();

    private enum Direction
    {
        up,
        down,
        left,
        right
    }

    void Start()
    {
        if (m_singleton == null)
        {
            m_singleton = this;
        }
        hudController = this.gameObject.GetComponent<HudController>();
        m_startRect = new Rect(Screen.width + 100, Screen.height / 2 - 25, 200, 50);
        m_titleRect = new Rect(0 - 400, 100, 400, 100);

        m_resumeRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 440, 200, 50);
        m_optionsRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 355, 200, 50);
        m_button3Rect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 270, 200, 50);
        m_button4Rect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 185, 200, 50);

        m_backRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 160, 200, 50);

        m_upRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 460, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_leftRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 390, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_downRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 320, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_rightRect = new Rect(m_menuBackgroundBounds.width / 2 - 100, m_menuBackgroundBounds.yMax - 250, OPTIONS_WIDTH, OPTIONS_HEIGHT);

        m_upLabelRect = new Rect(m_menuBackgroundBounds.width / 2 + 70, m_menuBackgroundBounds.yMax - 446, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_leftLabelRect = new Rect(m_menuBackgroundBounds.width / 2 + 70, m_menuBackgroundBounds.yMax - 376, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_downLabelRect = new Rect(m_menuBackgroundBounds.width / 2 + 70, m_menuBackgroundBounds.yMax - 306, OPTIONS_WIDTH, OPTIONS_HEIGHT);
        m_rightLabelRect = new Rect(m_menuBackgroundBounds.width / 2 + 70, m_menuBackgroundBounds.yMax - 236, OPTIONS_WIDTH, OPTIONS_HEIGHT);

        screenHeight = Screen.height;
        screenWidth = Screen.width;

        m_positionButtonsGroupOne = 0 - 200;
        m_positionButtonsGroupTwo = Screen.width;

        m_menuBounds = new Rect(m_menuBackgroundBounds.x, -m_menuBackgroundBounds.height - 10f, m_menuBackgroundBounds.width, m_menuBackgroundBounds.height);

    }

    // Update is called once per frame
    void Update()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            m_startRect = new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50);
            m_titleRect = new Rect(Screen.width / 2 - 200, 100, 400, 100);
            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }
        if (Input.GetButtonDown("ButtonA") || Input.GetButtonDown("Start"))
        {
            m_isSelectButtonPressed = true;
        }
        else
        {
            m_isSelectButtonPressed = false;
        }

        if (waiting.ContainsKey(true))
        {
            changeMovementKey();
        }

        if (menuButtons[currentButtonIndex].Equals("Resume") && Input.GetButtonDown("ButtonA"))
        {
            GameManager.Resume();
        }
    }

    void OnGUI()
    {
        if (m_displayTitleScreen)
        {
            DrawTitleScreen();
        }
        else if (m_displayOptions)
        {
            DrawOptionsScreen();
        }
        else
        {
            DrawMainMenu();
        }

    }

    void changeMovementKey()
    {
        if (Input.anyKeyDown)
        {
            List<KeyCode> keys = new List<KeyCode>((KeyCode[])whitelist);
            foreach (KeyCode key in whitelist)
            {
                if (Input.GetKeyDown(key))
                {
                    switch (waiting[true])
                    {
                        case Direction.up:
                            if (key != InputManager.DOWN && key != InputManager.LEFT && key != InputManager.RIGHT)
                            {
                                InputManager.UP = key;
                                PlayerPrefs.SetString(InputManager.PREF_UP_KEY, key.ToString());
                            }
                            else return;
                            break;
                        case Direction.down:
                            if (key != InputManager.UP && key != InputManager.LEFT && key != InputManager.RIGHT)
                            {
                                InputManager.DOWN = key;
                                PlayerPrefs.SetString(InputManager.PREF_DOWN_KEY, key.ToString());
                            }
                            else return;
                            break;
                        case Direction.left:
                            if (key != InputManager.UP && key != InputManager.DOWN && key != InputManager.RIGHT)
                            {
                                InputManager.LEFT = key;
                                PlayerPrefs.SetString(InputManager.PREF_LEFT_KEY, key.ToString());
                            }
                            else return;
                            break;
                        case Direction.right:
                            if (key != InputManager.UP && key != InputManager.DOWN && key != InputManager.LEFT)
                            {
                                InputManager.RIGHT = key;
                                PlayerPrefs.SetString(InputManager.PREF_RIGHT_KEY, key.ToString());
                            }
                            else return;
                            break;
                    }
                    waiting.Remove(true);
                    break;
                }
            }
        }
    }
    void DrawTitleScreen()
    {

        if (!playedIntroSound)
        {
            AudioManager.m_singleton.PlayIntroScreen();
            playedIntroSound = true;
        }
        SlideDisplay();
        GUI.skin = skin;
        GUI.Label(m_titleRect, "JACKED", m_titleStyle);
        GUI.SetNextControlName("Start");
        if (PlayerCharacter.s_singleton.gamePad)
        {
            GUI.FocusControl("Start");
        }
        if (GUI.Button(m_startRect, "START", skin.GetStyle("Button")) || (GUI.GetNameOfFocusedControl().Equals("Start") && m_isSelectButtonPressed))
        {
            hudController.Display(true);
            InputManager.Enable(true);
            GameManager.s_singleton.Enable(true);
            DisplayTitleScreen(false);
        }
    }

    void DrawOptionsScreen()
    {
        if (m_display)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", HudController.s_singleton.m_overlayStyle);
            if (m_menuBounds.y < m_menuBackgroundBounds.y)
            {
                m_menuBounds.y += Screen.width / 35f;
            }
            else if (m_menuBounds.y >= m_menuBackgroundBounds.y)
            {
                m_menuBounds.y = m_menuBackgroundBounds.y;
            }
        }
        else
        {
            if (m_menuBounds.yMax > -10)
            {
                m_menuBounds.y -= Screen.width / 35f;
            }
            else if (m_menuBounds.yMax <= -10)
            {
                m_menuBounds.y = -m_menuBackgroundBounds.height - 10f;
            }
        }

        GUI.Box(m_menuBounds, "", m_menuBackgroundStyle);
        GUI.BeginGroup(m_menuBounds);

        GUI.SetNextControlName("Up");
        if (GUI.Button(m_upRect, "UP", skin.GetStyle("Button")))
            waiting[true] = Direction.up;
        GUI.Label(m_upLabelRect, InputManager.UP.ToString());

        GUI.SetNextControlName("Left");
        if (GUI.Button(m_leftRect, "LEFT", skin.GetStyle("Button")))
            waiting[true] = Direction.left;
        GUI.Label(m_leftLabelRect, InputManager.LEFT.ToString());

        GUI.SetNextControlName("Down");
        if (GUI.Button(m_downRect, "DOWN", skin.GetStyle("Button")))
            waiting[true] = Direction.down;
        GUI.Label(m_downLabelRect, InputManager.DOWN.ToString());

        GUI.SetNextControlName("Right");
        if (GUI.Button(m_rightRect, "Right", skin.GetStyle("Button")))
            waiting[true] = Direction.right;
        GUI.Label(m_rightLabelRect, InputManager.RIGHT.ToString());

        GUI.SetNextControlName("Back");
        if (GUI.Button(m_backRect, "BACK", skin.GetStyle("Button")))
        {
            m_displayOptions = false;
        }

        GUI.EndGroup();
    }

    void DrawMainMenu()
    {
        if (m_display)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", HudController.s_singleton.m_overlayStyle);
            if (m_menuBounds.y < m_menuBackgroundBounds.y)
            {
                m_menuBounds.y += Screen.width / 35f;
            }
            else if (m_menuBounds.y >= m_menuBackgroundBounds.y)
            {
                m_menuBounds.y = m_menuBackgroundBounds.y;
            }
        }
        else
        {
            if (m_menuBounds.yMax > -10)
            {
                m_menuBounds.y -= Screen.width / 35f;
            }
            else if (m_menuBounds.yMax <= -10)
            {
                m_menuBounds.y = -m_menuBackgroundBounds.height - 10f;
            }
        }

        GUI.Box(m_menuBounds, "", m_menuBackgroundStyle);
        GUI.BeginGroup(m_menuBounds);

        //GUI.BeginGroup(new Rect(m_positionButtonsGroupOne, Screen.height / 3, 200, 400));
        GUI.SetNextControlName("Resume");
        if (GUI.Button(m_resumeRect, "RESUME", skin.GetStyle("Button")))
        {
            GameManager.Resume();
        }
        GUI.SetNextControlName("Button1");
        GUI.Button(m_button3Rect, "BUTTON", skin.GetStyle("Button"));
        //GUI.EndGroup();


        //GUI.BeginGroup(new Rect(m_positionButtonsGroupTwo, Screen.height / 3, 200, 400));
        GUI.SetNextControlName("Options");
        if (GUI.Button(m_optionsRect, "OPTIONS", skin.GetStyle("Button")))
        {
            m_displayOptions = true;
            DrawOptionsScreen();
        }

        GUI.SetNextControlName("Quit");
        if (GUI.Button(m_button4Rect, "QUIT", skin.GetStyle("Button")))
        {
            // Will quit game in production but not test
            Application.Quit();
        }
        GUI.EndGroup();
        //GUI.EndGroup();
        /*if (m_display) {
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
        }*/

        if (PlayerCharacter.s_singleton.gamePad)
        {
            GUI.FocusControl(menuButtons[currentButtonIndex]);
            m_menuButtonSwitchTimeBuffer += 1;
            if (Input.GetAxis("VerticalGamepad") == 1 && m_menuButtonSwitchTimeBuffer > k_menuButtonMinSwitchTime)
            {
                if (currentButtonIndex == menuButtons.Count - 1)
                {
                    currentButtonIndex = 0;
                }
                else
                {
                    currentButtonIndex++;
                }
                GUI.FocusControl(menuButtons[currentButtonIndex]);
                m_menuButtonSwitchTimeBuffer = 0;
            }
            else if (Input.GetAxis("VerticalGamepad") == -1 && m_menuButtonSwitchTimeBuffer > k_menuButtonMinSwitchTime)
            {
                if (currentButtonIndex == 0)
                {
                    currentButtonIndex = menuButtons.Count - 1;
                }
                else
                {
                    currentButtonIndex--;
                }
                GUI.FocusControl(menuButtons[currentButtonIndex]);
                m_menuButtonSwitchTimeBuffer = 0;
            }
        }
    }


    void SlideDisplay()
    { //MAKE THIS MORE REUSABLE AND LESS SPECIFIC
        float middleX = Screen.width / 2 - m_titleRect.width / 2;
        m_titleRect.x += 25;
        m_startRect.x -= 25;
        if (middleX - m_titleRect.x <= 0)
        {
            m_titleRect.x = middleX;
        }
        middleX = Screen.width / 2 - m_startRect.width / 2;
        if (m_startRect.x - middleX <= 0)
        {
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

    public static void DisplayOptionScreen(bool display)
    {
        MainMenu.m_singleton.m_displayOptions = display;
    }

    public static bool MainMenuDisplayed()
    {
        return MainMenu.m_singleton.m_display;
    }
}

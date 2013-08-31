using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudController : MonoBehaviour {

    public static HudController s_singleton;

    public GUIStyle m_textStyle;
    public GUIStyle m_tinyTextStyle;
    public GUIStyle m_largeFightingSpirit;
	public GUIStyle m_topGUIBar;
    public Texture2D m_heartTexture;
   

    public GameObject NotificationPrefab;
    public GameObject ScrollingNotificationPrefab;
    public GameObject PowerupNotification;

    public Texture progressTexture;

    private bool m_display = false;

    public Rect m_heartRateRect;
    public Rect m_scoreRect;
    private Rect m_xpRect;
    private float screenWidth, screenHeight;
    //These will be inside a GUI group, namely m_hearRateRect
    public Rect m_heartRect;
	private Rect m_defHeartRect;
    public Rect m_rateRect;
	private Rect m_controllerBoundsUpper;
	public Rect m_controllerBounds;
    private Rect m_progressBarBox;
    private Rect m_progressBarFill;
    private float m_actualProgressWidth;  //This is used to know how wide the progress bar will be after it stops moving
    private bool m_fillBarToTop; //If the level has incremented, we still want to fill the bar all the way before going back to 0
    private int m_mostRecentPlayerLevel = 1;
    private Rect m_xpLabelRect, m_levelRect;
    
    //Placeholders for real values
    private int heartRate = 100;
    private int playerXP = 0;
    private int score = 100000;
    //private float heartScale = 1f;

    private float m_lastBeat = 0.0f;
    private bool beatHeartUp = true;
    private float heartBeatDuration = 6.0f;

    private int MIN_HEART_SIZE, MAX_HEART_SIZE;
    private const float SECONDS_PER_MINUTE = 60.0f;
	
	public Rect m_progressBarBounds;
	public Rect m_xpBounds;
	public Rect m_skillBounds;
	public Rect m_levelBounds;
	
	public Rect m_skill1;
	public Rect m_skill2;
	public Rect m_skill3;
	public Rect m_skill4;
	private List<Rect> skillRects;
	
	public GUIStyle m_xpStyle;
	public GUIStyle m_lvlStyle;
	public GUIStyle m_iconStyle;
	public GUIStyle m_overlayStyle;
	
	private SkillUI skillUI;
	
	//These rectangles more closely aproximate the part of the screen with HUD drawn on it
	Rect actualSkillRect;
	Rect actualHudRect;
	
	// Use this for initialization
	void Start () {
        if (!s_singleton)
        {
            s_singleton = this;
        }
		m_defHeartRect = m_heartRect;
        m_lastBeat = Time.time;
        MAX_HEART_SIZE = (int)m_heartRateRect.height;
        MIN_HEART_SIZE = (int)(m_heartRateRect.height * .75f);
        
        m_xpRect = new Rect(Screen.width - 225, 0, 100, 100);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
		//m_controllerBounds = new Rect(0, Screen.height - 100, Screen.width, 100);
        //m_controllerBoundsUpper = new Rect(screenWidth/4.0f, 0, screenWidth/2.0f, 100);
        //m_progressBarFill = m_progressBarBox = new Rect(50, 5, (screenWidth / 4.0f) - 10, 20);
		m_progressBarFill = m_progressBarBox = m_progressBarBounds;
		m_xpLabelRect = m_xpBounds;
		m_levelRect = m_levelBounds;
		m_controllerBoundsUpper = m_skillBounds;
        //m_xpLabelRect = new Rect(10, 5, 30, 20);
        //m_levelRect = new Rect((screenWidth / 4.0f) + 50, 5, (screenWidth / 4.0f) - 55, 20);
        m_progressBarFill.width = 0;
		skillRects = new List<Rect>();
		skillRects.Add(m_skill1);
		skillRects.Add(m_skill2);
		skillRects.Add(m_skill3);
		skillRects.Add(m_skill4);
	
		skillUI = gameObject.GetComponent<SkillUI>();
	}
	
	// Update is called once per frame
	void Update () {

        //Things to execute even if the game is paused
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            m_scoreRect = new Rect(Screen.width - 100, 0, 100, 100);
            //m_xpRect = new Rect(Screen.width - 225, 0, 100, 100);
            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }

		m_progressBarFill.x = m_progressBarBounds.x;
		m_progressBarFill.y = m_progressBarBounds.y;
		m_progressBarFill.height = m_progressBarBounds.height;
		
        if (m_mostRecentPlayerLevel < PlayerCharacter.s_singleton.currentlevel)
        {
            m_mostRecentPlayerLevel = PlayerCharacter.s_singleton.currentlevel;
            m_fillBarToTop = true;
        }

        m_actualProgressWidth = ((PlayerCharacter.s_singleton.experiencePoints * 1.0f) / (PlayerCharacter.s_singleton.PointsToNextLevel() * 1.0f)) * (m_progressBarBox.width);
        if (m_actualProgressWidth > m_progressBarFill.width)
        {
            m_progressBarFill.width++;
        }
        else if (m_fillBarToTop)
        {
            m_progressBarFill.width++;
            if (m_progressBarFill.width >= m_progressBarBox.width)
            {
                m_fillBarToTop = false;
            }
        }
        else
        {
            m_progressBarFill.width = m_actualProgressWidth;
        }

        //Everything after this will not be executed if the game is paused
        if (GameManager.IsPaused())
        {
            return;
        }

        if (m_display)
        {
            score = (int)GameManager.s_singleton.m_score;
            heartRate = PlayerCharacter.s_singleton.m_heartRate;
            playerXP = PlayerCharacter.s_singleton.experiencePoints;
            //heartScale = (heartRate * 1.0f) / 300.0f;
            BeatHeart();
        }
		
		//These are used when deciding if the mouse is currently over the hud
		actualSkillRect = new Rect(m_skill1.x - 15, 0 ,m_skill4.xMax - (m_skill1.x -30 ), m_progressBarBounds.yMax + 10);
		actualHudRect = m_controllerBounds;
		actualHudRect.height -= 75;
		actualHudRect.y += 75;
	}

    void OnGUI()
    {
        if (m_display)
        {
            GUI.Box(m_controllerBounds,"", m_topGUIBar);
            GUI.BeginGroup(m_controllerBounds);
            GUI.DrawTexture(m_heartRect, m_heartTexture);
            GUI.Label(m_rateRect, heartRate.ToString() + " BPM", m_textStyle);
/*
           
            GUI.EndGroup();

            GUI.Label(m_scoreRect, score.ToString(), m_textStyle);
            
*/
            //GUI.Label(m_xpRect, playerXP.ToString() + " XP", m_textStyle);
            GUI.Label(m_scoreRect, score.ToString(), m_textStyle);
			GUI.EndGroup();

            //Skill Box
            GUI.Box(m_skillBounds, "", m_xpStyle);
            //GUI.BeginGroup(m_controllerBoundsUpper);
            GUI.Box(m_progressBarBounds, "");
            GUI.DrawTexture(m_progressBarFill, progressTexture);
            GUI.Label(m_progressBarBounds, "", m_tinyTextStyle);
            //GUI.Label(m_xpBounds, "XP: ");
            GUI.Label(m_levelBounds, "Lvl " + PlayerCharacter.s_singleton.currentlevel, m_lvlStyle);
            //GUI.EndGroup();
			
			List<Skill> curSkills = PlayerCharacter.s_singleton.getCurrentSkills();
			int i = 0;
			foreach (Skill s in curSkills) {
				if (GUI.Button(skillRects[i], "", m_iconStyle)) {
					skillUI.slide(!skillUI.open);
				}
				if (s != null) {
					Rect iconRect = skillRects[i];
					iconRect.width -=4;
					iconRect.height -=4;
					iconRect.x +=2;
					iconRect.y +=2;
					GUI.DrawTexture(iconRect, s.icon);
				} 
					
				i++;
			}
        }
    }

    private void BeatHeart()
    {
        if ((Time.time - m_lastBeat) > SECONDS_PER_MINUTE / (float)heartRate)
        {
            m_lastBeat = Time.time;
            beatHeartUp = true;
            AudioManager.m_singleton.PlayHeartBeat();
            if (heartRate > PlayerCharacter.s_singleton.GetMaxHeartRate() * 5.0f / 6.0f)
            {
                CameraController.s_singleton.PulseCamera(5);
            }
            else if (heartRate > PlayerCharacter.s_singleton.GetMaxHeartRate() * 2.0f / 3.0f)
            {
                CameraController.s_singleton.PulseCamera(3);
            }
            else if (heartRate > PlayerCharacter.s_singleton.GetMaxHeartRate() / 2.0f)
            {
                CameraController.s_singleton.PulseCamera(1);
            }
            else if (heartRate < 90)
            {
                CameraController.s_singleton.PulseCamera(1);
            }
            else if (heartRate < 70)
            {
                CameraController.s_singleton.PulseCamera(3);
            }
            else if (heartRate < 45)
            {
                CameraController.s_singleton.PulseCamera(5);
            }
        }

        if (beatHeartUp)
        {
            m_heartRect.width += ((float)(MAX_HEART_SIZE - MIN_HEART_SIZE)) / heartBeatDuration;
            m_heartRect.height += ((float)(MAX_HEART_SIZE - MIN_HEART_SIZE)) / heartBeatDuration;
            if (m_heartRect.height >= MAX_HEART_SIZE)
            {
                beatHeartUp = false;
            }
        }
        else if (m_heartRect.height > MIN_HEART_SIZE)
        {
            m_heartRect.width -= ((float)(MAX_HEART_SIZE - MIN_HEART_SIZE)) / heartBeatDuration;
            m_heartRect.height -= ((float)(MAX_HEART_SIZE - MIN_HEART_SIZE)) / heartBeatDuration;
        }

        m_heartRect.y = m_defHeartRect.y + (m_heartRect.height) / 2.0f;
        m_heartRect.x = m_defHeartRect.x - (m_heartRect.width) / 2.0f;
    }

    public void Display(bool display)
    {
        m_display = display;
    }
	
	public bool MouseOverHud() {
		Vector2 screenMousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		
		return actualHudRect.Contains(screenMousePos) || actualSkillRect .Contains(screenMousePos);	
	}
}

using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

    public static HudController s_singleton;

    public GUIStyle m_textStyle;
    public GUIStyle m_largeFightingSpirit;
	public GUIStyle m_topGUIBar;
    public Texture2D m_heartTexture;
   

    public GameObject NotificationPrefab;
    public GameObject ScrollingNotificationPrefab;
    public GameObject PowerupNotification;

    private bool m_display = false;

    private Rect m_heartRateRect = new Rect(0,0,300,100);
    private Rect m_scoreRect;
    private float screenWidth, screenHeight;
    //These will be inside a GUI group, namely m_hearRateRect
    private Rect m_heartRect= new Rect(0,0,100,100);
    private Rect m_rateRect = new Rect(100,20,150,80);
    
    //Placeholders for real values
    private int heartRate = 100;
    private int score = 100000;
    //private float heartScale = 1f;

    private float m_lastBeat = 0.0f;
    private bool beatHeartUp = true;
    private float heartBeatDuration = 6.0f;

    private int MIN_HEART_SIZE, MAX_HEART_SIZE;
    private const float SECONDS_PER_MINUTE = 60.0f;

	// Use this for initialization
	void Start () {
        if (!s_singleton)
        {
            s_singleton = this;
        }
        m_lastBeat = Time.time;
        MAX_HEART_SIZE = (int)m_heartRateRect.height;
        MIN_HEART_SIZE = (int)(m_heartRateRect.height * .75f);
        m_scoreRect = new Rect(Screen.width - 100, 0, 100, 100);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
        //Things to execute even if the game is paused
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            m_scoreRect = new Rect(Screen.width - 100, 0, 100, 100);
            screenHeight = Screen.height;
            screenWidth = Screen.width;
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
            //heartScale = (heartRate * 1.0f) / 300.0f;
            BeatHeart();
        }
	}

    void OnGUI()
    {
        if (m_display)
        {
            GUI.Box(new Rect(0, 0, screenWidth, 100),"", m_topGUIBar);
            GUI.BeginGroup(m_heartRateRect);
            GUI.DrawTexture(m_heartRect, m_heartTexture);
            GUI.Label(m_rateRect, heartRate.ToString() + " BPM", m_textStyle);
            GUI.EndGroup();

            GUI.Label(m_scoreRect, score.ToString(), m_textStyle);
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

        m_heartRect.y = ((float)(MAX_HEART_SIZE - m_heartRect.height)) / 2.0f;
        m_heartRect.x = ((float)(MAX_HEART_SIZE - m_heartRect.width)) / 2.0f;
    }

    public void Display(bool display)
    {
        m_display = display;
    }
}

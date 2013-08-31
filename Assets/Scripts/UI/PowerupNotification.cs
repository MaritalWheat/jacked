using UnityEngine;
using System.Collections;

public class PowerupNotification : MonoBehaviour {

    public GUIStyle m_style;
    public Font m_smallFont, m_mediumFont;


    private string m_message;
    private Rect m_notificationRect;
    private float m_duration;
    //private float m_startTime;
    private bool m_display = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.IsPaused())
        {
            return;
        }
        if (m_display)
        {
            if (m_duration <= 0)
            {
                Destroy(this.gameObject);
            }

            if (m_notificationRect.y < HudController.s_singleton.m_progressBarBounds.yMax + 10)
            {
                m_notificationRect.y += 5;
            }
            else
            {
                m_duration -= Time.deltaTime;
				m_style.normal.textColor = new Color(m_style.normal.textColor.r, m_style.normal.textColor.g, m_style.normal.textColor.b, m_style.normal.textColor.a - Time.deltaTime * 1.0f);
            }


        }
	}

    void OnGUI()
    {
        if (GameManager.IsPaused())
        {
            return;
        }
        if (m_display)
        {
            GUI.Label(m_notificationRect, m_message, m_style);
        }
    }

    public void DisplayLarge(string message, float duration ) {
        m_message = message;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width/2 - (size.x/2), Screen.height, size.x, size.y);
        m_duration = duration;
        //m_startTime = Time.time;
        m_display = true;
    }

    public void DisplaySmall(string message, float duration)
    {
        m_message = message;
        m_style.font = m_smallFont;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width / 2 - (size.x / 2), Screen.height, size.x, 50);
        m_duration = duration;
        //m_startTime = Time.time;
        m_display = true;
    }

    public void DisplayMedium(string message, float duration)
    {
        m_message = message;
        m_style.font = m_mediumFont;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width / 2 - (size.x / 2), 0, size.x, size.y);
        m_duration = duration;
        //m_startTime = Time.time;
        m_display = true;
    }
}

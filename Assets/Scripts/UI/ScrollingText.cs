using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {

    private GUIStyle m_style;

    private string m_message;
    private Rect m_notificationRect;
    private float m_speed;
    private bool m_display = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.s_singleton.IsPaused())
        {
            return;
        }
        if (m_display)
        {
            if (m_notificationRect.x + m_notificationRect.width < 0)
            {
                Destroy(this.gameObject);
            }
            m_notificationRect.x -= (m_speed);
        }
	}

    void OnGUI()
    {
        if (GameManager.s_singleton.IsPaused())
        {
            return;
        }
        if (m_display)
        {
            GUI.Label(m_notificationRect, m_message, m_style);
        }
    }

    public void Display(string message, float speed, GUIStyle style ) {
        m_style = style;
        m_message = message;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width, Screen.height/2-(size.y/2), size.x, size.y);
        m_speed = speed;
        m_display = true;
    }

    public void DisplayWithSound(string message, float speed, AudioClip clipToPlay)
    {
        m_message = message;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width, Screen.height / 2 - (size.y / 2), size.x, size.y);
        m_speed = speed;
        m_display = true;
    }
}
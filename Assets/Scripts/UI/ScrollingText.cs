using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {

    private GUIStyle m_style;

    private string m_message;
    private Rect m_notificationRect;
    private float m_speed;
    private bool m_display = false;
	private float m_fontSize;
	private float m_duration;


	// Use this for initialization
	void Start () {
		//m_duration = 5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.IsPaused())
        {
            return;
        }
        if (m_display)
        {
            if (m_duration < 0)
            {
                Destroy(this.gameObject);
            }
            //m_notificationRect.x -= (m_speed);
			//m_notificationRect.width += m_speed;
			if (m_duration > .5f) {
				m_fontSize += m_speed * 5 * Time.deltaTime;
			} else {
				m_fontSize -= m_speed * 5 * Time.deltaTime;
			}
			
			m_duration -= Time.deltaTime;
			//} else { 
			//	m_fontSize -= m_speed;
			//}
			//m_notificationRect.height += m_speed;
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
			if (m_fontSize <= 1) m_fontSize = 1;
			m_style.fontSize = (int)m_fontSize;
            GUI.Label(m_notificationRect, m_message, m_style);
        }
    }

    public void Display(string message, float speed, GUIStyle style ) {
        m_style = style;
        m_message = message;
		
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(Screen.width / 2 - size.x / 2, Screen.height/2-(size.y/2), size.x, size.y);
		m_style.fontSize = 1;
		m_fontSize = m_style.fontSize;
        m_speed = speed;
		m_duration = 1f;
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
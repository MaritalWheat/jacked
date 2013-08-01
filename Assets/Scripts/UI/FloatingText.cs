using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

    public GUIStyle m_style;

    private string m_message;
    private Rect m_notificationRect;
    private float m_duration;
    private float m_startTime;
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
            if (Time.time - m_startTime > m_duration)
            {
                Destroy(this.gameObject);
            }
            m_notificationRect.y -= Time.deltaTime*250.0f;
            m_style.normal.textColor = new Color(m_style.normal.textColor.r, m_style.normal.textColor.g, m_style.normal.textColor.b, m_style.normal.textColor.a - Time.deltaTime * 1.0f);

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

    public void Display(string message, Vector2 location, float duration ) {
        m_message = message;
        Vector2 size = m_style.CalcSize(new GUIContent(message));
        m_notificationRect = new Rect(location.x, location.y, size.x, size.y);
        m_duration = duration;
        m_startTime = Time.time;
        m_display = true;
    }
}

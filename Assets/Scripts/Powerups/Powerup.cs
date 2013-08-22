using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Powerup : MonoBehaviour {

    public List<Modifier> Modifiers;
    public float timeOnscreen;
    public string notificationMessage;
    public float m_effectDuration;
    public bool reverseOnComplete;

    private float m_timeUntilDestruction;

    private float m_timeLeft;
    private bool m_executed = false;

    private SpriteAnimation m_anim;
    private SpriteAnimationManager m_animManager;

	// Use this for initialization
	void Start () {
        m_anim = gameObject.GetComponent<SpriteAnimation>();
        m_animManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_animManager.SetSpriteAnimation(m_anim);
        m_timeUntilDestruction = timeOnscreen;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.IsPaused())
        {
            return;
        }
        if (m_timeLeft <= 0 && m_executed)
        {
            if (reverseOnComplete)
            {
                foreach (Modifier mod in Modifiers)
                {
                    mod.Reverse();
                }
            }

            Destroy(this.gameObject);
        }
        else if (m_timeUntilDestruction <= 0 && !m_executed)
        {
            Destroy(gameObject);
        }

        m_timeUntilDestruction -= Time.deltaTime;
        m_timeLeft -= Time.deltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            GameObject notification = (GameObject)Instantiate(HudController.s_singleton.PowerupNotification);
            notification.GetComponent<PowerupNotification>().DisplayLarge(notificationMessage, 3.0f);

            foreach (Modifier mod in Modifiers)
            {
                mod.Execute();
            }

            m_timeLeft = m_effectDuration;
            m_executed = true;
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            Destroy(this.gameObject.GetComponent<BoxCollider>());
            Destroy(m_anim);
            Destroy(m_animManager);
        }
        
    }
}

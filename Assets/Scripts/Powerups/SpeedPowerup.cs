using UnityEngine;
using System.Collections;

public class SpeedPowerup : MonoBehaviour {

    public float m_speedIncrease;
    public float m_effectDuration;

    private float m_timeUntilDestruction = 10.0f;

    private float m_timeLeft;
    private bool m_executed = false;

    private SpriteAnimation m_anim;
    private SpriteAnimationManager m_animManager;

	// Use this for initialization
	void Start () {
        m_anim = gameObject.GetComponent<SpriteAnimation>();
        m_animManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_animManager.SetSpriteAnimation(m_anim);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.IsPaused())
        {
            return;
        }
        if (m_timeLeft <= 0 && m_executed)
        {
            PlayerCharacter.s_singleton.m_maxCharacterSpeed -= m_speedIncrease;
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
            notification.GetComponent<PowerupNotification>().DisplayLarge("Speed Increased", 3.0f);
            PlayerCharacter.s_singleton.m_maxCharacterSpeed += m_speedIncrease;
            m_timeLeft = m_effectDuration;
            m_executed = true;
            Destroy(this.gameObject.GetComponent<MeshRenderer>());
            Destroy(this.gameObject.GetComponent<BoxCollider>());
            Destroy(m_anim);
            Destroy(m_animManager);
        }
        
    }
}

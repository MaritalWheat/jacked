using UnityEngine;
using System.Collections;

public class HeartRatePowerup : MonoBehaviour
{

    public int m_heartRateModifier;

    private float m_timeUntilDestruction = 10.0f;


    private SpriteAnimation m_anim;
    private SpriteAnimationManager m_animManager;

    // Use this for initialization
    void Start()
    {
        m_anim = gameObject.GetComponent<SpriteAnimation>();
        m_animManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_animManager.SetSpriteAnimation(m_anim);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_singleton.IsPaused())
        {
            return;
        }
        
        else if (m_timeUntilDestruction <= 0)
        {
            Destroy(gameObject);
        }

        m_timeUntilDestruction -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            PlayerCharacter.s_singleton.m_heartRate += m_heartRateModifier;
            Destroy(this.gameObject);
        }

    }
}

using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

    protected PlayerCharacter m_playerCharacter;
    protected CharacterController m_characterController;
    protected bool m_isHit;
    protected float m_timeBuffer;
    protected float starting_y;
    protected int m_experiencePoints = 1;
    
	protected float m_baseSpeed;
    public float m_speed;
    public int m_damage;
    public int m_health;
      
    protected SpriteAnimationManager m_animationManager;
    public SpriteAnimation m_idleLeftAnimation;
    public SpriteAnimation m_idleRightAnimation;
    public SpriteAnimation m_walkLeftAnimation;
    public SpriteAnimation m_walkRightAnimation;
    
	protected void Start () {
        OnStart();
	}

	protected void Update () {
        if (GameManager.IsPaused()) {
            return;
        }
        OnUpdate();
	}

    public virtual void OnStart() {
		EnemyManager.Enemies.Add(this);
		m_baseSpeed = m_speed;
        m_playerCharacter = PlayerCharacter.s_singleton;
        m_characterController = gameObject.GetComponent<CharacterController>();
        m_animationManager = GetComponent<SpriteAnimationManager>();
        m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
        starting_y = transform.position.y;
    }

    public virtual void OnUpdate() {
    }

    protected void OnTriggerEnter(Collider collider) {
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        //Debug.Log(bullet);
        if (bullet) {
            bullet.Destruct();
            GetDamaged(bullet.m_damage);
        }
        else {
            GameObject hitObject = collider.gameObject;
            if (hitObject != gameObject) {
                //Debug.Log(collider.gameObject);
                if (hitObject == m_playerCharacter.gameObject && !m_isHit) {
                    //Debug.Log ("got here");
                    DamagePlayer();
                    SetHit(true);
                }
            }
            if (m_isHit) {
                return;
            }
        }
    }

    protected void DamagePlayer() {
        m_playerCharacter.Damage(m_damage);
    }

    public virtual void GetDamaged(int damage){
        //Debug.Log("Health is " + m_health + ", Damage is " + damage);
        m_health -= damage;
        GameObject notification = (GameObject)Instantiate(HudController.s_singleton.NotificationPrefab);
        Vector3 notificationPos = Camera.main.WorldToScreenPoint(transform.position);
        notification.GetComponent<FloatingText>().Display("+" + damage.ToString(), new Vector2(notificationPos.x, Screen.height - notificationPos.y), 3.0f);
        if (m_health <= 0) {
			EnemyManager.Enemies.Remove(this);
            m_playerCharacter.experiencePoints += m_experiencePoints;
            GameManager.s_singleton.m_creaturesKilled++;
            GameManager.s_singleton.SetSpreeStatus(true);
        }
	}

    protected void SetHit(bool hit) {
        m_isHit = hit;
    }

	public float GetBaseSpeed() {
		return m_baseSpeed;
	}
}

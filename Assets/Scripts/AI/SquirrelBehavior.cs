using UnityEngine;
using System.Collections;


public class SquirrelBehavior : EnemyBehavior {
	
    public float m_pauseAfterHit;
    private SpriteAnimation m_idleAnimation;
	
	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}

    public override void OnStart() {
        base.OnStart();
        m_idleAnimation = m_idleLeftAnimation;
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (!m_isHit) {
            Vector3 direction = m_playerCharacter.transform.position - transform.position;
            if (direction.x <= 0) {
                m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
            }
            else {
                m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
            }
            MoveTowardsPlayer();
        }
        else {
            m_animationManager.SetSpriteAnimation(m_idleAnimation);
            m_timeBuffer += Time.deltaTime;
            if (m_timeBuffer > m_pauseAfterHit) {
                SetHit(false);
                m_timeBuffer = 0;
            }
        }
    }
	void MoveTowardsPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed * Time.deltaTime);
	}
	
	public override void GetDamaged(int damage){
        base.GetDamaged(damage);
        if (m_health <= 0)
        {
            GameManager.s_singleton.m_score += 100;
            Vector3 deathPos = transform.position;
			deathPos.y = 0.001f;
            GameManager.s_singleton.SpawnDeathObject(deathPos);
            Destroy(gameObject);
        }
	}


    void OnTriggerStay(Collider collider)
    {
        if (m_isHit)
        {
            return;
        }
        Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        if (bullet)
        {
            GetDamaged(bullet.m_damage);
            bullet.Destruct();
        }
    }
}

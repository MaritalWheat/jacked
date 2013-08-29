using UnityEngine;
using System.Collections;


public class CatBehavior : EnemyBehavior {
	
    private float m_timeSinceLast;	
    public int m_drainRange;
    private Vector3 m_moveDirection;
	private ParticleSystem m_particleSystem;
	private float m_time;
	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}

    public override void OnStart() {
        base.OnStart();
        m_particleSystem = gameObject.GetComponent<ParticleSystem>();
        m_time = 0;
        m_timeSinceLast = 0;
    }

    public override void OnUpdate() {
        base.OnUpdate();
        m_time += Time.deltaTime;
        bool inRange = WithinRange();
        if (inRange) {
            Vector3 direction = m_playerCharacter.transform.position - transform.position;
            if (direction.x <= 0) {
                m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
            }
            else {
                m_animationManager.SetSpriteAnimation(m_idleRightAnimation);
            }
            if (m_time - m_timeSinceLast >= .15) {
                m_particleSystem.Play();
                DamagePlayer();
                m_timeSinceLast = m_time;
            }
        }
        else {
            Vector3 direction = m_playerCharacter.transform.position - transform.position;
            if (direction.x <= 0) {
                m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
            }
            else {
                m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
            }
            m_particleSystem.Stop();
            MoveAround();
        }
    }

	void MoveAround(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed * Time.deltaTime);
	}
	
	public override void GetDamaged(int damage){
        base.GetDamaged(damage);
        if (m_health <= 0)
        {
            GameManager.s_singleton.m_catsKilled++;
            GameManager.s_singleton.m_score += 200;
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
        //Debug.Log(bullet);
        if (bullet)
        {
            GetDamaged(bullet.m_damage);
            bullet.Destruct();
        }
    }

	void setHit(bool hit){
		m_isHit = hit;
	}
	
	bool WithinRange(){
        float distance = Vector3.Distance(transform.position, m_playerCharacter.transform.position);
		if (distance <= m_drainRange){ //if in range of slide...arbitrary for now
				return true;
		}
		return false;
	}
}

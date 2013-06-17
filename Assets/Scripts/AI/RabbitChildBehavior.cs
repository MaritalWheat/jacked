using UnityEngine;
using System.Collections;


public class RabbitChildBehavior : EnemyBehavior {

    public float m_pauseAfterHit;

	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
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
            GameManager.s_singleton.SpawnDeathObject(transform.position);
            Destroy(gameObject);
            DamagePlayer();

        }
    }
	
	void MoveTowardsPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed);
	}
	
	public override void GetDamaged(int damage){
        base.GetDamaged(damage);
        if (m_health <= 0)
        {
            GameManager.s_singleton.m_score += 50;
            GameManager.s_singleton.SpawnDeathObject(transform.position);
			Destroy(gameObject);
        }
	}
}

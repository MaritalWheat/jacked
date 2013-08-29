using UnityEngine;
using System.Collections;


public class RabbitBehavior : EnemyBehavior {

    public GameObject babyRabbitPrefab;
    public float m_pauseAfterHit;


	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
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
            //m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
            MoveTowardsPlayer();
        }
        else {
            Vector3 direction = m_playerCharacter.transform.position - transform.position;
            if (direction.x <= 0) {
                m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
            }
            else {
                m_animationManager.SetSpriteAnimation(m_idleRightAnimation);
            }
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


	void DieandMultiply(){
        Vector3 deathPos = transform.position;
		deathPos.y = 0.001f;
        GameManager.s_singleton.SpawnDeathObject(deathPos);

		//Object rabbitChild;
		for (int i = 0; i < 1; i++){
			GameObject baby = (GameObject)Instantiate(babyRabbitPrefab, transform.position, transform.rotation);
			baby.transform.parent = transform.parent;
		}
		Destroy (this.gameObject);
	}


    public override void GetDamaged(int damage) {
        base.GetDamaged(damage);
        if (m_health <= 0) {
            GameManager.s_singleton.m_score += 100;
            DieandMultiply();
        }
    }
}

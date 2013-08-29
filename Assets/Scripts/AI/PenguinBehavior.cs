using UnityEngine;
using System.Collections;


public class PenguinBehavior : EnemyBehavior {

    public SpriteAnimation m_slideRightAnimation;
    public SpriteAnimation m_slideLeftAnimation;
    public float m_slideSpeed;	
    public int m_slideRange;
	public int m_retreatDistance;
	private Vector3 target;
    private bool m_slideStarted;
	private float slideTime;
    //private SpriteAnimation m_idleAnimation;

	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}

    public override void OnStart() {
 	    base.OnStart();
        //m_experiencePoints = 4;
        //m_idleAnimation = m_idleLeftAnimation;
    }

    public override void OnUpdate() {
        base.OnUpdate();
        if (m_isHit && (DistanceToPlayer() < m_retreatDistance)) {
            Vector3 direction = m_playerCharacter.transform.position - transform.position;
            if (direction.x <= 0) {
                m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
            }
            else {
                m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
            }
            MoveFromPlayer();
        }
        else if (m_isHit && (DistanceToPlayer() >= m_retreatDistance)) {
            SetHit(false);
        }
        else if (!WithinRange()) {
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
            if (!m_slideStarted) {
                Vector3 direction_tmp = m_playerCharacter.transform.position - transform.position;
                if (direction_tmp.x <= 0) {
                    m_animationManager.SetSpriteAnimation(m_slideLeftAnimation);
                }
                else {
                    m_animationManager.SetSpriteAnimation(m_slideRightAnimation);
                }
                //Debug.Log(m_slideStarted);
                Vector3 direction = m_playerCharacter.transform.position - transform.position;
                direction.Normalize();
                direction.y = 0;
                target = direction;
                switchSlideFlag(true);
                slideTime = 0;
            }
            else {
                if (slideTime > .40) {
                    Vector3 direction = m_playerCharacter.transform.position - transform.position;
                    if (direction.x <= 0) {
                        m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
                    }
                    else {
                        m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
                    }
                    MoveFromPlayer();
                }
                else {
                    Vector3 direction_tmp = m_playerCharacter.transform.position - transform.position;
                    if (direction_tmp.x <= 0) {
                        m_animationManager.SetSpriteAnimation(m_slideLeftAnimation);
                    }
                    else {
                        m_animationManager.SetSpriteAnimation(m_slideRightAnimation);
                    }
                    SlideTowardsPlayer(target);
                    //Debug.Log (slideTime);
                    //Debug.Log ("Sliding");
                }
            }
        }
    }
    
    new void OnTriggerEnter(Collider collider) {
		Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        //Debug.Log(bullet);
        if (bullet)
        {
            GetDamaged(bullet.m_damage);
            bullet.Destruct();
        }
		GameObject hitObject = collider.gameObject;
        if (hitObject != gameObject) {
            //Debug.Log(collider.gameObject);
			if (hitObject == m_playerCharacter.gameObject  && !m_isHit){
				//Debug.Log ("got here");
				DamagePlayer ();
				SetHit (true);
				switchSlideFlag (false);
			}
        } 
		if(m_isHit){
			return;
		}
		
    }
	
    void OnTriggerCollider(Collider collider) {
		if(m_isHit){
			return;
		}
    }

    void OnTriggerStay(Collider collider) {
		if(m_isHit){
			return;
		}
    }
	
	void MoveTowardsPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
        m_characterController.Move(direction * m_speed * Time.deltaTime);
		switchSlideFlag(false);
	}
	
	void SlideTowardsPlayer(Vector3 direction){	
		/*Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;*/
		slideTime += Time.deltaTime;
        m_characterController.Move(direction * m_slideSpeed * Time.deltaTime);
	}
	
	void MoveFromPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		direction.x = -direction.x;
		direction.z = -direction.z;
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
	
	bool WithinRange(){
        float distance = Vector3.Distance(transform.position, m_playerCharacter.transform.position);
		if (distance <= m_slideRange){ //if in range of slide...arbitrary for now
				return true;
		}
		return false;
	}
	
	float DistanceToPlayer(){
		return Vector3.Distance(transform.position, m_playerCharacter.transform.position);
	}
	
	void switchSlideFlag(bool slideStatus){
		m_slideStarted = slideStatus;
	}
}

using UnityEngine;
using System.Collections;


public class PenguinBehavior : MonoBehaviour {

    private PlayerCharacter m_playerCharacter;
	private CharacterController m_characterController;
	public float m_slideSpeed;
	public float m_speed;
	public int m_damage;
	public int m_health;
	public int m_slideRange;
	public int m_retreatDistance;
	
	private bool m_isHit;
	private bool m_slideStarted;
	private Vector3 target;
	private float slideTime;
	private float starting_y;
	
	public SpriteAnimation m_idleAnimation;

    public SpriteAnimation m_walkLeftAnimation;
    public SpriteAnimation m_slideLeftAnimation;
    public SpriteAnimation m_walkRightAnimation;
    public SpriteAnimation m_slideRightAnimation;
	
	private SpriteAnimationManager m_animationManager;
	
	// Use this for initialization
	void Start () {
        m_playerCharacter = PlayerCharacter.s_singleton;
		m_characterController = gameObject.GetComponent<CharacterController>();
		m_slideSpeed = 1f;
		m_speed = .25f;
		m_damage = 5;
		m_health = 10;
		m_slideRange = 25;
		m_retreatDistance = 30;
		
		m_animationManager = GetComponent<SpriteAnimationManager>();
        m_animationManager.SetSpriteAnimation(m_idleAnimation);
		starting_y = transform.position.y;
	}
	
	//Update is called once per frame
	//Penguin will move slowly towards player, slide towards player upon entering m_slideRange; if 
	//hits player will retreat m_retreatDistance
	void Update () {
        if (GameManager.s_singleton.IsPaused())
        {
            return;
        }
		if (m_isHit && (DistanceToPlayer() < m_retreatDistance)){
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
			}
			MoveFromPlayer();
		} else if (m_isHit && (DistanceToPlayer() >= m_retreatDistance)){
			setHit (false);
		} else if(!WithinRange()){
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
			}
			MoveTowardsPlayer();
		} else {
			if(!m_slideStarted){
				Vector3 direction_tmp = m_playerCharacter.transform.position - transform.position;
				if(direction_tmp.x <= 0){
					m_animationManager.SetSpriteAnimation(m_slideLeftAnimation);
				}else{
					m_animationManager.SetSpriteAnimation(m_slideRightAnimation);
				}
				Debug.Log (m_slideStarted);
				Vector3 direction = m_playerCharacter.transform.position - transform.position;
				direction.Normalize();
				direction.y = 0;
				target = direction;
				switchSlideFlag(true);
				slideTime = 0;
			}else{
				if(slideTime > .40){
					Vector3 direction = m_playerCharacter.transform.position - transform.position;
					if(direction.x <= 0){
						m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
					}else{
						m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
					}
					MoveFromPlayer ();	
				}else{
					Vector3 direction_tmp = m_playerCharacter.transform.position - transform.position;
					if(direction_tmp.x <= 0){
						m_animationManager.SetSpriteAnimation(m_slideLeftAnimation);
					}else{
						m_animationManager.SetSpriteAnimation(m_slideRightAnimation);
					}
					SlideTowardsPlayer (target);
					//Debug.Log (slideTime);
					//Debug.Log ("Sliding");
				}
			}
		}
	}
	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}

    void OnTriggerEnter(Collider collider) {
		Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        //Debug.Log(bullet);
        if (bullet)
        {
            getDamaged(bullet.m_damage);
            bullet.Destruct();
        }
		GameObject hitObject = collider.gameObject;
        if (hitObject != gameObject) {
            //Debug.Log(collider.gameObject);
			if (hitObject == m_playerCharacter.gameObject  && !m_isHit){
				//Debug.Log ("got here");
				DamagePlayer ();
				setHit (true);
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
	
	void setHit(bool hit){
		m_isHit = hit;
	}
	
	void MoveTowardsPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed);
		switchSlideFlag(false);
	}
	
	void SlideTowardsPlayer(Vector3 direction){	
		/*Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;*/
		slideTime += Time.deltaTime;
		m_characterController.Move(direction * m_slideSpeed);
	}
	
	void MoveFromPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		direction.x = -direction.x;
		direction.z = -direction.z;
		m_characterController.Move(direction * m_speed);
	}
	
	void DamagePlayer(){
		m_playerCharacter.Damage(m_damage);
	}
	
	public void getDamaged(int damage){
        m_health -= damage;
        GameObject notification = (GameObject)Instantiate(HudController.s_singleton.NotificationPrefab);
        Vector3 notificationPos = Camera.main.WorldToScreenPoint(transform.position);
        notification.GetComponent<FloatingText>().Display(damage.ToString(), new Vector2(notificationPos.x, Screen.height - notificationPos.y), 3.0f);

        if (m_health <= 0)
        {
            GameManager.s_singleton.m_creaturesKilled++;
            GameManager.s_singleton.m_score += 100;
            GameManager.s_singleton.SpawnDeathObject(transform.position);
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

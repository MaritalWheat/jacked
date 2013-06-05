using UnityEngine;
using System.Collections;


public class RabbitChildBehavior : MonoBehaviour {

	public float m_speed;
	public int m_damage;
	public int m_health;
	public float m_pauseAfterHit;
	
	private PlayerCharacter m_playerCharacter;
	private CharacterController m_characterController;
	private bool m_isHit;
	private float m_timeBuffer;
	
    public SpriteAnimation m_idleLeftAnimation;
    public SpriteAnimation m_idleRightAnimation;
    public SpriteAnimation m_walkLeftAnimation;
    public SpriteAnimation m_walkRightAnimation;
	private SpriteAnimationManager m_animationManager;
	
	void Start () {
        m_playerCharacter = PlayerCharacter.s_singleton;
		m_characterController = gameObject.GetComponent<CharacterController>();
		m_speed = 1f;
		m_damage = 5;
		m_health = 10;
		m_timeBuffer = 0;
		m_pauseAfterHit = 1;
		
		m_animationManager = GetComponent<SpriteAnimationManager>();
        m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
	}
	
	void Update () {
		if(!m_isHit){
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
			}
			MoveTowardsPlayer();
		}else{
            GameManager.s_singleton.SpawnDeathObject(transform.position);
			Destroy (gameObject);
			DamagePlayer ();
				
		}
	}
	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}
	
	void MoveTowardsPlayer(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed);
	}
	
	void DamagePlayer(){
		m_playerCharacter.Damage(2 * m_damage);
	}
	
	public void getDamaged(int damage){
        m_health -= damage;
        GameObject notification = (GameObject)Instantiate(HudController.s_singleton.NotificationPrefab);
        Vector3 notificationPos = Camera.main.WorldToScreenPoint(transform.position);
        notification.GetComponent<FloatingText>().Display(damage.ToString(), new Vector2(notificationPos.x, Screen.height - notificationPos.y), 3.0f);

        if (m_health <= 0)
        {
            GameManager.s_singleton.m_creaturesKilled++;
            GameManager.s_singleton.m_score += 50;
            GameManager.s_singleton.SpawnDeathObject(transform.position);
			Destroy(gameObject);
        }
	}
	
    void OnTriggerEnter(Collider collider) {
		Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        if (bullet)
        {
			bullet.Destruct();
            Destroy (gameObject);  
        }
		GameObject hitObject = collider.gameObject;
        if (hitObject != gameObject) {
			if (hitObject == m_playerCharacter.gameObject  && !m_isHit){
				DamagePlayer ();
				setHit (true);
			}
        } 
		if(m_isHit){
			return;
		}
		
    }
	
	void setHit(bool hit){
		m_isHit = hit;
	}
}

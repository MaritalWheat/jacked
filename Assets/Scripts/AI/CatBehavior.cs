using UnityEngine;
using System.Collections;


public class CatBehavior : MonoBehaviour {


    private PlayerCharacter m_playerCharacter;
	private CharacterController m_characterController;
	public float m_speed;
	public int m_damage;
	public int m_health;
	public float m_pauseAfterHit;
	public int m_drainRange;
	
	private bool m_isHit;
	private float m_time;
	private float m_timeSinceLast;
	private float starting_y;
	
	public SpriteAnimation m_idleLeftAnimation;
    public SpriteAnimation m_idleRightAnimation;
    public SpriteAnimation m_walkLeftAnimation;
    public SpriteAnimation m_walkRightAnimation;
	private SpriteAnimationManager m_animationManager;
	private Vector3 m_moveDirection;
	private ParticleSystem m_particleSystem;
	
	// Use this for initialization
	void Start () {
        m_playerCharacter = PlayerCharacter.s_singleton;
		m_characterController = gameObject.GetComponent<CharacterController>();
		m_particleSystem = gameObject.GetComponent<ParticleSystem>();
		m_speed = .02f;
		m_damage = 1;
		m_health = 10;
		m_pauseAfterHit = 1;
		m_drainRange = 30;
		
		m_animationManager = GetComponent<SpriteAnimationManager>();
        m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
		starting_y = transform.position.y;
		m_time = 0;
		m_timeSinceLast = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		m_time += Time.deltaTime;
		bool inRange = WithinRange();
		if(inRange){
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_idleRightAnimation);
			}
			if (m_time - m_timeSinceLast >= .15){
				m_particleSystem.Play();
				DamagePlayer ();
				m_timeSinceLast = m_time;
			}
		}else{	
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
			}
			m_particleSystem.Stop();
			MoveAround ();
		}
		
	}
	
	void LateUpdate(){
		Vector3 tmp = new Vector3(transform.position.x, 7.25f, transform.position.z);
		transform.position = tmp;
	}
	
	void MoveAround(){
		Vector3 direction = m_playerCharacter.transform.position - transform.position;
		direction.Normalize();
		direction.y = 0;
		m_characterController.Move(direction * m_speed);
	}
	
	void DamagePlayer(){
		m_playerCharacter.Damage(-m_damage);
	}
	
	public void getDamaged(int damage){
        m_health -= damage;
        GameObject notification = (GameObject)Instantiate(HudController.s_singleton.NotificationPrefab);
        Vector3 notificationPos = Camera.main.WorldToScreenPoint(transform.position);
        notification.GetComponent<FloatingText>().Display(damage.ToString(), new Vector2(notificationPos.x, Screen.height - notificationPos.y), 3.0f);

        if (m_health <= 0)
        {
            GameManager.s_singleton.m_catsKilled++;
            GameManager.s_singleton.m_score += 200;
            GameManager.s_singleton.SpawnDeathObject(transform.position);
            Destroy(gameObject);
        }
	}
	
    void OnTriggerEnter(Collider collider) {
		if(m_isHit){
			return;
		}
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
			}
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
            getDamaged(bullet.m_damage);
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

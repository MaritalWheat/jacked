using UnityEngine;
using System.Collections;


public class RabbitBehavior : MonoBehaviour {

    private PlayerCharacter m_playerCharacter;
	private CharacterController m_characterController;
	public GameObject prefab;
	public float m_speed;
	public int m_damage;
	public int m_health;
	public float m_pauseAfterHit;
	
	private bool m_isHit;
	private float m_timeBuffer;
	private float starting_y;
	
	public SpriteAnimation m_idleLeftAnimation;
    public SpriteAnimation m_idleRightAnimation;
    public SpriteAnimation m_walkLeftAnimation;
    public SpriteAnimation m_walkRightAnimation;
    private SpriteAnimationManager m_animationManager;
	
	
	// Use this for initialization
	void Start () {
        m_playerCharacter = PlayerCharacter.s_singleton;
		m_characterController = gameObject.GetComponent<CharacterController>();
		m_speed = .5f;
		m_damage = 5;
		m_health = 10;
		m_timeBuffer = 0;
		m_pauseAfterHit = 1;
		
		m_animationManager = GetComponent<SpriteAnimationManager>();
        m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
		starting_y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_isHit){
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_walkRightAnimation);
			}
			//m_animationManager.SetSpriteAnimation(m_walkLeftAnimation);
			MoveTowardsPlayer();
		}else{
			Vector3 direction = m_playerCharacter.transform.position - transform.position;
			if(direction.x <= 0){
				m_animationManager.SetSpriteAnimation(m_idleLeftAnimation);
			}else{
				m_animationManager.SetSpriteAnimation(m_idleRightAnimation);
			}
			m_timeBuffer += Time.deltaTime;
			if(m_timeBuffer > m_pauseAfterHit){
				setHit (false);	
				m_timeBuffer = 0;
			}
				
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
		m_playerCharacter.Damage(m_damage);
	}
	
	void DieandMultiply(){
        GameManager.s_singleton.SpawnDeathObject(transform.position);

		//Object rabbitChild;
		for (int i = 0; i < 1; i++){
			GameObject baby = (GameObject)Instantiate(prefab, transform.position, transform.rotation);
			baby.transform.parent = transform.parent;
		}
		Destroy (this.gameObject);
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
            DieandMultiply ();
			//Destroy(gameObject);
        }
	}
	
    void OnTriggerEnter(Collider collider) {
		Projectile bullet = collider.gameObject.GetComponent<Projectile>();
        //Debug.Log(bullet);
        if (bullet)
        {
            bullet.Destruct();
			getDamaged(bullet.m_damage);
        }else{
			GameObject hitObject = collider.gameObject;
        	if (hitObject != gameObject) {
            	//Debug.Log(collider.gameObject);
				if (hitObject == m_playerCharacter.gameObject  && !m_isHit){
					//Debug.Log ("got here");
					DamagePlayer ();
					setHit (true);
				}
        	} 
			if(m_isHit){
				return;
			}
		}
		
    }
	
	void setHit(bool hit){
		m_isHit = hit;
	}
}

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public int m_damage;
    public int m_speed;
    public int m_duration;
    public bool m_explodes;

    private float m_spawnTime;
    private CharacterController m_characterController;
    private ProjectileAnimations m_animations;
    private SpriteAnimationManager m_animManager;
    public float m_destructionDuration = .5f;
	// Use this for initialization
	void Start () {
        m_spawnTime = Time.time;
        m_characterController = transform.GetComponent<CharacterController>();
        m_animations = gameObject.GetComponent<ProjectileAnimations>();
        m_animManager = gameObject.GetComponent<SpriteAnimationManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time - m_spawnTime) > m_duration)
        {
            Destroy(this.gameObject);
        }

        //Destruction has started
        if (m_destructionDuration < .5f)
        {
            m_destructionDuration -= Time.deltaTime;
            if (m_destructionDuration < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Vector3 velocity = transform.forward * m_speed;
            m_characterController.SimpleMove(velocity);
        }
	}

    public void Destruct()
    {
        if (m_explodes)
        {
            GameObject explosion = (GameObject)Instantiate(ProjectileManager.s_singleton.m_rocketLauncherExplosion);
            explosion.transform.position = transform.position;

            int damage = m_damage;
            LayerMask enemyMask = LayerMask.NameToLayer("Enemy");
            int layerMask = 1 << (int)enemyMask;

            foreach (RaycastHit hit in  Physics.CapsuleCastAll(gameObject.transform.position, gameObject.transform.position, 5.0f, transform.forward, layerMask)) {
                SquirrelBehavior squirrel = hit.transform.gameObject.GetComponent<SquirrelBehavior>();
                if (squirrel != null)
                {
                    squirrel.getDamaged(damage);
                    return;
                }

                PenguinBehavior penguin = hit.transform.gameObject.GetComponent<PenguinBehavior>();
                if (penguin != null)
                {
                    penguin.getDamaged(damage);
                    return;
                }

                RabbitBehavior rabbit = hit.transform.gameObject.GetComponent<RabbitBehavior>();
                if (rabbit != null)
                {
                    rabbit.getDamaged(damage);
                    return;
                }

                RabbitBehavior babyRabbit = hit.transform.gameObject.GetComponent<RabbitBehavior>();
                if (babyRabbit != null)
                {
                    babyRabbit.getDamaged(damage);
                    return;
                }

                CatBehavior cat = hit.transform.gameObject.GetComponent<CatBehavior>();
                if (cat != null)
                {
                    cat.getDamaged(damage);
                    return;
                }
            }
        }
        m_animManager.SetSpriteAnimation(m_animations.m_destructionAnimation);
        m_destructionDuration -= Time.deltaTime;
        Destroy(gameObject.GetComponent<CharacterController>());
    }
}

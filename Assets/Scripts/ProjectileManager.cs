using UnityEngine;
using System.Collections;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager s_singleton;

    public Projectile m_defaultProjectilePrefab;
    public Projectile m_triShotProjectilePrefab;
    public Projectile m_rocketLauncherPrefab;
    public GameObject m_rocketLauncherExplosion;
    public GameObject m_laserPrefab;

    private GameObject m_projectileHolder;

	// Use this for initialization
	void Start () {
        if (s_singleton == null)
        {
            s_singleton = this;
        }

        m_projectileHolder = new GameObject("ProjectileContainer");
        m_projectileHolder.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public Projectile SpawnProjectile(Projectile projectile, Vector3 position, Quaternion rotation)
    {
        Projectile newProjectile = (Projectile)Instantiate(projectile, position, rotation);
        newProjectile.transform.parent = m_projectileHolder.transform;

        SpriteAnimationManager animManager = newProjectile.GetComponent<SpriteAnimationManager>();
        ProjectileAnimations projectileAnims = newProjectile.GetComponent<ProjectileAnimations>();

        animManager.SetSpriteAnimation(projectileAnims.m_idleAnimation);
        return newProjectile;
    }

    public void SpawnLaser(GameObject prefab, Vector3 position, Vector3 aimDirection) {
        GameObject newLaser = (GameObject)GameObject.Instantiate(ProjectileManager.s_singleton.m_laserPrefab);
        newLaser.transform.parent = m_projectileHolder.transform;
        LineRenderer line = newLaser.GetComponent<LineRenderer>();
        line.SetPosition(0, position);
        line.SetPosition(1, position + (aimDirection * 150));


        int damage = newLaser.GetComponent<LaserObject>().m_damage;
        damage = (int)(damage / 2);

        LayerMask enemyMask = LayerMask.NameToLayer("Enemy");
        int layerMask = 1 << (int)enemyMask;
        RaycastHit[] hits = Physics.RaycastAll(position, aimDirection, Mathf.Infinity, layerMask);
        foreach (RaycastHit hit in hits) {
            SquirrelBehavior squirrel = hit.transform.gameObject.GetComponent<SquirrelBehavior>();
            if (squirrel != null) {
                squirrel.getDamaged(damage);
                return;
            }

            PenguinBehavior penguin = hit.transform.gameObject.GetComponent<PenguinBehavior>();
            if (penguin != null) {
                penguin.getDamaged(damage);
                return;
            }

            RabbitBehavior rabbit = hit.transform.gameObject.GetComponent<RabbitBehavior>();
            if (rabbit != null) {
                rabbit.getDamaged(damage);
                return;
            }

            RabbitBehavior babyRabbit = hit.transform.gameObject.GetComponent<RabbitBehavior>();
            if (babyRabbit != null) {
                babyRabbit.getDamaged(damage);
                return;
            }

            CatBehavior cat = hit.transform.gameObject.GetComponent<CatBehavior>();
            if (cat != null) {
                cat.getDamaged(damage);
                return;
            }

        }
    }
}

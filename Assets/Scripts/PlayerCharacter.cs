using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    Move,
}

public enum PlayerWeapon
{
    Default,
    TriShot,
    Laser
}

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter s_singleton;
    public bool gamePad;
    public float m_characterSpeed;
    public float m_maxCharacterSpeed;
    public int m_heartRate;
    public Vector2 m_orient;
    public Vector3 m_aimDirection;
    public PlayerState m_playerState;
    public PlayerWeapon m_playerWeapon = PlayerWeapon.Default;

    private float m_damageMod;
    private bool m_enabled = false;
    
    private SpriteAnimation m_currentAnimation;

    private const int PLAYER_MAX_HEARTRATE = 225;
    private const int PLAYER_MIN_HEARTRATE = 0;

    //private SpriteAnimations
    private PlayerAnimations m_playerAnimations;
    private SpriteAnimationManager m_spriteAnimationManager;
    private InputManager m_inputManager;
    //Decrease heart rate when it reaches 0, then reset
    private float m_decreaseHeartRate;

	// Use this for initialization
	void Start ()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
        }
        
        m_spriteAnimationManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_playerAnimations = gameObject.GetComponent<PlayerAnimations>();
        m_inputManager = gameObject.GetComponent<InputManager>();

        SetAnimation(m_playerAnimations.m_playerMoveDown);
	}

	// Update is called once per frame
	void Update () {
        if (!m_enabled)
        {
            return;
        }
        if (m_decreaseHeartRate <= 0)
        {
            SlowHeartRate(1);
            m_decreaseHeartRate = .7f;
        }

        m_characterSpeed = m_maxCharacterSpeed * (((float)(m_heartRate +  PLAYER_MIN_HEARTRATE)) / (float)(PLAYER_MAX_HEARTRATE - PLAYER_MIN_HEARTRATE));
        m_damageMod = 2.0f*(((float)(PLAYER_MAX_HEARTRATE - (m_heartRate + PLAYER_MIN_HEARTRATE))) / (float)(PLAYER_MAX_HEARTRATE - PLAYER_MIN_HEARTRATE));


        m_decreaseHeartRate -= Time.deltaTime;
        gameObject.GetComponent<AudioSource>().pitch = Mathf.Max(.85f, m_heartRate / 100.0f);
        Vector2 aimDirection2D;
        if (!gamePad) {
            aimDirection2D = m_inputManager.PlayerToMouse();
        } else {
            aimDirection2D = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
        }
        m_aimDirection = new Vector3(aimDirection2D.x, 0, aimDirection2D.y);

        SpriteAnimation animationToPlay = m_playerAnimations.GetSpriteAnimation(m_playerState, m_aimDirection);
        if (animationToPlay != m_currentAnimation) {
            SetAnimation(animationToPlay);
        }
	}

    public void SetAnimation(SpriteAnimation animation)
    {
        m_spriteAnimationManager.SetSpriteAnimation(animation);
        m_currentAnimation = animation;
    }

    public void SlowHeartRate(int amountToSlow)
    {
        m_heartRate -= amountToSlow;

        if (m_heartRate < PLAYER_MIN_HEARTRATE)
        {
            PlayerDied();
        }
    }

    public void IncreaseHeartRate(int amountToIncrease)
    {
        m_heartRate += amountToIncrease;

        if (m_heartRate > PLAYER_MAX_HEARTRATE)
        {
            PlayerDied();
        }
    }

    public void Damage(int damageAmount)
    {
        IncreaseHeartRate(damageAmount);
        GameManager.s_singleton.SetSpreeStatus(false);
    }

    public void PlayerDied()
    {
        m_enabled = false;
        HudController.s_singleton.Display(false);
        InputManager.Enable(false);
        GameOver.s_singleton.Display();
    }

    public void FireWeapon()
    {
        switch (m_playerWeapon) {
            default:    // Fall-through
            case PlayerWeapon.Default:
                AudioManager.m_singleton.DefaultGun();
                Quaternion startingRotation = Quaternion.LookRotation(m_aimDirection);
                Projectile newProjectile = ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_defaultProjectilePrefab, transform.position, startingRotation);
                break;

            //case PlayerWeapon.RocketLauncher:
            //    Quaternion startingRotationRL = Quaternion.LookRotation(m_aimDirection);
            //    Projectile newRLProjectile = ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_rocketLauncherPrefab, transform.position, startingRotationRL);

            //   break;

            case PlayerWeapon.TriShot:
                AudioManager.m_singleton.TriShotGun();
                Quaternion startingRotation1 = Quaternion.LookRotation(m_aimDirection);
                Projectile newTriProjectile1 = ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation1);
 
                
                Vector3 leftDirection = Quaternion.Euler(0, -25, 0) * m_aimDirection;
                Vector3 RightDirection = Quaternion.Euler(0, 25, 0) * m_aimDirection;

                Quaternion startingRotation2 = Quaternion.LookRotation(leftDirection);
                Projectile newTriProjectile2 = ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation2);

                Quaternion startingRotation3 = Quaternion.LookRotation(RightDirection);
                Projectile newTriProjectile3 = ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation3);
 
                break;

            case PlayerWeapon.Laser:
                AudioManager.m_singleton.LaserGun();
                Vector3 aimDirectionNormalized = new Vector3(m_aimDirection.x, m_aimDirection.y, m_aimDirection.z);
                aimDirectionNormalized.Normalize();
                ProjectileManager.s_singleton.SpawnLaser(ProjectileManager.s_singleton.m_laserPrefab, transform.position, aimDirectionNormalized);
                break;

        }
    }

    public int GetMaxHeartRate()
    {
        return PLAYER_MAX_HEARTRATE;
    }

    public int GetMinHeartRate()
    {
        return PLAYER_MIN_HEARTRATE;
    }

    public float GetDamageMod()
    {
        return m_damageMod;
    }
    public void Stop()
    {
        m_enabled = false;
    }

    public void StartPlayer()
    {
        m_enabled = true;
    }
}

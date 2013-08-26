using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    Move,
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
    public Weapon m_playerWeapon;

    public int experiencePoints { set; get; }
    public int currentlevel { get; set; }
    private int nextLevelXP;

    private float m_damageMod;
    private bool m_enabled = false;
    
    private SpriteAnimation m_currentAnimation;

    private const int k_maxPlayerHeartRate = 225;
    private const int k_minPlayerHeartRate = 0;

    //private SpriteAnimations
    private PlayerAnimations m_playerAnimations;
    private SpriteAnimationManager m_spriteAnimationManager;
    private InputManager m_inputManager;
    //Decrease heart rate when it reaches 0, then reset
    private float m_decreaseHeartRate;

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

        //Definitely a temporary solution
        nextLevelXP = currentlevel * 10;

        if (experiencePoints >= nextLevelXP)
        {
            currentlevel++;
            experiencePoints -= nextLevelXP;
        }

        m_characterSpeed = m_maxCharacterSpeed * (((float)(m_heartRate +  k_minPlayerHeartRate)) / (float)(k_maxPlayerHeartRate - k_minPlayerHeartRate));
        m_damageMod = 2.0f*(((float)(k_maxPlayerHeartRate - (m_heartRate + k_minPlayerHeartRate))) / (float)(k_maxPlayerHeartRate - k_minPlayerHeartRate));


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
	
	public static Vector3 GetAimDirection() {
		return PlayerCharacter.s_singleton.m_aimDirection;
	}
		
    public void SetAnimation(SpriteAnimation animation)
    {
        m_spriteAnimationManager.SetSpriteAnimation(animation);
        m_currentAnimation = animation;
    }

    public void SlowHeartRate(int amountToSlow)
    {
        m_heartRate -= amountToSlow;

        if (m_heartRate < k_minPlayerHeartRate)
        {
            PlayerDied();
        }
    }

    public void IncreaseHeartRate(int amountToIncrease)
    {
        m_heartRate += amountToIncrease;

        if (m_heartRate > k_maxPlayerHeartRate)
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
    	if (m_playerWeapon.m_name.Equals("Default")) {
            AudioManager.m_singleton.DefaultGun();
            Quaternion startingRotation = Quaternion.LookRotation(m_aimDirection);
            ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_defaultProjectilePrefab, transform.position, startingRotation);
		}

        if (m_playerWeapon.m_name.Equals("TriShot")) {
            AudioManager.m_singleton.TriShotGun();
            Quaternion startingRotation1 = Quaternion.LookRotation(m_aimDirection);
            ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation1);

            
            Vector3 leftDirection = Quaternion.Euler(0, -25, 0) * m_aimDirection;
            Vector3 RightDirection = Quaternion.Euler(0, 25, 0) * m_aimDirection;

            Quaternion startingRotation2 = Quaternion.LookRotation(leftDirection);
            ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation2);

            Quaternion startingRotation3 = Quaternion.LookRotation(RightDirection);
            ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_triShotProjectilePrefab, transform.position, startingRotation3);

		}
		/*
            case PlayerWeapon.Laser:
                AudioManager.m_singleton.LaserGun();
                Vector3 aimDirectionNormalized = new Vector3(m_aimDirection.x, m_aimDirection.y, m_aimDirection.z);
                aimDirectionNormalized.Normalize();
                ProjectileManager.s_singleton.SpawnLaser(ProjectileManager.s_singleton.m_laserPrefab, transform.position, aimDirectionNormalized);
                break;

        }*/
    }

    public int GetMaxHeartRate()
    {
        return k_maxPlayerHeartRate;
    }

    public int GetMinHeartRate()
    {
        return k_minPlayerHeartRate;
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
	
	public static void SetPlayerWeapon (Weapon weapon) 
	{
		PlayerCharacter.s_singleton.m_playerWeapon = weapon;
	}

    public int PointsToNextLevel()
    {
        return nextLevelXP;
    }
}

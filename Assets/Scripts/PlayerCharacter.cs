using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
    public int m_heartRate = 100;
    public Vector2 m_orient;
    public Vector3 m_aimDirection;
    public PlayerState m_playerState;
    public Weapon m_playerWeapon;

    public int experiencePoints { set; get; }
	public int skillPoints {get; set;}
    public int currentlevel { get; set; }
    private int nextLevelXP;

    private float m_damageMod;
    private bool m_enabled = false;
    
    private SpriteAnimation m_currentAnimation;

    private const int k_maxPlayerHeartRate = 225;
    private const int k_minPlayerHeartRate = 0;

	private Color m_currentColor = Color.white;
	private Material m_material;
	
	private List<Skill> currentSkills = new List<Skill>();
	
    //private SpriteAnimations
    private PlayerAnimations m_playerAnimations;
    private SpriteAnimationManager m_spriteAnimationManager;
    private InputManager m_inputManager;
    //Decrease heart rate when it reaches 0, then reset
    private float m_decreaseHeartRate;
	private bool m_invulnerable;
	private bool m_ignoreBeatCycle;

	public static bool Invulnerable {
		get { return s_singleton.m_invulnerable; }
		set { s_singleton.m_invulnerable = value; }
	}

	public static bool IgnoreBeatCycle {
		get { return s_singleton.m_ignoreBeatCycle; }
		set { s_singleton.m_ignoreBeatCycle = value; }
	}

	void Start ()
    {
        if (s_singleton == null)
        {
            s_singleton = this;
        }
        
		skillPoints = 10;
		while (currentSkills.Count < 4) {
			currentSkills.Add(null);	
		}
		
        m_spriteAnimationManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_playerAnimations = gameObject.GetComponent<PlayerAnimations>();
        m_inputManager = gameObject.GetComponent<InputManager>();
		m_material = s_singleton.gameObject.renderer.material;
        SetAnimation(m_playerAnimations.m_playerMoveDown);
	}

	void Update () {
        if (!m_enabled)
        {
            return;
        }

		if (m_material.GetColor("_Color") != m_currentColor) {
			m_material.SetColor("_Color", m_currentColor);
		}
		
		//If the timer that tells you to tick down the heart rate has reached zero
        if (m_decreaseHeartRate <= 0)
        {
            SlowHeartRate(1);  //Slow heart rate by 1 bpm
            m_decreaseHeartRate = .2f; //Reset the timer
        }

        //Definitely a temporary solution
        nextLevelXP = currentlevel * 10;

        if (experiencePoints >= nextLevelXP)
        {
            currentlevel++;
			skillPoints++;
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
		if (m_ignoreBeatCycle) return;
        m_heartRate -= amountToSlow;

        if (m_heartRate < k_minPlayerHeartRate)
        {
            PlayerDied();
        }
    }

    public void IncreaseHeartRate(int amountToIncrease)
    {
		if (m_ignoreBeatCycle) return;
        m_heartRate += amountToIncrease;

        if (m_heartRate > k_maxPlayerHeartRate)
        {
            PlayerDied();
        }
    }

    public void Damage(int damageAmount)
    {
		if (PlayerCharacter.Invulnerable) return;
        IncreaseHeartRate(damageAmount*4);
        GameManager.s_singleton.SetSpreeStatus(false);
    }

    public void PlayerDied()
    {
        GameManager.s_singleton.EndGame();
    }

    public void Reset()
    {
        experiencePoints = 0;
        skillPoints = 0;
        currentlevel = 0;
        nextLevelXP = 0;
        m_heartRate = 100;
        currentSkills.Clear();
    }

    public void FireWeapon()
    {
		//Firing the weapon increases the heart rate, for now we will use a uniform value regardless of the weapon
		IncreaseHeartRate(3);
		
    	if (m_playerWeapon.m_name.Equals("Default")) {
            AudioManager.m_singleton.DefaultGun();
            Quaternion startingRotation = Quaternion.LookRotation(m_aimDirection);
			Vector3 pos = transform.position;
            ProjectileManager.s_singleton.SpawnProjectile(ProjectileManager.s_singleton.m_defaultProjectilePrefab, pos, startingRotation);
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
	
	public void newSkillAdded(Skill s) {
		if (currentSkills.Contains(s)) {
			return;	
		}
		int pos = 0;
		foreach (Skill curSkill in currentSkills) {
			if (curSkill == null) {
				Debug.Log("Should be putting it in spot " + pos);
				break;
			}
			pos ++; 
		}
		currentSkills[pos] = s;
	}
	
	public List<Skill> getCurrentSkills() {
		return currentSkills;	
	}

	public static void ChangeColor(Color color) {
		s_singleton.m_currentColor = color;
	}

	public static Color GetColor() {
		return s_singleton.m_currentColor;
	}
}

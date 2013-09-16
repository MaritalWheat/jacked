using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private CharacterController m_characterController;
    private PlayerCharacter m_playerCharacter;
	
	private float m_timeOfLastBullet;
    private Vector2 m_inputVelocity;
    private Vector3 m_lastMouseWorldPos = new Vector3(0, 0, 0);

    private static bool m_enabled = false;
	public static InputManager s_singleton;   
    private const float k_bulletDelay = .25f;

    public static KeyCode LEFT { get; set; }
    public static KeyCode RIGHT { get; set; }
    public static KeyCode UP { get; set; }
    public static KeyCode DOWN { get; set; }

    public const string PREF_UP_KEY = "upKey";
    public const string PREF_DOWN_KEY = "downKey";
    public const string PREF_RIGHT_KEY = "rightKey";
    public const string PREF_LEFT_KEY = "leftKey";



	
	void Start ()
    {
        string tempKey = PlayerPrefs.GetString(PREF_LEFT_KEY);
        LEFT = (tempKey.Length == 0) ? KeyCode.A : (KeyCode) System.Enum.Parse(typeof(KeyCode), tempKey);
        tempKey = PlayerPrefs.GetString(PREF_RIGHT_KEY);
        RIGHT = (tempKey.Length == 0) ? KeyCode.D : (KeyCode)System.Enum.Parse(typeof(KeyCode), tempKey);
        tempKey = PlayerPrefs.GetString(PREF_UP_KEY);
        UP = (tempKey.Length == 0) ? KeyCode.W : (KeyCode)System.Enum.Parse(typeof(KeyCode), tempKey);
        tempKey = PlayerPrefs.GetString(PREF_DOWN_KEY);
        DOWN = (tempKey.Length == 0) ? KeyCode.S : (KeyCode)System.Enum.Parse(typeof(KeyCode), tempKey);

		if (s_singleton == null) {
			s_singleton = this;
		}
        m_characterController = gameObject.GetComponent<CharacterController>();
        m_playerCharacter = gameObject.GetComponent<PlayerCharacter>();

	}
	
	void Update ()
    {
        GetMouseWorldPosition();
		
		if (!m_enabled) {
            return;
        }
		
		// MOVE THIS TO GUI MANAGER
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
			WeaponWheel.DisplayWeaponWheel(!WeaponWheel.GetWeaponWheelDisplayStatus());
		}
		
        m_inputVelocity = Vector2.zero;

        if (m_playerCharacter.gamePad) {
            m_inputVelocity = new Vector2(Input.GetAxis("HorizontalGamepad") * 2, Input.GetAxis("VerticalGamepad") * -2);
        } else {
            m_inputVelocity = new Vector2(GetHorizontal() * 2, GetVertical() * 2);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) {
            if (!GameManager.IsPaused()) {
                GameManager.ShowPauseMenu(!(GameManager.IsPaused()));
            } else {
                GameManager.Resume();
            }
        }
        
        //Check to see if the player fired
        bool fire = false;
        if (m_playerCharacter.gamePad) {
            if (Input.GetAxis("Fire1") < -.5 && Time.time - m_timeOfLastBullet > k_bulletDelay) {
                fire = true;
            }
        } else {
            if (Input.GetAxis("Fire1") > 0 && Time.time - m_timeOfLastBullet > k_bulletDelay) {
                fire = true;
            }
        }
		//Don't fire if mouse is over hud
		fire = fire && !HudController.s_singleton.MouseOverHud();
        if (fire) {
            PlayerCharacter.s_singleton.FireWeapon();
            m_timeOfLastBullet = Time.time;
        }
        

        m_inputVelocity *= m_playerCharacter.m_characterSpeed;

        if (m_inputVelocity == Vector2.zero) {
            m_playerCharacter.m_playerState = PlayerState.Idle;
        } else {
            m_playerCharacter.m_playerState = PlayerState.Move;
        }

        m_characterController.SimpleMove(new Vector3(m_inputVelocity.x, 0.0f, m_inputVelocity.y));
		
		//Check the skill hotkeys, later we should change the hotkeys to be modifiable, for now they are 1 2 3 4
		List<Skill> skills = PlayerCharacter.s_singleton.getCurrentSkills();
		if (Input.GetKey(KeyCode.Alpha1)) {
			if (skills[0] != null) {
				SkillManager.fireSkill(skills[0].skillName);	
			}
		} else if (Input.GetKey(KeyCode.Alpha2)) {
			if (skills[1] != null) {
				SkillManager.fireSkill(skills[1].skillName);	
			}
		} else if (Input.GetKey(KeyCode.Alpha3)) {
			if (skills[2] != null) {
				SkillManager.fireSkill(skills[2].skillName);	
			}
		} else if (Input.GetKey(KeyCode.Alpha4)) {
			if (skills[3] != null) {
				SkillManager.fireSkill(skills[3].skillName);	
			}
		}
	}

    private int GetHorizontal()
    {
        int horizontalValue = 0;

        if (Input.GetKey(LEFT))
            horizontalValue--;

        if (Input.GetKey(RIGHT))
            horizontalValue++;

        return horizontalValue;
    }

    private int GetVertical()
    {
        int verticalValue = 0;

        if (Input.GetKey(UP))
            verticalValue++;

        if (Input.GetKey(DOWN))
            verticalValue--;

        return verticalValue;
    }

    private void UpInput()
    {
        m_inputVelocity += new Vector2(0, 1);
    }

    private void RightInput()
    {
        m_inputVelocity += new Vector2(1, 0);
    }

    private void DownInput()
    {
        m_inputVelocity += new Vector2(0, -1);
    }

    private void LeftInput()
    {
        m_inputVelocity += new Vector2(-1, 0);
    }

    public static void Enable(bool enable)
    {
        m_enabled = enable;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        Vector3 worldPos = Vector3.zero;

        foreach (RaycastHit hit in hits) {
            if (hit.transform.gameObject.name.ToUpper() == "FLOOR")
            {
                worldPos = hit.point;
            }
        }

        if (worldPos != Vector3.zero )
        {
            m_lastMouseWorldPos = worldPos;
            return worldPos;
        }
        else
        {
            return m_lastMouseWorldPos;
        }
    }

    public Vector2 PlayerToMouse()
    {
        Vector3 mPos = GetMouseWorldPosition();
        return new Vector2(mPos.x - m_playerCharacter.transform.position.x, mPos.z - m_playerCharacter.transform.position.z);
    }
}

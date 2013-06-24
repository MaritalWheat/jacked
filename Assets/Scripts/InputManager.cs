using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private CharacterController m_characterController;
    private PlayerCharacter m_playerCharacter;

    private Vector2 m_inputVelocity;

    private Vector3 m_lastMouseWorldPos = new Vector3(0, 0, 0);

    private static bool m_enabled = false;

    private float timeOfLastBullet;
    private const float BULLET_DELAY = .25f;

	// Use this for initialization
	void Start ()
    {
        m_characterController = gameObject.GetComponent<CharacterController>();
        m_playerCharacter = gameObject.GetComponent<PlayerCharacter>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Zero");
        }
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("One");
        }
        if (Input.GetMouseButtonDown(2)) {
            Debug.Log("Two");
        }
        if (Input.GetMouseButtonDown(3)) {
            Debug.Log("Three");
        }

        if (!m_enabled) {
            return;
        }

        m_inputVelocity = Vector2.zero;

        if (m_playerCharacter.gamePad) {
            m_inputVelocity = new Vector2(Input.GetAxis("HorizontalGamepad") * 2, Input.GetAxis("VerticalGamepad") * -2);
        } else {
            m_inputVelocity = new Vector2(Input.GetAxis("Horizontal") * 2, Input.GetAxis("Vertical") * 2);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) {
            if (!GameManager.s_singleton.IsPaused()) {
                GameManager.s_singleton.Pause(!(GameManager.s_singleton.IsPaused()));
            } else {
                GameManager.s_singleton.Resume();
            }
        }
        
        //Check to see if the player fired
        bool fire = false;
        if (m_playerCharacter.gamePad) {
            if (Input.GetAxis("Fire1") < -.5 && Time.time - timeOfLastBullet > BULLET_DELAY) {
                fire = true;
            }
        } else {
            if (Input.GetAxis("Fire1") > 0 && Time.time - timeOfLastBullet > BULLET_DELAY) {
                fire = true;
            }
        }

        if (fire) {
            PlayerCharacter.s_singleton.FireWeapon();
            timeOfLastBullet = Time.time;
        }
        

        m_inputVelocity *= m_playerCharacter.m_characterSpeed;

        if (m_inputVelocity == Vector2.zero) {
            m_playerCharacter.m_playerState = PlayerState.Idle;
        } else {
            m_playerCharacter.m_playerState = PlayerState.Move;
        }

        m_characterController.SimpleMove(new Vector3(m_inputVelocity.x, 0.0f, m_inputVelocity.y));
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

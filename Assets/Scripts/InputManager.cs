using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private CharacterController m_characterController;
    private PlayerCharacter m_playerCharacter;

    private Vector2 m_inputVelocity;

    private Vector3 m_lastMouseWorldPos = new Vector3(0, 0, 0);

    private static bool m_enabled = false;
    
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

        if (!m_enabled) {
            return;
        }

        m_inputVelocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            UpInput();
        }

        if (Input.GetKey(KeyCode.A)) {
            LeftInput();
        }

        if (Input.GetKey(KeyCode.S)) {
            DownInput();
        }

        if (Input.GetKey(KeyCode.D))
        {
            RightInput();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            GameManager.s_singleton.Pause( !(GameManager.s_singleton.IsPaused()));       
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            PlayerCharacter.s_singleton.FireWeapon();
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

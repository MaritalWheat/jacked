using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour
{
    public SpriteAnimation m_playerIdleUp;
    public SpriteAnimation m_playerIdleDown;
    public SpriteAnimation m_playerIdleLeft;
    public SpriteAnimation m_playerIdleRight;

    public SpriteAnimation m_playerIdleUpLeft;
    public SpriteAnimation m_playerIdleUpRight;
    public SpriteAnimation m_playerIdleDownLeft;
    public SpriteAnimation m_playerIdleDownRight;

    public SpriteAnimation m_playerMoveUp;
    public SpriteAnimation m_playerMoveDown;
    public SpriteAnimation m_playerMoveLeft;
    public SpriteAnimation m_playerMoveRight;
    public SpriteAnimation m_playerMoveUpLeft;
    public SpriteAnimation m_playerMoveUpRight;
    public SpriteAnimation m_playerMoveDownLeft;
    public SpriteAnimation m_playerMoveDownRight;

    private const float ANGLE_CAP_UP = 35.0f;
    private const float ANGLE_CAP_UP_DIAG = 75.0f;
    private const float ANGLE_CAP_SIDE = 125.0f;
    private const float ANGLE_CAP_DOWN_DIAG = 165.0f;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public SpriteAnimation GetSpriteAnimation(PlayerState state, Vector3 forwardDirection) {
        SpriteAnimation animation = null;
        switch (state) {
            default: // fall-through
            case PlayerState.Idle:
                animation = GetPlayerIdleAnimation(forwardDirection);
                break;

            case PlayerState.Move:
                animation = GetPlayerMoveAnimation(forwardDirection);
                break;
        }

        return animation;
    }

    private SpriteAnimation GetPlayerIdleAnimation(Vector3 forwardDirection)
    {
        Vector3 worldForward = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(worldForward, forwardDirection);

        Vector3 worldRight = new Vector3(1, 0, 0);
        float x = Vector3.Dot(forwardDirection, worldRight);

        if (x >= 0) {
            if (angle < ANGLE_CAP_UP) {
                return m_playerIdleUp;
            }

            if (angle < ANGLE_CAP_UP_DIAG) {
                return m_playerIdleUpRight;
            }

            if (angle < ANGLE_CAP_SIDE) {
                return m_playerIdleRight;
            }

            if (angle < ANGLE_CAP_DOWN_DIAG) {
                return m_playerIdleDownRight;
            }

            return m_playerIdleDown;
        } else {
            if (angle < ANGLE_CAP_UP) {
                return m_playerIdleUp;
            }

            if (angle < ANGLE_CAP_UP_DIAG) {
                return m_playerIdleUpLeft;
            }

            if (angle < ANGLE_CAP_SIDE) {
                return m_playerIdleLeft;
            }

            if (angle < ANGLE_CAP_DOWN_DIAG) {
                return m_playerIdleDownLeft;
            }

            return m_playerIdleDown;
        }
    }

    private SpriteAnimation GetPlayerMoveAnimation(Vector3 forwardDirection) {
        Vector3 worldForward = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(worldForward, forwardDirection);

        Vector3 worldRight = new Vector3(1, 0, 0);
        float x = Vector3.Dot(forwardDirection, worldRight);

        if (x >= 0) {
            if (angle < ANGLE_CAP_UP) {
                return m_playerMoveUp;
            }

            if (angle < ANGLE_CAP_UP_DIAG) {
                return m_playerMoveUpRight;
            }

            if (angle < ANGLE_CAP_SIDE) {
                return m_playerMoveRight;
            }

            if (angle < ANGLE_CAP_DOWN_DIAG) {
                return m_playerMoveDownRight;
            }

            return m_playerIdleDown;
        } else {
            if (angle < ANGLE_CAP_UP) {
                return m_playerMoveUp;
            }

            if (angle < ANGLE_CAP_UP_DIAG) {
                return m_playerMoveUpLeft;
            }

            if (angle < ANGLE_CAP_SIDE) {
                return m_playerMoveLeft;
            }

            if (angle < ANGLE_CAP_DOWN_DIAG) {
                return m_playerMoveDownLeft;
            }

            return m_playerMoveDown;
        }
    }
}

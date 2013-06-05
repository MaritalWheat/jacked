using UnityEngine;
using System.Collections;

public class GunPickup : MonoBehaviour
{

    public PlayerWeapon m_weapon;


    private SpriteAnimation m_anim;
    private SpriteAnimationManager m_animManager;

    // Use this for initialization
    void Start()
    {
        m_anim = gameObject.GetComponent<SpriteAnimation>();
        m_animManager = gameObject.GetComponent<SpriteAnimationManager>();
        m_animManager.SetSpriteAnimation(m_anim);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerCharacter>())
        {
            GameObject notification = (GameObject)Instantiate(HudController.s_singleton.PowerupNotification);
            notification.GetComponent<PowerupNotification>().DisplayLarge("Recieved New Weapon", 3.0f);
            PlayerCharacter.s_singleton.m_playerWeapon = m_weapon;
            Destroy(gameObject);
        }

    }
}

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioClip test;
    public AudioClip defaultGun;
    public AudioClip laserGun;
    public AudioClip triGun;
    public AudioClip test1;
    private AudioSource heart;

    public static AudioManager m_singleton;

    private AudioSource source;

    public enum EnemyType {
        Cat,Penguin,Rabbit,RabbitChild,Squirrell
    }

	// Use this for initialization
	void Start () {
        if (m_singleton == null)
        {
            m_singleton = this;
        }
        source = gameObject.GetComponent<AudioSource>();
        heart = PlayerCharacter.s_singleton.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayTest()
    {
        source.PlayOneShot(test);
    }

    public void PlayHeartBeat()
    {
        heart.Play();
    }

    public void PlayDeathSound(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Cat:
                break;
            case EnemyType.Squirrell:
                break;
            case EnemyType.Rabbit:
                break;
            case EnemyType.RabbitChild:
                break;
            case EnemyType.Penguin:
                break;
        }
    }

    public void DefaultGun()
    {
        source.PlayOneShot(defaultGun);
    }

    public void LaserGun()
    {
        source.PlayOneShot(laserGun);
    }

    public void TriShotGun()
    {
        source.PlayOneShot(triGun);
    }

}

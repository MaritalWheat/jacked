using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager s_singleton;

    public GameObject PenguinPrefab;
    public GameObject SquirrelPrefab;
    public GameObject RabbitPrefab;
	public GameObject CatPrefab;
    public List<GameObject> m_powerups;
    public GameObject m_deathDecalPrefab;

    public float m_creaturesKilled = 0;
    public float m_catsKilled = 0;
    public float m_score = 0;

    private GameObject m_floor;
    private GameObject m_enemies;
    private GameObject m_deathDecalsHolder;
    private bool m_currentWaveFinished = true;
    private int m_currentWaveNumber = 10;
    private bool m_enabled = false;
    private float m_timeUntilNextWave = 5.0f;
    private bool m_paused = false;
    private GameObject[] m_enemiesList;

    private int m_enemiesLeftToSpawn = 0;
    private const int MAX_ENEMIES_ON_SCREEN = 20;

    private bool onSpree = false;
    private bool killingSpree = false;
    private bool monsterSpree = false;
    private int spreeCount = 0;
    private int startOfSpreeCount;
    private const int k_killingSpree = 5;
    private const int k_monsterSpree = 10;
    private const int k_glorySpree = 15;

    private bool m_slowMoOn = false;
    private float m_slowMoTime = 0;
    private const float k_maxSlowMoTime = .2f;
	
	private bool m_displayWeaponWheel = false;
	private bool m_postWave;

	void Start () {
        
        if (!s_singleton)
        {
            s_singleton = this;
        }
        m_floor = GameObject.Find("Floor");
        m_enemies = GameObject.Find("Enemies");
        m_deathDecalsHolder = new GameObject("DeathDecals");
        m_deathDecalsHolder.transform.parent = transform;
        m_enemiesList = new GameObject[4] { PenguinPrefab, SquirrelPrefab, RabbitPrefab, CatPrefab };
	}

	void Update () {
        if (m_paused  || !Store.CheckStoreClosed()) {
            Time.timeScale = 0f;
        } else if (m_slowMoOn) {
            Time.timeScale = 0.1f;
            m_slowMoOn = SlowMo();
        } else if (m_displayWeaponWheel) {
			Time.timeScale = 0.1f;
		} else {
            Time.timeScale = 1f;
        }

        if (onSpree) {
            if (spreeCount == 0) {
                startOfSpreeCount = (int)m_creaturesKilled;
                spreeCount++;
            }
            else if (spreeCount == k_killingSpree && !killingSpree) {
                AudioManager.m_singleton.PlayKillingSpree();
                killingSpree = true;
                m_slowMoOn = SlowMo();
            }
            else if (spreeCount == k_monsterSpree && !monsterSpree) {
                AudioManager.m_singleton.PlayMonsterSpree();
                monsterSpree = true;
                m_slowMoOn = SlowMo();
            }
            else if (spreeCount >= k_glorySpree) {
                AudioManager.m_singleton.PlayGlorySpree();
                spreeCount = 0;
                onSpree = false;
                killingSpree = false;
                monsterSpree = false;
                Time.timeScale = 0.25f;
                m_slowMoOn = SlowMo();
            }
            else {
                spreeCount = (int)(m_creaturesKilled - startOfSpreeCount);
            }
        }
        if (!m_enabled)
        {
            return;
        }
        if (IsPaused())
        {
            return;
        }

        if (m_enemiesLeftToSpawn != 0 && m_enemies.transform.childCount < MAX_ENEMIES_ON_SCREEN)
        {
            int random = Random.Range(0, m_enemiesList.Length);
            Spawn(m_enemiesList[random]);
            m_enemiesLeftToSpawn--;
        }

        
        if (m_timeUntilNextWave > 0)
        {
            m_timeUntilNextWave -= Time.deltaTime;
        }

       	if (m_currentWaveFinished) {
			m_currentWaveFinished = false;
			StartPostWave();					
		}

	    if (m_enemies.transform.childCount == 0 && !m_currentWaveFinished && !m_postWave)
        {
            m_currentWaveFinished = true;
            m_currentWaveNumber++;

        }

		if (m_postWave) {
			if (Store.CheckStoreClosed()) {
				m_postWave = false;             
                GameObject notification = (GameObject)Instantiate(HudController.s_singleton.ScrollingNotificationPrefab);
                notification.GetComponent<ScrollingText>().Display("Begin Wave " + m_currentWaveNumber + " !", 10.0f, HudController.s_singleton.m_largeFightingSpirit);
                m_timeUntilNextWave = 3.0F;
				StartWave();
				Debug.Log ("Got here.");	
			}
		}
	}

    public void EndGame()
    {
        Enable(false);
        HudController.s_singleton.Display(false);
        InputManager.Enable(false);
        
        GameOver.s_singleton.Display();
        PlayerCharacter.s_singleton.Reset();
    }
	
	private void StartPostWave() {
		Store.DisplayStore();
		m_postWave = true;
	}
	
    private bool SlowMo() {
        if (m_slowMoTime <= k_maxSlowMoTime) {
            m_slowMoTime += Time.deltaTime;
            return true;
        } 
        else {
            m_slowMoTime = 0;
            return false;
        }
    }
	
		
	private bool RoundBreak() {
		return Store.CheckStoreClosed();
	}
	
    private void StartWave()
    {

        // Clear last wave decals
        int numDecals = m_deathDecalsHolder.transform.GetChildCount();
        for (int index = 0; index < numDecals; index++) {
            Transform childTransform = m_deathDecalsHolder.transform.GetChild(index);
            GameObject childGameObject = childTransform.gameObject;
            childGameObject.GetComponent<DeathAnimationScript>().FadeOutToDestroy();
        }

        m_enemiesLeftToSpawn = NumberOfEnemies();
    }
    
    private GameObject Spawn(GameObject prefab)
    {
        Vector3 spawnPos = new Vector3();
        spawnPos.y = m_floor.transform.position.y + 4;
        if (Random.value - .5 > 0)
        {
            spawnPos.x = Random.Range(.5f, .9f) * m_floor.GetComponent<BoxCollider>().bounds.extents.x;
        }
        else
        {
            spawnPos.x = Random.Range(-.9f, -.5f) * m_floor.GetComponent<BoxCollider>().bounds.extents.x;
        }
        if (Random.value - .5 > 0)
        {
            spawnPos.z = Random.Range(.5f, .9f) * m_floor.GetComponent<BoxCollider>().bounds.extents.z;
        }
        else
        {
            spawnPos.z = Random.Range(-.9f, -.5f) * m_floor.GetComponent<BoxCollider>().bounds.extents.z;
        }
        GameObject enemy = (GameObject)Instantiate(prefab);
        //enemy.transform.position = m_floor.transform.InverseTransformPoint(spawnPos);
        enemy.transform.position = spawnPos;
        enemy.transform.parent = m_enemies.transform;
        return enemy;
    }

    private void SpawnPowerup()
    {
        Vector3 spawnPos = new Vector3();
        spawnPos.y = m_floor.transform.position.y + 4;
        if (Random.value - .5 > 0)
        {
            spawnPos.x = Random.Range(0f, .6f) * m_floor.GetComponent<BoxCollider>().bounds.extents.x;
        }
        else
        {
            spawnPos.x = Random.Range(-.6f, 0f) * m_floor.GetComponent<BoxCollider>().bounds.extents.x;
        }
        if (Random.value - .5 > 0)
        {
            spawnPos.z = Random.Range(0f, .6f) * m_floor.GetComponent<BoxCollider>().bounds.extents.z;
        }
        else
        {
            spawnPos.z = Random.Range(-.6f, 0f) * m_floor.GetComponent<BoxCollider>().bounds.extents.z;
        }
        GameObject pUp = (GameObject)Instantiate(m_powerups[Random.Range(0,m_powerups.Count)]);
        //enemy.transform.position = m_floor.transform.InverseTransformPoint(spawnPos);
        pUp.transform.position = spawnPos;
    }

    public void SpawnDeathObject(Vector3 position) {
        GameObject newDeathObject = (GameObject)Instantiate(m_deathDecalPrefab, position, Quaternion.identity);
        newDeathObject.transform.parent = m_deathDecalsHolder.transform;
    }

    public void Enable(bool enable)
    {
        if (enable)
        {
            PlayerCharacter.s_singleton.StartPlayer();
            m_timeUntilNextWave = 5.0f;
        }
        m_enabled = enable;
    }

    private int NumberOfEnemies()
    {
        return m_currentWaveNumber;
    }

    public static bool IsPaused()
    {
        return GameManager.s_singleton.m_paused;
    }

	/// Shows the pause menu and pauses the game
    public static void ShowPauseMenu(bool enable)
    {
		if (MainMenu.MainMenuDisplayed() != enable) {
        	MainMenu.DisplayMainMenu(enable);
        	PauseGame(enable);
		}
    }
	
	public static void PauseGame(bool enable) {
		GameManager.s_singleton.m_paused = enable;
	}

    public static void Resume()
    {
        MainMenu.DisplayMainMenu(false);
		PauseGame(false);
    }

    public void Reset()
    {
        PlayerCharacter.s_singleton.Reset();
		SkillManager.Reset();
		CoroutineHandler.Reset();

        foreach (Transform child in m_enemies.transform)
        {
            Destroy(child.gameObject);
        }

        m_currentWaveFinished = true;
        m_currentWaveNumber = 1;
        m_score = 0;
        m_creaturesKilled = 0;
    }

    public void SetSpreeStatus(bool status) {
        if (status == false) {
            spreeCount = 0;
            killingSpree = false;
            monsterSpree = false;
        }
        onSpree = status;
    }
	
	public static void DisplayWeaponWheel(bool display) {
		GameManager.s_singleton.m_displayWeaponWheel = display;
	}
}

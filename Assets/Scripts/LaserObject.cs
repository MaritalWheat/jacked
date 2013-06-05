using UnityEngine;
using System.Collections;

public class LaserObject : MonoBehaviour {
    public float m_displayTime;
    public int m_damage;

    private float m_spawnTime;

	// Use this for initialization
	void Start () {
        m_spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if ((Time.time - m_spawnTime) > m_displayTime) {
            Destroy(gameObject);
        }
	}
}

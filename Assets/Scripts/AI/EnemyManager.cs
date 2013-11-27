using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private static EnemyManager m_singleton;
	private List<EnemyBehavior> m_enemies;

	public static List<EnemyBehavior> Enemies {
		get {
			return m_singleton.m_enemies;
		}

		set {
			m_singleton.m_enemies = value;
		}
	}

	void Start () {
		if (m_singleton == null) {
			m_enemies = new List<EnemyBehavior>();
			m_singleton = this;
		}
	}
}

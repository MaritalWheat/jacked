using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoroutineHandler : MonoBehaviour {

	private static CoroutineHandler m_singleton;
	private Dictionary<int, CoroutineInstance> m_coroutines;
	private int m_assignableKey = 0;

	void Start () {
		if (m_singleton == null) {
			m_singleton = this;
			m_coroutines = new Dictionary<int, CoroutineInstance>();
		}
	}

	public static void Reset() {
		for (int i = 0; i < m_singleton.m_coroutines.Count; i++) {
			TakeDown(m_singleton.m_coroutines[i].m_key);
		}
		m_singleton.m_coroutines = new Dictionary<int, CoroutineInstance>();
	}

	public static int StartCoroutine(Func<int, int> executable) {
		GameObject tmp = new GameObject();
		tmp.transform.parent = m_singleton.transform;
		tmp.name = "Coroutine Instance";
		tmp.AddComponent<CoroutineInstance>();
		CoroutineInstance instance = tmp.GetComponent<CoroutineInstance>();
		instance.m_key = m_singleton.m_assignableKey;
		m_singleton.m_assignableKey++;
		m_singleton.m_coroutines.Add(instance.m_key, instance);
		instance.StartCoroutine("DoCoroutine", executable);
		//m_singleton.StartCoroutine("DoCoroutine", executable);
		return instance.m_key;
	}

	public static void TakeDown(int key) {
		CoroutineInstance instance;
		m_singleton.m_coroutines.TryGetValue(key, out instance);
		if (instance == null) return; //throw error?
		m_singleton.m_coroutines.Remove(key);
		instance.StopCoroutine("DoCoroutine");
		Destroy(instance.gameObject);
	}
}

public class CoroutineInstance : MonoBehaviour {
	public int m_key;

	IEnumerator DoCoroutine(Func<int, int> executable) {
		int rc = 0; //return code from function
		while (rc != 1) {
			rc = executable(1);
			yield return new WaitForSeconds(0.1f);
		}
		CoroutineHandler.TakeDown(this.m_key);
	}
}

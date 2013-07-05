using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {
	
	public static WeaponManager m_singleton;	
	public List<Weapon> m_weapons;
	private Dictionary<string, Weapon> m_weaponsDict;
	
	void Start () {
		m_weaponsDict = new Dictionary<string, Weapon>();
		if (m_singleton == null) {
			m_singleton = this;
		}
		for (int i = 0; i < m_weapons.Count; i++) {
			//m_weapons[i].SetIndex(i);
			m_weaponsDict.Add(m_weapons[i].m_name, m_weapons[i]);
		}
		PlayerCharacter.SetPlayerWeapon(WeaponManager.GetWeapon("Default"));			
	}
	
	void Update () {
	
	}
	
	public static Weapon GetWeapon(string name) 
	{
		Weapon value;
		WeaponManager.m_singleton.m_weaponsDict.TryGetValue(name, out value);
		return value;
	}
		
}

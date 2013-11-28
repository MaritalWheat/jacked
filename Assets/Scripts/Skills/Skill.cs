using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {
    public bool m_passive;
    public Skill m_preReq;
    public string m_name;
    public int m_cost;
    public List<Modifier> m_mods;
	public Texture m_icon;
	public string m_description;

	
	public float m_cooldownTime;
	protected float m_cooldownTimer;

	public virtual bool Locked {
		get; set;
	}

	public virtual bool Cooldown {
		get; set;
	}

	public virtual bool Activated {
		get; set;
	}

	public int Level {get; set;}
	
	//Do not use start and update since this is currently not on a gameobject it will not be called
	
	public int GetNextCost() 
	{
	//Maybe this should be based on level later
		return 1;
	}
	
	/// <summary>
	/// Execute the skill, this can be overriden by children for special skills
	/// </summary>
	public virtual void Execute() 
	{

	}

	public virtual int CooldownTimer(int arg) {
		m_cooldownTimer += Time.deltaTime;
		if (m_cooldownTimer > m_cooldownTime) {
			Locked = false;
			Cooldown = false;
			return 1;
		}
		return 0;
	}

	public virtual void StartCooldownTimer() {
		if (!Cooldown) {
			Cooldown = true;
		} else {
			return;
		}

		m_cooldownTimer = 0.0f;
		Func<int, int> executable = CooldownTimer;
		CoroutineHandler.StartCoroutine(executable);
	}

	public float GetCooldownTime() {
		return m_cooldownTimer;
	}
}

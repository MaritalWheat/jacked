using UnityEngine;
using System;
using System.Collections;

public class TimedSkill : Skill {
	[HideInInspector]
	public float m_timer = 0.0f;
	public float m_duration;

	public virtual int Timer(int arg) {
		m_timer += Time.deltaTime;
		Debug.Log(m_timer);
		if (m_timer > m_duration) {
			StartCooldownTimer();
			Locked = false;
			Finish();
			return 1;
		}
		return 0;
	}

	public virtual void StartTimer() {
		m_timer = 0.0f;
		Func<int, int> executable = Timer;
		CoroutineHandler.StartCoroutine(executable);
	}

	public override int CooldownTimer(int arg) {
		m_cooldownTimer += Time.deltaTime;
		if (m_cooldownTimer > m_cooldownTime) {
			Cooldown = false;
			return 1;
		}
		return 0;
	}
}

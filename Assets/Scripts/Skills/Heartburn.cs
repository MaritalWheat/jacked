using UnityEngine;
using System;
using System.Collections;

public class Heartburn : Skill {
	private bool m_activated;
	private int m_routineHandle;

	public override void Execute() 
	{
		if (!m_activated) {
			m_activated = true;
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", Color.red);
			//SkillManager.m_singleton.StartCoroutine("Activated");
			Func<int, int> executable = SlowHeartRate;
			m_routineHandle = CoroutineHandler.StartCoroutine(executable);
		} else {
			m_activated = false;
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", Color.white);
			//SkillManager.m_singleton.StopCoroutine("Activated");
			CoroutineHandler.TakeDown(m_routineHandle);
		}
	}

	public int SlowHeartRate(int arg) {
		PlayerCharacter.s_singleton.SlowHeartRate(1);
		return 0;
		//PlayerCharacter.s_singleton.SlowHeartRate(1);
	}
}

using UnityEngine;
using System.Collections;


public class SurvivalInstinct : TimedSkill {
	
	public override void Execute() 
	{
		if (Locked || Cooldown) return;
		if (!Activated) {
			Activated = true;
			for (int i = 0; i < EnemyManager.Enemies.Count; i++) {
				EnemyManager.Enemies[i].m_speed = EnemyManager.Enemies[i].GetBaseSpeed() / 3;
			}
			PlayerCharacter.s_singleton.m_maxCharacterSpeed += 50.0f;

			StartTimer();
			Locked = true;
		}
	}

	public override void Finish() 
	{
		if (Locked) return;
		Activated = false;
		for (int i = 0; i < EnemyManager.Enemies.Count; i++) {
			EnemyManager.Enemies[i].m_speed = EnemyManager.Enemies[i].GetBaseSpeed();
		}
		PlayerCharacter.s_singleton.m_maxCharacterSpeed -= 50.0f;
	}
}

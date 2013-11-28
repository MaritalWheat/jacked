using UnityEngine;
using System.Collections;

//passive ability that activates if health drops below a certain percentage, increasing player speed while
//slowing down enemies

public class SurvivalInstinct : TimedSkill {
	
	public override void Execute() 
	{
		if (Locked) return;
		if (!Activated) {
			Activated = true;
			for (int i = 0; i < EnemyManager.Enemies.Count; i++) {
				EnemyManager.Enemies[i].m_speed = EnemyManager.Enemies[i].GetBaseSpeed() / 3;
			}
			PlayerCharacter.s_singleton.m_maxCharacterSpeed += 50.0f;

			StartTimer();
			Locked = true;
		} else {
			Activated = false;
			for (int i = 0; i < EnemyManager.Enemies.Count; i++) {
				EnemyManager.Enemies[i].m_speed = EnemyManager.Enemies[i].GetBaseSpeed();
			}
			PlayerCharacter.s_singleton.m_maxCharacterSpeed -= 50.0f;
		}
	}
}

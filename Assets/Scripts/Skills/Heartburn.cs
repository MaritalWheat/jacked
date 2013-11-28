using UnityEngine;
using System;
using System.Collections;

public class Heartburn : Skill {
	private int m_routineHandle;

	public override void Execute() 
	{
		if (!Activated && !Locked) {
			Activated = true;
			Color tmp = PlayerCharacter.GetColor();
			tmp = new Color(1.0f, 0.0f, 0.0f, tmp.a);
			PlayerCharacter.ChangeColor(tmp);
			Func<int, int> executable = SlowHeartRate;
			m_routineHandle = CoroutineHandler.StartCoroutine(executable);
			Locked = true;
		} else {
			Activated = false;
			Color tmp = Color.white;
			tmp.a = PlayerCharacter.GetColor().a;
			PlayerCharacter.ChangeColor(tmp);
			CoroutineHandler.TakeDown(m_routineHandle);
			StartCooldownTimer();
		}
	}

	public int SlowHeartRate(int arg) {
		PlayerCharacter.s_singleton.SlowHeartRate(1);
		return 0;
	}
}

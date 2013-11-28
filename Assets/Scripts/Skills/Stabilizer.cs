using UnityEngine;
using System.Collections;

//temporarily freezes heart rate to current
public class Stabilizer : TimedSkill {
	private bool m_activated;
	
	public override void Execute() 
	{
		if (Locked) return;
		if (!m_activated) {
			m_activated = true;
			PlayerCharacter.IgnoreBeatCycle = true;
			StartTimer();
			Locked = true;
		} else {
			m_activated = false;
			PlayerCharacter.IgnoreBeatCycle = false;
		}
	}
}

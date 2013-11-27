using UnityEngine;
using System.Collections;

//temporarily freezes heart rate to current
public class Stabilizer : Skill {
	private bool m_activated;
	
	public override void Execute() 
	{
		if (!m_activated) {
			m_activated = true;
			PlayerCharacter.IgnoreBeatCycle = true;
		} else {
			m_activated = false;
			PlayerCharacter.IgnoreBeatCycle = false;
		}
	}
}

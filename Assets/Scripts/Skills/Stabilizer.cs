using UnityEngine;
using System.Collections;

//temporarily freezes heart rate to current
public class Stabilizer : TimedSkill {
	
	public override void Execute() 
	{
		if (Locked) return;
		if (!Activated) {
			Activated = true;
			PlayerCharacter.IgnoreBeatCycle = true;
			StartTimer();
			Locked = true;
		} else {
			Activated = false;
			PlayerCharacter.IgnoreBeatCycle = false;
		}
	}
}

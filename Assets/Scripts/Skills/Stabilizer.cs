using UnityEngine;
using System.Collections;

//temporarily freezes heart rate to current
public class Stabilizer : TimedSkill {
	
	public override void Execute() 
	{
		if (Locked || Cooldown) return;
		if (!Activated) {
			Activated = true;
			PlayerCharacter.IgnoreBeatCycle = true;
			StartTimer();
			Locked = true;
		}
	}

	public override void Finish() 
	{
		if (Locked) return;
		Activated = false;
		PlayerCharacter.IgnoreBeatCycle = false;
	}
}

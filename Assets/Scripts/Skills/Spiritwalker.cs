using UnityEngine;
using System;
using System.Collections;

public class Spiritwalker : TimedSkill {
	private GrayscaleEffect m_fx;

	public override void Execute() 
	{
		if (Locked || Cooldown) return;
		if (!Activated) {
			Activated = true;
			Color tmp = PlayerCharacter.GetColor();
			tmp.a = 0.25f;
			PlayerCharacter.ChangeColor(tmp);
			PlayerCharacter.Invulnerable = true;
			Physics.IgnoreLayerCollision(8, 10, true);
			m_fx = Camera.main.GetComponent<GrayscaleEffect>();
			m_fx.enabled = true;

			StartTimer();
			Locked = true;
		}
	}

	public override void Finish() 
	{
		if (Locked) return;
		Activated = false;
		Color tmp = PlayerCharacter.GetColor();
		tmp.a = 1.0f;
		PlayerCharacter.ChangeColor(tmp);
		PlayerCharacter.Invulnerable = false;
		Physics.IgnoreLayerCollision(8, 10, false);
		m_fx.enabled = false;
	}
}

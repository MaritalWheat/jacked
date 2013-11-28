using UnityEngine;
using System;
using System.Collections;

public class Spiritwalker : TimedSkill {
	private bool m_activated;
	private Vector4 m_originalColor;
	private GrayscaleEffect m_fx;

	public override void Execute() 
	{
		if (Locked) return;
		if (!m_activated) {
			m_activated = true;
			m_originalColor = PlayerCharacter.s_singleton.gameObject.renderer.material.GetColor("_Color");
			Color newColor = new Color(m_originalColor.x, m_originalColor.y, m_originalColor.z, 0.25f);
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", newColor);
			PlayerCharacter.Invulnerable = true;
			Physics.IgnoreLayerCollision(8, 10, true);
			m_fx = Camera.main.GetComponent<GrayscaleEffect>();
			m_fx.enabled = true;

			StartTimer();
			Locked = true;
		} else {
			m_activated = false;
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", m_originalColor);
			PlayerCharacter.Invulnerable = false;
			Physics.IgnoreLayerCollision(8, 10, false);
			m_fx.enabled = false;
		}
	}
}

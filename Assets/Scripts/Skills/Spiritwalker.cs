using UnityEngine;
using System.Collections;

public class Spiritwalker : Skill {
	private bool m_activated;
	private Vector4 m_originalColor;
	
	public override void Execute() 
	{
		if (!m_activated) {
			m_activated = true;
			m_originalColor = PlayerCharacter.s_singleton.gameObject.renderer.material.GetColor("_Color");
			Color newColor = new Color(m_originalColor.x, m_originalColor.y, m_originalColor.z, 0.25f);
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", newColor);
			PlayerCharacter.Invulnerable = true;
			Physics.IgnoreLayerCollision(8, 10, true);
			//SkillManager.m_singleton.StartCoroutine("Activated");
		} else {
			m_activated = false;
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", m_originalColor);
			PlayerCharacter.Invulnerable = false;
			Physics.IgnoreLayerCollision(8, 10, false);
			//SkillManager.m_singleton.StopCoroutine("Activated");
		}
	}
}
